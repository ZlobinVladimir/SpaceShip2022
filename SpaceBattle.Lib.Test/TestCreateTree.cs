using SpaceBattle.Lib;
using Hwdtech;
using Moq;

namespace Tests.TestDecisionTree
{
    public class TestTreeCreation
    {

        Mock<IStrategy> DecisionStrategy = new Mock<IStrategy>();
        public TestTreeCreation()
        {
            new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create.Tree", (object[] args) => DecisionStrategy.Object.Run(args)).Execute();
        }


        [Fact]
        public void FileNotFoundException()
        {
            string path = "MyFile.txt";
            DecisionStrategy.Setup(c => c.Run(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();
            var mock = new CreateTree(path);
            Assert.Throws<FileNotFoundException>(() => mock.Execute());
            DecisionStrategy.Verify();
        }
        
        [Fact]
        public void Exception()
        {
            string path = "";
            DecisionStrategy.Setup(c => c.Run(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();
            var mock = new CreateTree(path);
            Assert.Throws<Exception>(() => mock.Execute());
            DecisionStrategy.Verify();
        }
    }
}