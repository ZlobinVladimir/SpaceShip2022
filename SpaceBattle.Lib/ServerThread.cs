namespace SpaceBattle.Lib;

using Hwdtech;

public class ServerThread
{
    bool stop = false;
    Thread thread;
    IReceiver queue;

    Action strategy;

    public ServerThread(IReceiver receiver)
    {
        this.queue = receiver;

        this.strategy = () =>
        {
            CommandHandler();
        };

        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy.Invoke();
            }
        });
    }
    public void Start()
    {
        thread.Start();
    }

    internal void Stop()
    {
        stop = true;
    }

    public bool ThreadIsEmpty()
    {
        return queue.isEmpty();
    }

    public bool ThreadEqual(Thread secondThread)
    {
        return this.thread == secondThread;
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    internal void CommandHandler()
    {
       ICommand command = this.queue.Receive();
        try
        {
            command.Execute();
        }
        catch (Exception e)
        {
            var exceptionCommand = IoC.Resolve<ICommand>("HandleException", e, command);
            exceptionCommand.Execute();
        }
    }
    public bool GetStop()
    {
        return this.stop;
    }
}
