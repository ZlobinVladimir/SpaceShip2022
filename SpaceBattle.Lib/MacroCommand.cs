namespace SpaceBattle.Lib;
public class MacroCommand: ICommand
{
    IEnumerable<ICommand> commands;
    public MacroCommand(IEnumerable<ICommand> commands)
    {
        this.commands = commands;
    }

     public void Execute()
    {
        var command = commands.GetEnumerator();
        while(command.MoveNext())
        {
            command.Current.Execute();
        }
    }
}
