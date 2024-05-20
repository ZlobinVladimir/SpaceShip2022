using Hwdtech.Ioc;
using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class CreateThreadTests
{
    public CreateThreadTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var threadDict = new ConcurrentDictionary<string, ServerThread>();
        var senderDict = new ConcurrentDictionary<string, ISender>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadIDMyThreadMapping", (object[] _) => threadDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadIDSenderMapping", (object[] _) => senderDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderAdapterGetByID", (object[] id) => senderDict[(string)id[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadGetByID", (object[] id) => threadDict[(string)id[0]]).Execute();

        var createAndStartThreadStrategy = new CreateThreadDataStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateThreadData", (object[] args) => createAndStartThreadStrategy.StartStrategy(args)).Execute();
        var createReceiverAdapterStrategy = new CreateReceiverAdapterStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateReceiverAdapter", (object[] args) => createReceiverAdapterStrategy.StartStrategy(args)).Execute();
    }
    [Fact]
    public void MyThreadCreateTest()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>(100);
        var sender = new SenderAdapter(queue);
        var receiver = IoC.Resolve<IReceiver>("CreateReceiverAdapter", queue);
        var thread = IoC.Resolve<ServerThread>("CreateThreadData", "1", sender, receiver);
        Assert.True(thread.ThreadIsEmpty());
        Assert.False(thread.GetStop());
        Assert.NotNull(IoC.Resolve<ServerThread>("ServerThreadGetByID", "1"));
        Assert.NotNull(IoC.Resolve<ISender>("SenderAdapterGetByID", "1"));
    }
    [Fact]
    public void MyThreadEqualsTrueTest()
    {
        var sleep = new ManualResetEvent(false);
        BlockingCollection<ICommand> queue2 = new BlockingCollection<ICommand>(100);
        var sender2 = new SenderAdapter(queue2);
        var receiver2 = IoC.Resolve<IReceiver>("CreateReceiverAdapter", queue2, () => { sleep.Set(); });
        var thread2 = IoC.Resolve<ServerThread>("CreateThreadData", "2", sender2, receiver2);
        Assert.True(thread2.Equals(IoC.Resolve<ServerThread>("ServerThreadGetByID", "2")));
        sleep.WaitOne(200);

    }
}

