namespace SpaceBattle.Lib;


public interface IUObject
{
    object propGet(string name);
    void propSet(string name, object value );

}
