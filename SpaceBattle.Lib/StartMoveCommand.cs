namespace SpaceBattle.Lib;
using Hwdtech;

public class StartMoveCommand : ICommand
{
    private IMoveCommandStartable moveCommandStart;
    public StartMoveCommand(IMoveCommandStartable moveCommandStart)
    {
        this.moveCommandStart = moveCommandStart;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Operations.SetProperty", moveCommandStart.Target, "Velocity", moveCommandStart.Properties["Velocity"]);
        var  moveCommand = IoC.Resolve<ICommand>("Commands.MoveCommand", moveCommandStart.Target);
        IoC.Resolve<ICommand>("Queue.Push", moveCommandStart.Queue, moveCommand).Execute();
        
    }
}
