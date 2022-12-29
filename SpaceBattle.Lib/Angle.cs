namespace SpaceBattle.Lib;

public class Angle
{
    public double degree;
    public Angle(double degree)
    {
        if(degree == 0) throw new ArgumentException();
        this.degree = degree % 360;
    }
    
    public static Angle operator + (Angle first, Angle second)
    {
        double tempDegree = (first.degree + second.degree) % 360;
        return new Angle(tempDegree);
    }
    public static bool operator ==(Angle first, Angle second) => first.degree == second.degree;

    public static bool operator !=(Angle first, Angle second) => first.degree != second.degree;

    public override bool Equals(object? obj) => obj is Angle a && degree == a.degree;

    public override int GetHashCode() => this.degree.ToString().GetHashCode();
}
