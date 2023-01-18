using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test {
    public class CollisionCheckCommandTests
    {
        public CollisionCheckCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var regStrategy = new Mock<IStrategy>();
            regStrategy.Setup(s => s.Run(It.IsAny<object[]>())).Returns(new List<int>());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.GetList", (object[] args) => regStrategy.Object.Run(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.GetDiff", (object[] args) => new GetDiffStrategy().Run(args)).Execute();
        }

        [Fact]
        public void CollisionCheckNull()
        {
            var uObject1 = new Mock<IUObject>();
            var uObject2 = new Mock<IUObject>();

            var checkReturns = new Mock<IStrategy>();
            checkReturns.Setup(c => c.Run(It.IsAny<object[]>())).Throws((new NullReferenceException()));
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Collision.CheckWithTree", (object[] args) => checkReturns.Object.Run(args)).Execute();
            
            var collisionCheck = new CollisionCheck(uObject1.Object, uObject2.Object);
            Assert.ThrowsAny<Exception>(() => collisionCheck.Execute());
        }
    }
}