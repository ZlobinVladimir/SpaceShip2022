using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;
public class TestGameCommand
{
    public object globalScope;
    public Queue<ICommand> queue;
    Mock<IStrategy> strategy = new Mock<IStrategy>();
    public TestGameCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        globalScope = scope;
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTick", (object[] args) => (object)1000).Execute();
        strategy.Setup(x => x.StartStrategy()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object[] args) => strategy.Object).Execute();
    }

    [Fact]
    public void GameCommandTest()
    {
        ICommand emptyCommand = new EmptyCommand();
        var receiver = new QueueReceiver(queue);
        var gameCmd = new GameCommand(globalScope, receiver);
        for (int i = 0; i < 100; i++)
        {
            queue.Enqueue(emptyCommand);
        }
        var err = new Exception();
        Mock<ICommand> errorCommand = new Mock<ICommand>();
        errorCommand.Setup(x => x.Execute()).Throws<Exception>(() => err);

        queue.Enqueue(errorCommand.Object);
        gameCmd.Execute();

        strategy.Verify();
    }
}
