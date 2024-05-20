using Hwdtech;
namespace SpaceBattle.Lib;

public class SoftStopStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        var id = args[0];
        var thread = IoC.Resolve<ServerThread>("ServerThreadGetByID", id);
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", id);
        if (args.Length > 1)
        {
            Action act1 = (Action)args[1];
            var softStopCommand = IoC.Resolve<ICommand>("CommandForSoftStopStrategy", thread, act1);
            return softStopCommand;
        }
        else
        {
            var softStopCommand = IoC.Resolve<ICommand>("CommandForSoftStopStrategy", thread);
            return softStopCommand;
        }
    }
}
