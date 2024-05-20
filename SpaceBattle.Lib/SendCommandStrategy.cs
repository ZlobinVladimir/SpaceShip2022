namespace SpaceBattle.Lib;

public class SendCommandStrategy: IStrategy
{
    public object StartStrategy(params object[] args)
    {
        var sendCommand = new SendCommand((ISender)args[0], (ICommand)args[1]);
        return sendCommand;
    }
}

