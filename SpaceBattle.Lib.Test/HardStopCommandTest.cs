using Hwdtech.Ioc;
using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class HardStopCommandTests
{
    public HardStopCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var threadDict = new ConcurrentDictionary<string, ServerThread>();
        var senderDict = new ConcurrentDictionary<string, ISender>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadIDMyThreadMapping", (object[] _) => threadDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadIDSenderMapping", (object[] _) => senderDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderAdapterGetByID", (object[] id) => senderDict[(string)id[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadGetByID", (object[] id) => threadDict[(string)id[0]]).Execute();

        var createThreadStrategy = new CreateThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateThread", (object[] args) => createThreadStrategy.StartStrategy(args)).Execute();
        var createThreadDataStrategy = new CreateThreadDataStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateThreadData", (object[] args) => createThreadDataStrategy.StartStrategy(args)).Execute();
        var createReceiverAdapterStrategy = new CreateReceiverAdapterStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateReceiverAdapter", (object[] args) => createReceiverAdapterStrategy.StartStrategy(args)).Execute();
        var hardStopStrategy = new HardStopStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HardStop", (object[] args) => hardStopStrategy.StartStrategy(args)).Execute();
        var sendCommandStrategy = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommand", (object[] args) => sendCommandStrategy.StartStrategy(args)).Execute();
        var macroCommandForHardStopStrategy = new MacroCommandForHardStopStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MacroCommandForHardStopStrategy", (object[] args) => macroCommandForHardStopStrategy.StartStrategy(args)).Execute();
    }
    [Fact]
    public void HardStopCommandSuccess()
    {
        var thread = IoC.Resolve<ServerThread>("CreateThread", "11");
        var sleep = new ManualResetEvent(false);
        Assert.True(thread.ThreadIsEmpty());
        var hardStopCommand = IoC.Resolve<ICommand>("HardStop", "11", () => { sleep.Set(); });
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "11");
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, hardStopCommand);
        sendCommand.Execute();
        sleep.WaitOne(100);
        Assert.True(thread.ThreadIsEmpty());
        Assert.True(thread.GetStop());
    }
    [Fact]
    public void HardStopCommandWithoutAction()
    {
        var thread = IoC.Resolve<ServerThread>("CreateThread", "22");
        var hardStopCommand = IoC.Resolve<ICommand>("HardStop", "22");
        var sleep = new ManualResetEvent(false);
        Assert.NotNull(hardStopCommand);
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "22");
        IoC.Resolve<ICommand>("SendCommand", sender, new ActionCommand(() => { sleep.Set(); })).Execute();
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, hardStopCommand);
        sendCommand.Execute();
        sleep.WaitOne(100);
    }
    [Fact]
    public void MyThreadHardStopTestWithException()
    {
        var command = new Mock<ICommand>();
        var regStrategy = new Mock<IStrategy>();
        command.Setup(_command => _command.Execute()).Verifiable();
        regStrategy.Setup(_strategy => _strategy.StartStrategy(It.IsAny<object[]>())).Returns(command.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object[] args) => regStrategy.Object.StartStrategy(args)).Execute();
        Action act = () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object[] args) => regStrategy.Object.StartStrategy(args)).Execute();
        };

        var thread3 = IoC.Resolve<ServerThread>("CreateThread", "33", act);
        var thread4 = IoC.Resolve<ServerThread>("CreateThread", "44", act);
        var sleep = new ManualResetEvent(false);
        var hardStopCommand = IoC.Resolve<ICommand>("HardStop", "44", () => { sleep.Set(); });
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "33");
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, hardStopCommand);

        sendCommand.Execute();
        sleep.WaitOne(100);
        Assert.True(thread3.ThreadIsEmpty());
        Assert.False(thread3.GetStop());
        Assert.False(thread4.GetStop());
    }
}
