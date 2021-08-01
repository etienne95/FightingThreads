# FightingThreads
Console application that runs threads that consume a custom concurrent queue

## How to use it?
1. In the root repository folder, run command `dotnet run`

## Expected result
Something like this:
```
Thread 't2' has dequeued the item named '26'.
Thread 't1' has dequeued the item named '27'.
Thread 't2' has dequeued the item named '28'.
Thread 't1' has dequeued the item named '29'.
Thread 't2' has dequeued the item named '30'.
Thread 't1' has dequeued the item named '31'.
Thread 't2' has dequeued the item named '32'.
Thread 't1' has dequeued the item named '33'.
Thread 't2' has dequeued the item named '34'.
Thread 't2' has been interrupted.
Thread 't1' has been interrupted.
```
*The number of logs depend of constant `ITEMS_BACTH`, defined in the **Program.cs** class*
