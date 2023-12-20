using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

public class TestContinuousOperation{
    Mock<IStrategy> Strategy1 = new Mock<IStrategy>();
    Mock<IStrategy> Strategy2 = new Mock<IStrategy>();
    Mock<IStrategy> Strategy3 = new Mock<IStrategy>();
    public TestContinuousOperation(){ 
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue", (object[] args) => Strategy1.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create.Command", (object[] args) => Strategy2.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Repeat", (object[] args) => Strategy3.Object.Run(args)).Execute();
    }
    private static void CreateStrategy(Mock<IStrategy> mock1, Mock<SpaceBattle.Lib.ICommand> mock2){
        mock1.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mock2.Object).Verifiable();
    }

    private static void QueueStrategy(Mock<IStrategy> mock1, Mock<Queue<SpaceBattle.Lib.ICommand>> mock2){
        mock1.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mock2.Object).Verifiable();
    }
    [Fact]
    public void CreateContinuousOperationStrategyTest(){
        var mock1 = new Mock<SpaceBattle.Lib.ICommand>();
        mock1.Setup(x => x.Execute());
        var mock2 = new Mock<Queue<SpaceBattle.Lib.ICommand>>();
        QueueStrategy(Strategy1, mock2);
        CreateStrategy(Strategy2, mock1);
        CreateStrategy(Strategy3, mock1);

        var Contop = new ContinuousOperation();
        var UObj = new Mock<IUObject>();
        Contop.Run(It.IsAny<string>(), UObj.Object);

        Strategy1.Verify();
        Strategy2.Verify();
        Strategy3.Verify();

    }
}
