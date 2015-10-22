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

                        if (x > 0 && y > 0)
                        {
                            ControlMeshVertex topLeft = controlMeshVertices[x - 1, y];
                            ControlMeshVertex topRight = controlMeshVertices[x, y];
                            ControlMeshVertex bottomRight = controlMeshVertices[x, y - 1];
                            ControlMeshVertex bottomLeft = controlMeshVertices[x - 1, y - 1];
                            squares[x - 1, y - 1] = new MarchingSquare(topLeft, topRight, bottomRight, bottomLeft);
                        }
                    }
                }
            }
        }

        return squares;
    }

    private static void AddMarchingSquare(this MeshData meshData, MarchingSquare square)
    {
        MeshVertex[] points = square.GetPoints();
        if (points != null)
        {
            meshData.AddPoints(square.GetPoints());

            foreach (MeshVertex meshVertex in points.OfType<ControlMeshVertex>().Where(p => p.VertexIndex != null))
            {
                meshData.NonEdgeVertexIndices.Add((int)meshVertex.VertexIndex);
            }
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
        for (int i = 2; i < points.Length; i++)
        {
            int vertexIndexA = (int)points[0].VertexIndex;
            int vertexIndexB = (int)points[i - 1].VertexIndex;
            int vertexIndexC = (int)points[i].VertexIndex;

            meshData.AddTriangle(vertexIndexA, vertexIndexB, vertexIndexC);
        }
    }
}