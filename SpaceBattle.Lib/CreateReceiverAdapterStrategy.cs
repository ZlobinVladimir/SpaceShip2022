namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class CreateReceiverAdapterStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        var commands = (BlockingCollection<ICommand>)args[0];
        if (args.Length > 1)
        {
            commands.Append(new ActionCommand((Action)args[1]));
        }
        IReceiver queue = new ReceiverAdapter(commands);
        return queue;
    }
}
