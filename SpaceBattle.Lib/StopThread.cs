namespace SpaceBattle.Lib;

public class ThreadStopper: ICommand
{
    ServerThread thread;
    public ThreadStopper(ServerThread thread)
    {
        this.thread = thread;
    }

    public void Execute()
    {
        if(thread.Equals(Thread.CurrentThread))
        {
            thread.Stop();
        }
        else
        {
            throw new Exception();
        }
    }
}
