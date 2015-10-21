using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMarchingSquaresMesh(this bool[,] map)
    {
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
        }

        return meshData;
    }

    public static IEnumerable<Vector2[]> GetEdges(this MeshData meshData)
    {
        foreach (List<int> outline in meshData.GetOutlines().Select(o => o.ToList()))
        {
            Vector2[] edgePoints = new Vector2[outline.Count];

            for (int i = 0; i < outline.Count; i++)
            {
                edgePoints[i] = meshData.Vertices[outline[i]];

                if (i < outline.Count - 1)
                {
                    Vector3 start = meshData.Vertices[outline[i]] + Vector3.back * 0.5f;
                    Vector3 end = meshData.Vertices[outline[i + 1]] + Vector3.back * 0.5f;
                    Debug.DrawLine(start, end, Color.green, 10);
                } 
            }

            yield return edgePoints;
        }
    }

    private static MarchingSquare[,] CreateMarchingSquareGrid(this bool[,] map)
    {
        MarchingSquare[,] squares = null;

        if (map != null)
        {
            int sizeX = map.GetLength(0);
            int sizeY = map.GetLength(1);

            if (sizeX > 0 && sizeY > 0)
            {
                squares = new MarchingSquare[sizeX - 1, sizeY - 1];

                for (int x = 0; x < sizeX - 1; x++)
                {
                    for (int y = 0; y < sizeY - 1; y++)
                    {
                        bool topLeftIsActive = map[x, y + 1];
                        bool topRightIsActive = map[x + 1, y + 1];
                        bool bottomRightIsActive = map[x + 1, y];
                        bool bottomLeftIsActive = map[x, y];
                        Vector2 squarePosition = new Vector2(-sizeX / 2f + x + 1, -sizeY / 2f + y + 1);

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
                meshData.checkedVertices.Add((int)square.TopLeft.VertexIndex);
                meshData.checkedVertices.Add((int)square.TopRight.VertexIndex);
                meshData.checkedVertices.Add((int)square.BottomRight.VertexIndex);
                meshData.checkedVertices.Add((int)square.BottomLeft.VertexIndex);
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

            meshData.AddTriangle(vertexIndexA, vertexIndexB, vertexIndexC);
        }
    }
}