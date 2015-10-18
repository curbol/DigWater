public class SoilNode
{
    public int X { get; set; }
    public int Y { get; set; }
    public SoilType SoilType { get; set; }

    public SoilNode(int x, int y, SoilType soilType)
    {
        X = x;
        Y = y;
        SoilType = soilType;
    }
}