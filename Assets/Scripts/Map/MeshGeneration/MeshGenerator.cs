using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateMarchingSquaresMesh(this bool[,] map)
    {
        Mesh mesh = new Mesh();
        MeshData meshData = new MeshData();
        MarchingSquare[,] marchingSquares = map.CreateMarchingSquareGrid();

        if (marchingSquares != null)
        {
            for (int x = 0; x < marchingSquares.GetLength(0); x++)
            {
                for (int y = 0; y < marchingSquares.GetLength(1); y++)
                {
                    meshData.AddMarchingSquare(marchingSquares[x, y]);
                }
            }

            mesh.vertices = meshData.Vertices.ToArray();
            mesh.triangles = meshData.Triangles.ToArray();
            mesh.RecalculateNormals();
        }

        return mesh;
    }

    private static MarchingSquare[,] CreateMarchingSquareGrid(this bool[,] map)
    {
        MarchingSquare[,] squares = null;

        if (map != null)
        {
            int mapWidth = map.GetLength(0);
            int mapHeight = map.GetLength(1);

            if (mapWidth > 0 && mapHeight > 0)
            {
                squares = new MarchingSquare[mapWidth - 1, mapHeight - 1];

                for (int x = 0; x < mapWidth - 1; x++)
                {
                    for (int y = 0; y < mapHeight - 1; y++)
                    {
                        bool topLeftIsActive = map[x, y + 1];
                        bool topRightIsActive = map[x + 1, y + 1];
                        bool bottomRightIsActive = map[x + 1, y];
                        bool bottomLeftIsActive = map[x, y];
                        Vector2 squarePosition = new Vector2(-mapWidth / 2f + x + 1, -mapHeight / 2f + y + 1);

                        squares[x, y] = new MarchingSquare(squarePosition, topLeftIsActive, topRightIsActive, bottomRightIsActive, bottomLeftIsActive);
                    }
                }
            }
        }

        return squares;
    }

    private static void AddMarchingSquare(this MeshData meshData, MarchingSquare square)
    {
        switch (square.Configuration)
        {
            case 0:
                break;
            case 1:
                meshData.AddPoints(square.CenterLeft, square.CenterBottom, square.BottomLeft);
                break;
            case 2:
                meshData.AddPoints(square.BottomRight, square.CenterBottom, square.CenterRight);
                break;
            case 3:
                meshData.AddPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 4:
                meshData.AddPoints(square.TopRight, square.CenterRight, square.CenterTop);
                break;
            case 5:
                meshData.AddPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                break;
            case 6:
                meshData.AddPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                break;
            case 7:
                meshData.AddPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 8:
                meshData.AddPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                break;
            case 9:
                meshData.AddPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                break;
            case 10:
                meshData.AddPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;
            case 11:
                meshData.AddPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                break;
            case 12:
                meshData.AddPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                break;
            case 13:
                meshData.AddPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                break;
            case 14:
                meshData.AddPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;
            case 15:
                meshData.AddPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                break;
        }
    }

    private static void AddPoints(this MeshData meshData, params MeshVertex[] points)
    {
        meshData.AssignVertices(points);
        meshData.AddTriangles(points);
    }

    private static void AssignVertices(this MeshData meshData, params MeshVertex[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == null)
            {
                points[i].VertexIndex = meshData.Vertices.Count;
                meshData.Vertices.Add(points[i].Position);
            }
        }
    }

    private static void AddTriangles(this MeshData meshData, params MeshVertex[] points)
    {
        for (int i = 3; i <= points.Length; i++)
        {
            int vertexIndexA = (int)points[0].VertexIndex;
            int vertexIndexB = (int)points[i - 2].VertexIndex;
            int vertexIndexC = (int)points[i - 1].VertexIndex;

            meshData.Triangles.AddRange(new int[] { vertexIndexA, vertexIndexB, vertexIndexC });
        }
    }
}