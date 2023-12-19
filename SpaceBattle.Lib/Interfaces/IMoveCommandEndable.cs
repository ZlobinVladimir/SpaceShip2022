namespace SpaceBattle.Lib;

public interface IMoveCommandEndable{
    ICommand Move {get; }
    IUObject Obj {get; }
    Queue<ICommand> ObjQueue {get; }
}