using Hwdtech;
using System.Diagnostics;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    IReceiver receiver;
    object scope;
    Stopwatch time;

    public GameCommand(object scope, IReceiver receiver)
    {
        this.scope = scope;
        this.receiver = receiver;
        time = new Stopwatch();
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        var gameTick = IoC.Resolve<int>("GetTick");
        time.Start();
        while (time.ElapsedMilliseconds <= gameTick)
        {
            if (!receiver.isEmpty())
            {
                var cmd = receiver.Receive();
                try
                {
                    cmd.Execute();
                }
                catch (Exception err)
                {
                    var exceptinHandlerStrategy = IoC.Resolve<IStrategy>("HandleException", cmd, err);
                    exceptinHandlerStrategy.StartStrategy();
                }
            }
            else break;
        }
        time.Reset();
    }
}
