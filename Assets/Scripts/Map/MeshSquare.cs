public class MeshSquare
{
    public MeshControlNode TopLeft { get; set; }
    public MeshControlNode TopRight { get; set; }
    public MeshControlNode BottomLeft { get; set; }
    public MeshControlNode BottomRight { get; set; }

    public MeshNode CenterTop { get; set; }
    public MeshNode CenterLeft { get; set; }
    public MeshNode CenterRight { get; set; }
    public MeshNode CenterBottom { get; set; }

    public int Configuration { get; set; }

    public MeshSquare(MeshControlNode topLeft, MeshControlNode topRight, MeshControlNode bottomRight, MeshControlNode bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;

        CenterTop = TopLeft.RightNode;
        CenterRight = BottomRight.AboveNode;
        CenterBottom = BottomLeft.RightNode;
        CenterLeft = BottomLeft.AboveNode;

        Configuration += topLeft.Active ? 8 : 0;
        Configuration += topRight.Active ? 4 : 0;
        Configuration += BottomRight.Active ? 2 : 0;
        Configuration += BottomLeft.Active ? 1 : 0;
    }
}