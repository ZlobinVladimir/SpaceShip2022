namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject Target { get; }

    IDictionary<string, object> Properties { get; }

    Queue<ICommand> Queue { get; }
}
