public struct Coordinate
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Coordinate)
        {
            return X == ((Coordinate)obj).X && Y == ((Coordinate)obj).Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() * X.GetHashCode() * Y.GetHashCode() - X.GetHashCode() + Y.GetHashCode();
    }

    public static bool operator ==(Coordinate c1, Coordinate c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(Coordinate c1, Coordinate c2)
    {
        return !c1.Equals(c2);
    }
}