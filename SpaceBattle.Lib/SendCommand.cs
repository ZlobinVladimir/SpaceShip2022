using System.ComponentModel;

namespace SpaceBattle.Lib;

public class SendCommand: ICommand
{
    private ISender sender;
    private ICommand command;
    public SendCommand(ISender sender, ICommand command)
    {
        this.sender = sender;
        this.command = command;
    }

    public void Execute()
    {
        sender.Push(command);
    }
}
