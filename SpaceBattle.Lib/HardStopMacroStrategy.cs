namespace SpaceBattle.Lib;
using System.Collections.Generic;


public class MacroCommandForHardStopStrategy: IStrategy
{
    public object StartStrategy(params object[] args)
    {
        List<ICommand> commands = new List<ICommand>();
        var actCommand = new ActionCommand((Action)args[1]);
        var threadStopCommand = (ICommand)args[0];
        commands.Add(threadStopCommand);
        commands.Add(actCommand);
        return new MacroCommands(commands);
    }
}
