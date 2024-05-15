namespace SpaceBattle.Lib;

public interface ISender
{
    public void Push(ICommand command);
}
