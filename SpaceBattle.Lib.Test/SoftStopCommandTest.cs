using Hwdtech.Ioc;
using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;
public class SoftStopCommandTests
{
    public SoftStopCommandTests()
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
        var softStopStrategy = new SoftStopStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SoftStop", (object[] args) => softStopStrategy.StartStrategy(args)).Execute();
        var commandForSoftStopStrategy = new CommandForSoftStopStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CommandForSoftStopStrategy", (object[] args) => commandForSoftStopStrategy.StartStrategy(args)).Execute();
        var sendCommandStrategy = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommand", (object[] args) => sendCommandStrategy.StartStrategy(args)).Execute();
        
    }
    [Fact]
    public void SoftStopCommandSuccess()
    {
        var sleep = new ManualResetEvent(false);
        var thread = IoC.Resolve<ServerThread>("CreateThread", "1");
        Assert.True(thread.ThreadIsEmpty());
        var softStopCommand = IoC.Resolve<ICommand>("SoftStop", "1", () => { sleep.Set(); });
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "1");
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, softStopCommand);
        sendCommand.Execute();
        sleep.WaitOne(100);
        Assert.True(thread.ThreadIsEmpty());
        Assert.True(thread.GetStop());
    }
    [Fact]
    public void SoftStopWithOtherCommandsSuccess()
    {
        var mockCommand1 = new Mock<ICommand>();
        var mockCommand3 = new Mock<ICommand>();
        var mockCommand4 = new Mock<ICommand>();
        var mockCommand2 = new Mock<ICommand>();
        mockCommand1.Setup(_command => _command.Execute()).Verifiable();
        mockCommand3.Setup(_command => _command.Execute()).Verifiable();
        mockCommand4.Setup(_command => _command.Execute()).Verifiable();
        mockCommand2.Setup(_command => _command.Execute()).Verifiable();

        var sleep = new ManualResetEvent(false);
        var thread = IoC.Resolve<ServerThread>("CreateThread", "2");
        
        var softStopCommand = IoC.Resolve<ICommand>("SoftStop", "2");
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "2");
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand1.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand2.Object).Execute();
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, softStopCommand);
        sendCommand.Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand3.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand4.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, new ActionCommand(() => { sleep.Set(); })).Execute();
        
        sleep.WaitOne(100);
        mockCommand1.Verify();
        mockCommand3.Verify();
        mockCommand4.Verify();
        mockCommand2.Verify();
        
        
    }
    [Fact]
    public void MyThreadSoftStopTestWithException()
    {
        
        var command1 = new Mock<ICommand>();
        var regStrategy1 = new Mock<IStrategy>();
        command1.Setup(_command => _command.Execute()).Verifiable();
        regStrategy1.Setup(_strategy => _strategy.StartStrategy(It.IsAny<object[]>())).Returns(command1.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object[] args) => regStrategy1.Object.StartStrategy(args)).Execute();
        Action act1 = () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object[] args) => regStrategy1.Object.StartStrategy(args)).Execute();
        };

        var thread = IoC.Resolve<ServerThread>("CreateThread", "3", act1);
        var thread2 = IoC.Resolve<ServerThread>("CreateThread", "4", act1);
        var sleep = new ManualResetEvent(false);
        var softStopCommand = IoC.Resolve<ICommand>("SoftStop", "4", () => { sleep.Set(); });
        var sender = IoC.Resolve<ISender>("SenderAdapterGetByID", "3");
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, softStopCommand);

        sendCommand.Execute();
        sleep.WaitOne(100);
        Assert.True(thread.ThreadIsEmpty());
        Assert.False(!thread.ThreadIsEmpty());
        Assert.False(thread.GetStop());
        Assert.False(thread2.GetStop());
    }
}
