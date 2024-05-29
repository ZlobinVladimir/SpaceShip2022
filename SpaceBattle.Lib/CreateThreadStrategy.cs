using Hwdtech;
namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class CreateThreadStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();
        var sender = new SenderAdapter(queue);
        if (args.Length > 1)
        {
            sender.Push(new ActionCommand((Action)args[1]));
        }
        var receiveradapter = IoC.Resolve<ReceiverAdapter>("CreateReceiverAdapter", queue);
        var thread = IoC.Resolve<ServerThread>("CreateThreadData", (string)args[0], sender, receiveradapter);
        return thread;
    }
}
