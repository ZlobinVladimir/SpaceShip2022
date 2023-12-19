namespace SpaceBattle.Lib;

public interface IStrategy{
    object Run(params object [] parameters);
}