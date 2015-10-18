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

    public MarchingSquare(Vector2 squarePosition, bool topLeftIsActive = false, bool topRightIsActive = false, bool bottomRightIsActive = false, bool bottomLeftIsActive = false)
    {
        TopLeft = new MeshVertex(squarePosition + new Vector2(-0.5f, 0.5f));
        TopRight = new MeshVertex(squarePosition + new Vector2(0.5f, 0.5f));
        BottomRight = new MeshVertex(squarePosition + new Vector2(0.5f, -0.5f));
        BottomLeft = new MeshVertex(squarePosition + new Vector2(-0.5f, -0.5f));

        CenterTop = new MeshVertex(squarePosition + Vector2.up * 0.5f);
        CenterRight = new MeshVertex(squarePosition + Vector2.right * 0.5f);
        CenterBottom = new MeshVertex(squarePosition + Vector2.down * 0.5f);
        CenterLeft = new MeshVertex(squarePosition + Vector2.left * 0.5f);

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