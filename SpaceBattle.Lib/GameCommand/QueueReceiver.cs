namespace SpaceBattle.Lib;

public class QueueReceiver : IReceiver
{
    Queue<ICommand> queue;
    public QueueReceiver(Queue<ICommand> queue)
    {
        this.queue = queue;
    }
    public ICommand Receive()
    {
        return queue.Dequeue();
    }
    public bool isEmpty()
    {
        return queue.Count == 0;
    }
}
