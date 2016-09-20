using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshGeneration
{
    private enum Corner { TopLeft, TopRight, BottomRight, BottomLeft };

    public static MeshData GetMarchingSquaresMeshData(this bool[,] map, float scale)
    {
        MarchingSquare[,] marchingSquares = map.CreateMarchingSquareGrid(scale);
        return GetMeshData(marchingSquares);
    }

    public static MeshData GetMarchingSquaresMeshData(this bool[,] subMap, int[,] intMap, float scale)
    {
        MarchingSquare[,] marchingSquares = subMap.CreateMarchingSquareGrid(intMap, scale);
        return GetMeshData(marchingSquares);
    }

    public static Mesh BuildMesh(this MeshData meshData)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.GetVertices();
        mesh.triangles = meshData.GetTriangles();
        mesh.tangents = meshData.GetTangents();
        mesh.uv = meshData.GetUV();
        mesh.RecalculateNormals();

        return mesh;
    }

    public static IEnumerable<Vector2[]> BuildMeshEdges(this MeshData meshData)
    {
        foreach (IEnumerable<int> outline in meshData.GetOutlines())
        {
            yield return outline.Select(a => (Vector2)meshData.Vertices[a]).ToArray();
        }
    }

    private static void AddMarchingSquare(this MeshData meshData, MarchingSquare square)
    {
        MeshVertex[] points = square.GetPoints();
        if (points != null)
        {
            meshData.AddPoints(points);

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

    private static void AssignVertices(this MeshData meshData, params MeshVertex[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == null)
            {
                points[i].VertexIndex = meshData.Vertices.Count;
                meshData.AddVertex(points[i].Position);
            }
        }
    }

    private static MarchingSquare[,] CreateMarchingSquareGrid(this bool[,] map, float scale)
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
                        Vector2 position = new Vector2(-sizeX / 2F + x + 0.5F, -sizeY / 2F + y + 0.5F) * scale;
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

    private static MarchingSquare[,] CreateMarchingSquareGrid(this bool[,] subMap, int[,] fullMap, float scale)
    {
        MarchingSquare[,] squares = null;

        if (subMap != null)
        {
            int sizeX = subMap.GetLength(0);
            int sizeY = subMap.GetLength(1);

            if (sizeX > 0 && sizeY > 0)
            {
                squares = new MarchingSquare[sizeX - 1, sizeY - 1];

                ControlMeshVertex[,] controlMeshVertices = new ControlMeshVertex[sizeX, sizeY];
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        // Create new control mesh vertex for coordinate (x,y)
                        Vector2 position = new Vector2(-sizeX / 2F + x + 0.5F, -sizeY / 2F + y + 0.5F) * scale;
                        controlMeshVertices[x, y] = new ControlMeshVertex(position, subMap[x, y]);

                        // Create new marching square for coordinate (x-1, y-1)
                        if (x > 0 && y > 0)
                        {
                            ControlMeshVertex topLeft = controlMeshVertices[x - 1, y];
                            ControlMeshVertex topRight = controlMeshVertices[x, y];
                            ControlMeshVertex bottomRight = controlMeshVertices[x, y - 1];
                            ControlMeshVertex bottomLeft = controlMeshVertices[x - 1, y - 1];

                            squares[x - 1, y - 1] = new MarchingSquare(topLeft, topRight, bottomRight, bottomLeft);

                            // Check for special cases of different meshes touching (to prevent void spaces)
                            bool[] activeVertices = new[] { topLeft.IsActive, topRight.IsActive, bottomRight.IsActive, bottomLeft.IsActive };
                            int[] fullMapTypeIds = new[] { fullMap[x - 1, y], fullMap[x, y], fullMap[x, y - 1], fullMap[x - 1, y - 1] };
                            bool[] fullMapTangentInequality = new[]
                            {
                                fullMapTypeIds[(int)Corner.TopRight] != fullMapTypeIds[(int)Corner.BottomLeft],
                                fullMapTypeIds[(int)Corner.TopLeft] != fullMapTypeIds[(int)Corner.BottomRight],
                                fullMapTypeIds[(int)Corner.TopRight] != fullMapTypeIds[(int)Corner.BottomLeft],
                                fullMapTypeIds[(int)Corner.TopLeft] != fullMapTypeIds[(int)Corner.BottomRight],
                            };

                            if (activeVertices.Count(a => a) == 1)
                            {
                                int activeIndex = Array.IndexOf(activeVertices, true);
                                if (fullMapTangentInequality[activeIndex])
                                {
                                    squares[x - 1, y - 1].Configuration = 16 + activeIndex;
                                }
                            }
                        }
                    }
                }
            }
        }

        return squares;
    }

    private static MeshData GetMeshData(MarchingSquare[,] marchingSquares)
    {
        MeshData meshData = new MeshData();

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
}