namespace SpaceBattle.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech.Ioc;
using Hwdtech;

public class TestsMacroCommand
{
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
}
