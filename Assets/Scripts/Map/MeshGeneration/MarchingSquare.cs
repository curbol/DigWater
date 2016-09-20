using UnityEngine;

public class MarchingSquare
{
    public ControlMeshVertex TopLeft { get; set; }
    public ControlMeshVertex TopRight { get; set; }
    public ControlMeshVertex BottomRight { get; set; }
    public ControlMeshVertex BottomLeft { get; set; }
    public MeshVertex CenterTop { get; set; }
    public MeshVertex CenterRight { get; set; }
    public MeshVertex CenterBottom { get; set; }
    public MeshVertex CenterLeft { get; set; }
    public MeshVertex CenterCenter { get; set; }
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
        CenterCenter = new MeshVertex((Vector2)bottomLeft.UpVertex.Position + (Vector2.right * MapManager.Map.Scale / 2));

        SetConfiguration(topLeft.IsActive, topRight.IsActive, bottomRight.IsActive, bottomLeft.IsActive);
    }

    public MeshVertex[] GetPoints()
    {
        switch (Configuration)
        {
            case 1:
                return new MeshVertex[] { CenterLeft, CenterBottom, BottomLeft };

            case 2:
                return new MeshVertex[] { BottomRight, CenterBottom, CenterRight };

            case 3:
                return new MeshVertex[] { CenterRight, BottomRight, BottomLeft, CenterLeft };

            case 4:
                return new MeshVertex[] { TopRight, CenterRight, CenterTop };

            case 5:
                return new MeshVertex[] { CenterTop, TopRight, CenterRight, CenterBottom, BottomLeft, CenterLeft };

            case 6:
                return new MeshVertex[] { CenterTop, TopRight, BottomRight, CenterBottom };

            case 7:
                return new MeshVertex[] { CenterTop, TopRight, BottomRight, BottomLeft, CenterLeft };

            case 8:
                return new MeshVertex[] { TopLeft, CenterTop, CenterLeft };

            case 9:
                return new MeshVertex[] { TopLeft, CenterTop, CenterBottom, BottomLeft };

            case 10:
                return new MeshVertex[] { TopLeft, CenterTop, CenterRight, BottomRight, CenterBottom, CenterLeft };

            case 11:
                return new MeshVertex[] { TopLeft, CenterTop, CenterRight, BottomRight, BottomLeft };

            case 12:
                return new MeshVertex[] { TopLeft, TopRight, CenterRight, CenterLeft };

            case 13:
                return new MeshVertex[] { TopLeft, TopRight, CenterRight, CenterBottom, BottomLeft };

            case 14:
                return new MeshVertex[] { TopLeft, TopRight, BottomRight, CenterBottom, CenterLeft };

            case 15:
                return new MeshVertex[] { TopLeft, TopRight, BottomRight, BottomLeft };

            // Additional cases to merge different marching squre meshes (prevent void spaces)
            // Top left sub-square
            case 16:
                return new MeshVertex[] { TopLeft, CenterTop, CenterCenter, CenterLeft };
            // Top right sub-square
            case 17:
                return new MeshVertex[] { CenterTop, TopRight, CenterRight, CenterCenter };
            // Bottom right sub-square
            case 18:
                return new MeshVertex[] { CenterCenter, CenterRight, BottomRight, CenterBottom };
            // Bottom left sub-square
            case 19:
                return new MeshVertex[] { CenterLeft, CenterCenter, CenterBottom, BottomLeft };
        }

        return null;
    }

    public void SetConfiguration(bool topLeftIsActive, bool topRightIsActive, bool bottomRightIsActive, bool bottomLeftIsActive)
    {
        Configuration += topLeftIsActive ? 8 : 0;
        Configuration += topRightIsActive ? 4 : 0;
        Configuration += bottomRightIsActive ? 2 : 0;
        Configuration += bottomLeftIsActive ? 1 : 0;
    }
}