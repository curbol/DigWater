using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshGeneration
{
    public static MeshData GetMarchingSquaresMesh(this bool[,] map)
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

                ControlMeshVertex[,] controlMeshVertices = new ControlMeshVertex[sizeX, sizeY];
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        Vector2 position = new Vector2(-sizeX / 2f + x + 0.5f, -sizeY / 2f + y + 0.5f);
                        controlMeshVertices[x, y] = new ControlMeshVertex(position, map[x, y]);
                    }
                }

                for (int x = 0; x < sizeX - 1; x++)
                {
                    for (int y = 0; y < sizeY - 1; y++)
                    {
                        ControlMeshVertex topLeft = controlMeshVertices[x, y + 1];
                        ControlMeshVertex topRight = controlMeshVertices[x + 1, y + 1];
                        ControlMeshVertex bottomRight = controlMeshVertices[x + 1, y];
                        ControlMeshVertex bottomLeft = controlMeshVertices[x, y];
                        squares[x, y] = new MarchingSquare(topLeft, topRight, bottomRight, bottomLeft);
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

        if (square.TopLeft.VertexIndex != null)
            meshData.NonEdgeVertexIndices.Add((int)square.TopLeft.VertexIndex);

        if (square.TopRight.VertexIndex != null)
            meshData.NonEdgeVertexIndices.Add((int)square.TopRight.VertexIndex);

        if (square.BottomRight.VertexIndex != null)
            meshData.NonEdgeVertexIndices.Add((int)square.BottomRight.VertexIndex);

        if (square.BottomLeft.VertexIndex != null)
            meshData.NonEdgeVertexIndices.Add((int)square.BottomLeft.VertexIndex);
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
        for (int i = 2; i < points.Length; i++)
        {
            int vertexIndexA = (int)points[0].VertexIndex;
            int vertexIndexB = (int)points[i - 1].VertexIndex;
            int vertexIndexC = (int)points[i].VertexIndex;

            meshData.AddTriangle(vertexIndexA, vertexIndexB, vertexIndexC);
        }
    }
}