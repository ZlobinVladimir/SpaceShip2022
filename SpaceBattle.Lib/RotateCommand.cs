namespace SpaceBattle.Lib;

public class RotateCommand: ICommand
{
    public IRotatable SomeObj;
    public RotateCommand(IRotatable SomeObj)
    {
        this.SomeObj = SomeObj;
    }
    public void Execute()
    {
        SomeObj.angle += SomeObj.angularVelocity;
    }
}
