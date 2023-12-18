using System.Numerics;

namespace SpaceBattle.Lib.Test;

public class MoveTest{
    [Fact]
    public void MoveCheck(){
        Vector2 NewPosition = new(12, 5);
        Vector2 NewVelocity = new(-7, 3);
        Assert.Equal(NewPosition + NewVelocity, new Vector2(5, 8));
    }

    [Fact]
    public void CantReadPositionCheck(){
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.Position).Throws<Exception>();
        movable.SetupGet(m => m.Velocity).Returns(new Vector2(2, -1));
        var move_command = new MoveCommand(movable.Object);
        Assert.Throws<Exception>(() => move_command.Execute());
    }

    [Fact]
    public void CantReadVelocityCheck(){
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.Position).Returns(new Vector2(-5, 4));
        movable.SetupGet(m => m.Velocity).Throws<Exception>();
        var move_command = new MoveCommand(movable.Object);
        Assert.Throws<Exception>(() => move_command.Execute());
    }

    [Fact]
    public void CantChangePositionCheck(){
        var movable = new Mock<IMovable>();
        movable.SetupProperty(m => m.Position, new Vector2(4, 9));
        movable.SetupSet(m => m.Position = It.IsAny<Vector2>()).Throws<Exception>();
        movable.SetupGet(m => m.Velocity).Returns(new Vector2(7, 2));
        var move_command = new MoveCommand(movable.Object);
        Assert.Throws<Exception>(() => move_command.Execute());
    }
}