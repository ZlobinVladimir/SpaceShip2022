using Hwdtech;
namespace SpaceBattle.Lib;

public class ContinuousOperation : IStrategy{
    public object Run(params object[] args){
        IUObject obj = (IUObject)args[1];
        var name = (string)args[0];
        var queue = IoC.Resolve<Queue<ICommand>>("Game.Queue", obj);
        var createcom = IoC.Resolve<ICommand>("Create.Command", name, obj);
        var contop = IoC.Resolve<ICommand>("Commands.Repeat", queue, createcom);
        return contop;
    }
}
