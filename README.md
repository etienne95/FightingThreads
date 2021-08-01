# FightingThreads
Console application that runs threads that consume a custom concurrent queue

## How to use it?
1. In the root repository folder, run command `dotnet run`

## Expected result
Something like this:
```
Thread 't1' has dequeued the item named '1'.
Thread 't1' has dequeued the item named '2'.
Thread 't1' has dequeued the item named '3'.
Thread 't1' has dequeued the item named '4'.
Thread 't1' has dequeued the item named '5'.
Thread 't2' has been interrupted.
Thread 't1' has been interrupted.
```
