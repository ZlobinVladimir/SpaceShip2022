namespace SpaceBattle.Lib;

public class MoveCommand : ICommand{

    private readonly IMovable movable;

    public MoveCommand(IMovable movable) => this.movable = movable;
    
    public void Execute() => this.movable.Pos += this.movable.Velocity;
}
