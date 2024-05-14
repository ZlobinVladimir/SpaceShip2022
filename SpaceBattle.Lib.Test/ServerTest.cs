namespace SpaceBattle.Lib.Test;

using Xunit;
using Moq;
using System;
using System.IO;
using System.Collections.Generic;
using Hwdtech;

public class Test_ServerStart
{
    public object globalScope;        
    int threadsStartCount = 0;
    int threadsStopCount = 0;
    bool starting = false;
    bool stopping = false;
    public Test_ServerStart() 
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Thread.CreateAndStartThread", (object[] args) => {
            return new ActionCommand( () => {threadsStartCount++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GetDictionary", (object[] args) => {
            Dictionary<string, string> threads = new Dictionary<string, string>() { 
                {"123", "T111"}, 
                {"456", "T222"},
                {"789", "T333"}
            };
            return threads;
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.HardStop", (object[] args) => {
            return new ActionCommand( () => {threadsStopCount++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.StartServer", (object[] args) => {
            starting = true;
            return new EmptyCommand();
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Thread.StopServer", (object[] args) => {
            stopping = true;
            return new EmptyCommand();
        }).Execute();

        globalScope = scope;
    }

    [Fact]
    public void ConsoleTest()
    {
        int numOfThread = 3;
        var args = new[] { "3" }; 
        var consoleInput = new StringReader("Hello, World!");
        var consoleOutput = new StringWriter();
        var originalInput = Console.In;
        var originalOutput = Console.Out;
        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        Server.Main(args);

        var output = consoleOutput.ToString();
        Console.SetIn(originalInput);
        Console.SetOut(originalOutput);

        Assert.Contains("Starting the server...", output);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Thread.StartServer", numOfThread).Execute();
        Assert.True(true == starting);
        Assert.Contains("All threads are functioning", output);
        Assert.Contains("Stopping the server...", output);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Thread.StopServer").Execute();
        Assert.True(true == stopping);
        Assert.Contains("Press any key to exit...", output);
    }

    [Fact]
    public void StartThreadTest()
    {

        int numOfThread = 5;
        var startServerCommand = new StartServerCommand(numOfThread);
        startServerCommand.Execute();

        Assert.Equal(numOfThread, threadsStartCount);
    }
}