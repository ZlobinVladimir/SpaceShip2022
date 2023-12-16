namespace SpaceBattle.Lib;

public interface IUObject{
    object GetProperty(string name);
    void SetProperty(string name, object value);
}