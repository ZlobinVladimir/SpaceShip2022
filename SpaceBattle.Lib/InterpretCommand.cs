namespace SpaceBattle.Lib;
using Hwdtech;

public class InterpretCommand : ICommand 
{
    IInterpretMessage _message;

    public InterpretCommand(IInterpretMessage message) => _message = message;

    public void Execute()
    {
        var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Command.Create.FromMessage", _message);

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Queue.Push", _message.GameID, cmd).Execute();
    }
}