using UnityEngine;

public class MarchingSquare
{
    public MeshVertex TopLeft { get; set; }
    public MeshVertex TopRight { get; set; }
    public MeshVertex BottomLeft { get; set; }
    public MeshVertex BottomRight { get; set; }

    public MeshVertex CenterTop { get; set; }
    public MeshVertex CenterLeft { get; set; }
    public MeshVertex CenterRight { get; set; }
    public MeshVertex CenterBottom { get; set; }

    public int Configuration { get; set; }

    public MarchingSquare(Vector2 squarePosition, float nodeSize, bool[] IsActive)
    {
        TopLeft = new MeshVertex(squarePosition + new Vector2(-nodeSize / 2f, nodeSize / 2f));
        TopRight = new MeshVertex(squarePosition + new Vector2(nodeSize / 2f, nodeSize / 2f));
        BottomRight = new MeshVertex(squarePosition + new Vector2(nodeSize / 2f, -nodeSize / 2f));
        BottomLeft = new MeshVertex(squarePosition + new Vector2(-nodeSize / 2f, -nodeSize / 2f));

        CenterTop = new MeshVertex(squarePosition + Vector2.up * nodeSize / 2f);
        CenterRight = new MeshVertex(squarePosition + Vector2.right * nodeSize / 2f);
        CenterBottom = new MeshVertex(squarePosition + Vector2.down * nodeSize / 2f);
        CenterLeft = new MeshVertex(squarePosition + Vector2.left * nodeSize / 2f);

        Configuration += IsActive[(int)SquareVertex.TopLeft] ? 8 : 0;
        Configuration += IsActive[(int)SquareVertex.TopRight] ? 4 : 0;
        Configuration += IsActive[(int)SquareVertex.BottomRight] ? 2 : 0;
        Configuration += IsActive[(int)SquareVertex.BottomLeft] ? 1 : 0;
    }
}