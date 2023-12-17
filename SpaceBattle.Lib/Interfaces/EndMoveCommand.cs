namespace SpaceBattle.Lib;
using Hwdtech;
public class EndMoveCommand : ICommand{
    IMoveCommandEndable MoveCommandEnd;
    public EndMoveCommand(IMoveCommandEndable obj){
        MoveCommandEnd = obj;
    }
    public void Execute(){
        ICommand StopCommand = IoC.Resolve<ICommand>("Clear.Command");
        IoC.Resolve<ICommand>("Delete.Property", MoveCommandEnd.Obj).Execute();
        IoC.Resolve<ICommand>("Insert.Command", MoveCommandEnd.ObjQueue, MoveCommandEnd.Move, StopCommand).Execute();
    }
}