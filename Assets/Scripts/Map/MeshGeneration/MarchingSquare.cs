using UnityEngine;

public class MarchingSquare
{
    public ControlMeshVertex TopLeft { get; set; }
    public ControlMeshVertex TopRight { get; set; }
    public ControlMeshVertex BottomRight { get; set; }
    public ControlMeshVertex BottomLeft { get; set; }

    public MeshVertex CenterTop { get; set; }
    public MeshVertex CenterLeft { get; set; }
    public MeshVertex CenterRight { get; set; }
    public MeshVertex CenterBottom { get; set; }

    public int Configuration { get; set; }

    public MarchingSquare(ControlMeshVertex topLeft, ControlMeshVertex topRight, ControlMeshVertex bottomRight, ControlMeshVertex bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;

        CenterTop = topLeft.RightVertex;
        CenterRight = bottomRight.UpVertex;
        CenterBottom = bottomLeft.RightVertex;
        CenterLeft = bottomLeft.UpVertex;

        SetConfiguration(topLeft.IsActive, topRight.IsActive, bottomRight.IsActive, bottomLeft.IsActive);
    }

    public void SetConfiguration(bool topLeftIsActive, bool topRightIsActive, bool bottomRightIsActive, bool bottomLeftIsActive)
    {
        Configuration += topLeftIsActive ? 8 : 0;
        Configuration += topRightIsActive ? 4 : 0;
        Configuration += bottomRightIsActive ? 2 : 0;
        Configuration += bottomLeftIsActive ? 1 : 0;
    }
}