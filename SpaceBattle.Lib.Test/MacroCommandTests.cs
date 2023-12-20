namespace SpaceBattle.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech.Ioc;
using Hwdtech;

public class TestsMacroCommand
{
    Mock<IStrategy> strat1 = new Mock<IStrategy>();
    Mock<IStrategy> strat2 = new Mock<IStrategy>();
    Mock<IStrategy> strat3 = new Mock<IStrategy>();
    Mock<IStrategy> result = new Mock<IStrategy>();
    
    public TestsMacroCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Config.MacroCommand.Move", (object[] args) => strat1.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command1", (object[] args) => strat2.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command2", (object[] args) => strat3.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SimpleMacroCommand", (object[] args) => result.Object.Run(args)).Execute();
    }
    private static void SetupStrategy(Mock<IStrategy> mock1, List<string> list)
    {
        mock1.Setup(strategy => strategy.Run(It.IsAny<object[]>())).Returns(list).Verifiable();
    }
    private static void SetupStategyObject(Mock<IStrategy> mock1, Mock<SpaceBattle.Lib.ICommand> mock2)
    {
        mock1.Setup(strategy => strategy.Run(It.IsAny<object[]>())).Returns(mock2.Object).Verifiable();

    }
    private static void SetupStrategyResult(Mock<IStrategy> mock1, MacroCommand mock2)
    {
            mock1.Setup(strategy => strategy.Run(It.IsAny<object>())).Returns(mock2).Verifiable();
    }
    [Fact]
    public void PositiveMacroCommand()
    {
        var mock1 = new Mock<SpaceBattle.Lib.ICommand>();
        var mock2 = new Mock<SpaceBattle.Lib.ICommand>();
        var commands = new List<SpaceBattle.Lib.ICommand> { mock1.Object, mock2.Object };
        mock1.Setup(command => command.Execute()).Verifiable();
        mock2.Setup(command => command.Execute()).Verifiable();
        var mCommand = new MacroCommand(commands);
        mCommand.Execute();
    }
     [Fact]
    public void PositiveMacroCommandStrategy()
    {
        var UObject1 = new Mock<IUObject>();
        var mock1 = new Mock<SpaceBattle.Lib.ICommand>();
        var mock2 = new Mock<SpaceBattle.Lib.ICommand>();
        var StrategyMacroCommand = new MacroCommandStrategy();
        SetupStrategy(strat1, new List<string> { "Command1", "Command2" });
        SetupStategyObject(strat2, mock1);
        SetupStategyObject(strat3, mock2);
        var commands = new List<SpaceBattle.Lib.ICommand> {mock1.Object, mock2.Object };
        var macroCommand = new MacroCommand(commands);
        SetupStrategyResult(result, macroCommand);
        StrategyMacroCommand.Run(UObject1.Object, "Move");
        strat1.Verify();
        strat2.Verify();
        strat3.Verify();
        result.Verify();
        }
}
