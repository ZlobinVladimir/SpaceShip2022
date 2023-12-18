namespace SpaceBattle.Lib;
using Hwdtech;
using HI = Hwdtech.IoC;
public class EndMoveCommand : ICommand{
    IMoveCommandEndable MoveCommandEnd;
    public EndMoveCommand(IMoveCommandEndable obj){
        MoveCommandEnd = obj;
    }
    public void Execute(){
        ICommand EndCommand = HI.Resolve<ICommand>("Clear.Command");
        HI.Resolve<ICommand>("Delete.Property", MoveCommandEnd.Obj).Execute();
        HI.Resolve<ICommand>("Insert.Command", MoveCommandEnd.ObjQueue, MoveCommandEnd.Move, EndCommand).Execute();
    }
}
