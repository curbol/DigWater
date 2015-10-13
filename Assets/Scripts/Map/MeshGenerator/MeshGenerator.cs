using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateMesh(int[,] map, float nodeSize, int isActiveKey = 1)
    {
        Mesh mesh = new Mesh();
        MeshInfo meshInfo = new MeshInfo();
        MarchingSquare[,] marchingSquares = CreateMarchingSquareGrid(map, nodeSize, isActiveKey);

        for (int x = 0; x < marchingSquares.GetLength(0); x++)
        {
            for (int y = 0; y < marchingSquares.GetLength(1); y++)
            {
                meshInfo.AddMarchingSquare(marchingSquares[x, y]);
            }
        }

        mesh.vertices = meshInfo.Vertices.ToArray();
        mesh.triangles = meshInfo.Triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    private static MarchingSquare[,] CreateMarchingSquareGrid(int[,] map, float nodeSize, int isActiveKey)
    {
        MarchingSquare[,] squares;

        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);

        float mapWidth = nodeCountX * nodeSize;
        float mapHeight = nodeCountY * nodeSize;

        squares = new MarchingSquare[nodeCountX - 1, nodeCountY - 1];
        for (int x = 0; x < nodeCountX - 1; x++)
        {
            for (int y = 0; y < nodeCountY - 1; y++)
            {
                bool[] isActive = new bool[4];
                isActive[(int)SquareVertex.TopLeft] = map[x, y + 1] == isActiveKey;
                isActive[(int)SquareVertex.TopRight] = map[x + 1, y + 1] == isActiveKey;
                isActive[(int)SquareVertex.BottomRight] = map[x + 1, y] == isActiveKey;
                isActive[(int)SquareVertex.BottomLeft] = map[x, y] == isActiveKey;

                Vector2 squarePosition = new Vector2(-mapWidth / 2f + x * nodeSize + nodeSize, -mapHeight / 2f + y * nodeSize + nodeSize);

                squares[x, y] = new MarchingSquare(squarePosition, nodeSize, isActive);
            }
        }

        return squares;
    }

    private static void AddMarchingSquare(this MeshInfo meshInfo, MarchingSquare square)
    {
        switch (square.Configuration)
        {
            // 0 points
            case 0:
                break;

            // 1 point
            case 1:
                meshInfo.AddPoints(square.CenterLeft, square.CenterBottom, square.BottomLeft);
                break;
            case 2:
                meshInfo.AddPoints(square.BottomRight, square.CenterBottom, square.CenterRight);
                break;
            case 4:
                meshInfo.AddPoints(square.TopRight, square.CenterRight, square.CenterTop);
                break;
            case 8:
                meshInfo.AddPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                break;

            // 2 points
            case 3:
                meshInfo.AddPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 5:
                meshInfo.AddPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                break;
            case 6:
                meshInfo.AddPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                break;
            case 9:
                meshInfo.AddPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                break;
            case 10:
                meshInfo.AddPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;
            case 12:
                meshInfo.AddPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                break;

            // 3 points
            case 7:
                meshInfo.AddPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 11:
                meshInfo.AddPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                break;
            case 13:
                meshInfo.AddPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                break;
            case 14:
                meshInfo.AddPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;

            // 4 points
            case 15:
                meshInfo.AddPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                break;
        }
    }

    private static void AddPoints(this MeshInfo meshInfo, params MeshVertex[] points)
    {
        meshInfo.AssignVertices(points);
        meshInfo.AddTriangles(points);
    }

    private static void AssignVertices(this MeshInfo meshInfo, params MeshVertex[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == null)
            {
                points[i].VertexIndex = meshInfo.Vertices.Count;
                meshInfo.Vertices.Add(points[i].Position);
            }
        }
    }

    private static void AddTriangles(this MeshInfo meshInfo, params MeshVertex[] points)
    {
        for (int i = 3; i <= points.Length; i++)
        {
            meshInfo.Triangles.Add((int)points[0].VertexIndex);
            meshInfo.Triangles.Add((int)points[i - 2].VertexIndex);
            meshInfo.Triangles.Add((int)points[i - 1].VertexIndex);
        }
    }
}