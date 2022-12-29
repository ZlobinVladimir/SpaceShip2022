namespace SpaceBattle.Lib.Test;
public class TestRotate
{
    [Fact]
    public void SuccessRotate()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.Setup(a => a.angle).Returns(new Angle(45));
        rotateMock.Setup(a => a.angularVelocity).Returns(new Angle(90));
        var command = new RotateCommand(rotateMock.Object);
        command.Execute();
        rotateMock.VerifySet(a => a.angle = new Angle(135), Times.Once);   
    }
    [Fact]
    public void AngleGetException()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.SetupGet<Angle>(m => m.angularVelocity).Returns(new Angle (90));
        rotateMock.SetupSet<Angle>(m => m.angle = new Angle (135));
        rotateMock.SetupGet<Angle>(m => m.angle).Throws<Exception>();
        var command = new RotateCommand(rotateMock.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
    [Fact]
    public void AngularVelGetException()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.SetupProperty<Angle>(m => m.angle, new Angle (45));
        rotateMock.SetupGet<Angle>(m => m.angularVelocity).Throws<Exception>();
        var command = new RotateCommand(rotateMock.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
    [Fact]
    public void AngleSetException()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.SetupProperty(m => m.angle, new Angle(45));
        rotateMock.SetupGet<Angle>(m => m.angularVelocity).Returns(new Angle (90));
        rotateMock.SetupSet<Angle>(m => m.angle = It.IsAny<Angle>()).Throws<Exception>();
        var command= new RotateCommand(rotateMock.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
}
