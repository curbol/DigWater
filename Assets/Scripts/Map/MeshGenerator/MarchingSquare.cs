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

    public MarchingSquare(Vector2 squarePosition, float nodeSize, bool topLeftIsActive = false, bool topRightIsActive = false, bool bottomRightIsActive = false, bool bottomLeftIsActive = false)
    {
        TopLeft = new MeshVertex(squarePosition + new Vector2(-nodeSize / 2f, nodeSize / 2f));
        TopRight = new MeshVertex(squarePosition + new Vector2(nodeSize / 2f, nodeSize / 2f));
        BottomRight = new MeshVertex(squarePosition + new Vector2(nodeSize / 2f, -nodeSize / 2f));
        BottomLeft = new MeshVertex(squarePosition + new Vector2(-nodeSize / 2f, -nodeSize / 2f));

        CenterTop = new MeshVertex(squarePosition + Vector2.up * nodeSize / 2f);
        CenterRight = new MeshVertex(squarePosition + Vector2.right * nodeSize / 2f);
        CenterBottom = new MeshVertex(squarePosition + Vector2.down * nodeSize / 2f);
        CenterLeft = new MeshVertex(squarePosition + Vector2.left * nodeSize / 2f);

        SetConfiguration(topLeftIsActive, topRightIsActive, bottomRightIsActive, bottomLeftIsActive);
    }

    public void SetConfiguration(bool topLeftIsActive, bool topRightIsActive, bool bottomRightIsActive, bool bottomLeftIsActive)
    {
        Configuration += topLeftIsActive ? 8 : 0;
        Configuration += topRightIsActive ? 4 : 0;
        Configuration += bottomRightIsActive ? 2 : 0;
        Configuration += bottomLeftIsActive ? 1 : 0;
    }
}