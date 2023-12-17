using Hwdtech;
namespace SpaceBattle.Lib;
    public class MacroCommandStrategy: IStrategy
    {
        public object Run(params object[] args)
        {
            var obj1 = (IUObject)args[0];
            var name = (string)args[1];
            IEnumerable<string> NamesOfCommand = IoC.Resolve<IEnumerable<string>>("Config.MacroCommand." + name);
            IEnumerable<ICommand> commands = new List<ICommand>();
            var inameofcommand = NamesOfCommand.GetEnumerator();
            while (inameofcommand.MoveNext())
            {
                commands.Append(IoC.Resolve<ICommand>(inameofcommand.Current, obj1));
            }
            return IoC.Resolve<ICommand>("SimpleMacroCommand", commands);
        }
    }
