namespace SpaceBattle.Lib.Test;

public class AngleTest
{
    [Fact]
    public void Degree_Not_Zero()
    {
        Assert.Throws<ArgumentException>(() => new Angle(0));
    }

    [Fact]
    public void Degree_Not_More360()
    {
        Angle newAngle = new(380);
        Assert.Equal(newAngle.degree, 20);
    }

    [Fact]
    public void Angle_Addition_Success()
    {
        Angle newAngle = new(120);
        Angle newAngle2 = new(280);
        Assert.Equal(newAngle + newAngle2, new Angle(40));
    }
    [Fact]
    public void Angle_Equal_Success()
    {
        Angle newAngle = new(60);
        Angle newAngle2 = new(420);
        Assert.True(newAngle == newAngle2);
    }
    [Fact]
    public void Angle_Equal_Fail()
    {
        Angle newAngle = new(60);
        Angle newAngle2 = new(70);
        Assert.False(newAngle == newAngle2);
    }
    [Fact]
    public void Angle_Unequal_Success()
    {
      Angle newAngle = new(30);
      Angle newAngle2 = new(45);
      Assert.True(newAngle != newAngle2);
    }
    [Fact]
    public void Angle_Unequal_Fail()
    {
        Angle newAngle = new(30);
        Angle newAngle2 = new(390);
        Assert.False(newAngle != newAngle2);
    }
    [Fact]
    public void GetHashCode_Sucess()
    {
        Angle newAngle = new(12);
        Angle newAngle2 = new(372);
        Assert.True(newAngle.GetHashCode() == newAngle2.GetHashCode());
    }
}
