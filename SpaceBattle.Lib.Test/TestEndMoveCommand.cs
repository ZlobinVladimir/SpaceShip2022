namespace SpaceBattle.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech;
using Moq;

public class EndMoveCommandTests
{
    public EndMoveCommandTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());
        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mockCommand.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => mockStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => mockStrategy.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.SetProperty", (object[] args) => mockStrategy.Object.Run(args)).Execute();
    }

    [Fact]
    public void EndMoveCommandTest()
    {
        var move_endable = new Mock<IMoveCommandEndable>();
        move_endable.SetupGet(c => c.Move).Returns(new Mock<SpaceBattle.Lib.ICommand>().Object);
        move_endable.SetupGet(c => c.Obj).Returns(new Mock<IUObject>().Object);
        move_endable.SetupGet(c => c.ObjQueue).Returns(new Mock<Queue<SpaceBattle.Lib.ICommand>>().Object);

        var EndMove = new EndMoveCommand(move_endable.Object);
        EndMove.Execute();
        move_endable.VerifyAll();
    }
}
