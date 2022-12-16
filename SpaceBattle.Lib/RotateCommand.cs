namespace SpaceBattle.Lib;

class RotateCommand: ICommand
{
    public IRotatable SomeObj;
    RotateCommand(IRotatable SomeObj)
    {
        this.SomeObj = SomeObj;
    }
    public void Execute()
    {
        
    }
}