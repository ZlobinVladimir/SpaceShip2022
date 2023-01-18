namespace SpaceBattle.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech.Ioc;
using Hwdtech;


public class StartMoveCommandTests
{
    public StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());
        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mockCommand.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => mockStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => mockStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.SetProperty", (object[] args) => mockStrategy.Object.Run(args)).Execute();
    }

    [Fact]

    public void StartMoveCommand_Successful()
    {
       
        var move_startable = new Mock<IMoveCommandStartable>();
        move_startable.SetupGet(c => c.Target).Returns(new Mock<IUObject>().Object).Verifiable();
        move_startable.SetupGet(c => c.Properties).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();
        
        var startMove = new StartMoveCommand(move_startable.Object);
        startMove.Execute();
        move_startable.Verify();
    }
}