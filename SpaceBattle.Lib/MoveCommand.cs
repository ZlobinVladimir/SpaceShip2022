namespace SpaceBattle.Lib;

public class MoveCommand : ICommand {
    public IMovable Obj;
    public MoveCommand(IMovable Obj){
        this.Obj = Obj;
    }
    public void Execute(){
        Obj.Position += Obj.Velocity;
    }
}