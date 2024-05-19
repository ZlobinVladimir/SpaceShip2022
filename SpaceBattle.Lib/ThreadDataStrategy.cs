using Hwdtech;
namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class CreateThreadDataStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        var senderDict = IoC.Resolve<ConcurrentDictionary<string, ISender>>("ThreadIDSenderMapping");
        senderDict.TryAdd((string)args[0], (ISender)args[1]);
        var MT = new ServerThread((IReceiver)args[2]);
        MT.Start();
        var threadDict = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("ThreadIDMyThreadMapping");
        threadDict.TryAdd((string)args[0], MT);
        return MT;
    }
}
