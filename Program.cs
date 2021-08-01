using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

public class Program
{
    static CustomQueue<string> items;
    static List<Thread> threads;
    const int ITEMS_BATCH = 500;

    public static void Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        try
        {
            items = new CustomQueue<string>();
            items.ItemEnqueued += OnItemEnqueued;

            var t1 = new Thread(() => Consume(cts.Token));
            t1.Name = "t1";
            var t2 = new Thread(() => Consume(cts.Token));
            t2.Name = "t2";

            threads = new List<Thread>() { t1, t2 };

            // First batch of data
            for (int i = 0; i < ITEMS_BATCH; i++)
            {
                items.Enqueue(i.ToString());
            }

            threads.ForEach(t => t.Start());

            // Wait some time for the threads process
            Thread.Sleep(2500);

            // Second batch of data
            for (int i = ITEMS_BATCH; i < ITEMS_BATCH * 2; i++)
            {
                items.Enqueue(i.ToString());
            }

            // Wait forthe threads to end they job
            Thread.Sleep(2500);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"The program has failed with error: {ex.Message}");
        }
        finally
        {
            // Kill associated subprocess with this CancellationToken
            cts.Cancel();

            items.Enqueue("to force threads to wake up and process the cancellation token");
        }
    }

    static void OnItemEnqueued(object sender, EventArgs e)
    {
        foreach (var waitingThread in threads.Where(t => t.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
        {
            waitingThread.Interrupt();
        }
    }

    public static void Consume(CancellationToken cancelToken)
    {
        while (true)
        {
            if (cancelToken.IsCancellationRequested) return;

            try
            {
                if (items.Count() > 0)
                {
                    var result = items.Dequeue();
                    Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' has dequeued the item named '{result}'.");
                }
                else
                {
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' has been interrupted.");
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' has failed with error '{ex.Message}'.");
            }
        }
    }
}

class CustomQueue<T>
{
    private List<T> _items;
    private Mutex _mutex;
    public event EventHandler ItemEnqueued;

    public CustomQueue()
    {
        _items = new List<T>();
        _mutex = new Mutex();
    }

    public void Enqueue(T item)
    {
        _mutex.WaitOne();

        _items.Add(item);
        ItemEnqueued?.Invoke(this, EventArgs.Empty);

        _mutex.ReleaseMutex();
    }

    public T Dequeue()
    {
        _mutex.WaitOne();

        if (_items.Count == 0) return default(T);

        var item = _items.ElementAt(0);
        _items.Remove(item);

        _mutex.ReleaseMutex();

        return item;
    }

    public int Count() => _items.Count;
}