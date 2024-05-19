namespace SpaceBattle.Lib;

public class BehaviourUpdater: ICommand
{
    ServerThread thread;
    Action action;
    public BehaviourUpdater(ServerThread thread, Action action)
    {
        this.thread = thread;
        this.action = action;
    }
    public void Execute()
    {
        if (thread.ThreadEqual(Thread.CurrentThread))
        {
            thread.UpdateBehaviour(action);
        }
        else
        {
            throw new Exception();
        }
    }
}
