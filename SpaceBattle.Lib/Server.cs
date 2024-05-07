namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class Server
{
    public static void Main(string[] args)
    {
        int ThreadNumber = int.Parse(args[0]);

        Console.WriteLine("Launching server...");

        IoC.Resolve<ICommand>("Thread.ConsoleStartServer", ThreadNumber).Execute();

        Console.WriteLine("All threads are functioning");

        Console.WriteLine("Press any key to stop the server...");
        Console.Read();

        Console.WriteLine("Stopping server...");

        IoC.Resolve<ICommand>("Thread.ConsoleStopServer").Execute();

        Console.WriteLine("Exiting. Press any key to exit...");
        Console.Read();
    }
}
