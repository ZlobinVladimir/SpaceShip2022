using Xunit.Abstractions;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
namespace SpaceBattle.Lib.Test;
public class AdapterCodeGeneratorTests
{
    private readonly ITestOutputHelper output;
    public AdapterCodeGeneratorTests(ITestOutputHelper output)
    {
        this.output = output;
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateAdapterCode", (object[] args) => new AdapterBuilderStrategy().StartStrategy(args)).Execute(); 
    }

    [Fact]
    public void AdapterCodeGeneratorTest_1()
    {
        String ReciverAdapterTargetCode = 
        @"class ReceiverAdapter : IReceiver {
        BlockingCollection<ICommand> target;
        public ReceiverAdapter(BlockingCollection<ICommand> target) => this.target = target; 
        public ICommand Receive () {
            return IoC.Resolve<ICommand>(""Receive.Strategy"", target);
        }
        public Boolean isEmpty () {
            return IoC.Resolve<Boolean>(""isEmpty.Strategy"", target);
        }
    }";

        String SenderAdapterCode = 
        @"class SenderAdapter : ISender {
        BlockingCollection<ICommand> target;
        public SenderAdapter(BlockingCollection<ICommand> target) => this.target = target; 
        public void Push (ICommand command) {
            IoC.Resolve<ICommand>(""Push.Command"", target, command).Execute();
        }
    }";

        String RotatableAdapterCode = 
        @"class RotatableAdapter : IRotatable {
        Angle target;
        public RotatableAdapter(Angle target) => this.target = target; 
        public Angle angle {
               get { return IoC.Resolve<Angle>(""angle.Get"", target); }
               set { IoC.Resolve<ICommand>(""angle.Set"", target, value).Execute(); }
        }
        public Angle angularVelocity {
               get { return IoC.Resolve<Angle>(""angularVelocity.Get"", target); }
        }
    }";

        Assert.Equal(ReciverAdapterTargetCode, IoC.Resolve<String>("GenerateAdapterCode", typeof(IReceiver), typeof(BlockingCollection<ICommand>)));
        Assert.Equal(SenderAdapterCode, IoC.Resolve<String>("GenerateAdapterCode", typeof(ISender), typeof(BlockingCollection<ICommand>)));
        Assert.Equal(RotatableAdapterCode,IoC.Resolve<String>("GenerateAdapterCode", typeof(IRotatable), typeof(Angle)));
    }
}
