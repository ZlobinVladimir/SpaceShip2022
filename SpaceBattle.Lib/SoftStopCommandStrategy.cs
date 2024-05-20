namespace SpaceBattle.Lib;

public class CommandForSoftStopStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        var thread = (ServerThread)args[0];
        {
            Action act = new Action(() =>
            {
                if (!thread.ThreadIsEmpty())
                {
                    thread.CommandHandler();
                }
                else
                {
                    new ThreadStopper(thread).Execute();
                    if (args.Length > 1)
                    {
                        Action act1 = (Action)args[1];
                        act1();
                    }
                }
            });
            return new BehaviourUpdater((ServerThread)args[0], act);
        }
    }
}
