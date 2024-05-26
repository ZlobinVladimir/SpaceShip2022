namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class TestInterpretCommand
{
    Dictionary<int, Queue<SpaceBattle.Lib.ICommand>> gameQueueMap = new Dictionary<int, Queue<SpaceBattle.Lib.ICommand>>();
    Dictionary<int, IUObject> gameUObjectMap = new Dictionary<int, IUObject>();

    public TestInterpretCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        Mock<IUObject> mockUObject = new Mock<IUObject>();
        mockUObject.Setup(x => x.setProperty(It.IsAny<string>(), It.IsAny<object>()));
        gameQueueMap.Add(1, new Queue<SpaceBattle.Lib.ICommand>());
        gameUObjectMap.Add(1, mockUObject.Object);

        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Map", (object[] args) => gameQueueMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Map", (object[] args) => gameUObjectMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => new GetGameQueueStrategy().Strategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.UObject", (object[] args) => new GetGameUObjectStrategy().Strategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Create.FromMessage", (object[] args) => new CreateGameCommandFromMessageStrategy().Strategy(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new GameQueuePushCommandStrategy().Strategy(args)).Execute();
    }
}
