namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class Server
{
    public static void Main(string[] args)
    {
        int ThreadNumber = int.Parse(args[0]);

        Console.WriteLine("Starting the server...");

        IoC.Resolve<ICommand>("Thread.StartServer", ThreadNumber).Execute();

        Console.WriteLine("All threads are functioning");

        Console.WriteLine("Press any key to stop the server...");
        Console.Read();

        Console.WriteLine("Stopping the server...");

        IoC.Resolve<ICommand>("Thread.StopServer").Execute();

        Console.WriteLine("Press any key to exit...");
        Console.Read();
    }
}
