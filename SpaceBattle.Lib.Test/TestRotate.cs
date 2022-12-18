namespace SpaceBattle.Lib.Test;
public class TestRotate
{
    [Fact]
    public void Degree_135()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.Setup(a => a.angle).Returns(new Angle(45));
        rotateMock.Setup(a => a.angularVelocity).Returns(new Angle(90));
        var command = new RotateCommand(rotateMock.Object);
        command.Execute();
        rotateMock.VerifySet(a => a.angle = new Angle(135), Times.Once);   
    } 
    [Fact]
    public void No_Angle()
    {
        Mock<IRotatable> rotateMock = new Mock<IRotatable>();
        rotateMock.Setup(a => a.angle).Returns(new Angle(45));
        var command = new RotateCommand(rotateMock.Object);

    }
}
