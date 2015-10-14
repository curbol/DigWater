using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshData
{
    private Dictionary<int, List<int[]>> trianglesDictionary;
    private List<List<int>> outlines;
    private HashSet<int> checkedVertices;

    public List<Vector3> Vertices { get; private set; }
    public List<int> Triangles { get; private set; }

    public MeshData()
    {
        trianglesDictionary = new Dictionary<int, List<int[]>>();
        outlines = new List<List<int>>();
        checkedVertices = new HashSet<int>();

        Vertices = new List<Vector3>();
        Triangles = new List<int>();
    }

    public void AddTriangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
    {
        int[] vertexIndices = { vertexIndexA, vertexIndexB, vertexIndexC };
        foreach (int vertexIndex in vertexIndices)
        {
            Triangles.Add(vertexIndex);

            if (trianglesDictionary.ContainsKey(vertexIndex))
            {
                trianglesDictionary[vertexIndex].Add(vertexIndices);
            }
            else
            {
                trianglesDictionary.Add(vertexIndex, new List<int[]> { vertexIndices });
            }
        }
    }

    private void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < Vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int? newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != null)
                {
                    checkedVertices.Add(vertexIndex);
                    outlines.Add(new List<int> { vertexIndex });

                    // TODO: Why can't this start with vertexIndex?
                    FollowOutline((int)newOutlineVertex, outlines.Count - 1);

                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    private void FollowOutline(int vertexIndex, int outlineIndex)
    {

    }

    private int? GetConnectedOutlineVertex(int vertexIndexA)
    {
        int? vertexIndex = null;

        if (trianglesDictionary.ContainsKey(vertexIndexA))
        {
            List<int[]> trianglesContainingVertex = trianglesDictionary[vertexIndexA];
            foreach (int[] triangleContainingVertex in trianglesContainingVertex)
            {
                foreach (int vertexIndexB in triangleContainingVertex)
                {
                    if (vertexIndexA != vertexIndexB && IsOutlineEdge(vertexIndexA, vertexIndexB))
                    {
                        vertexIndex = vertexIndexB;
                    }
                }
            }
        }

        return vertexIndex;
    }

    private bool IsOutlineEdge(int vertexIndexA, int vertexIndexB)
    {
        int sharedTriangleCount = 0;

        if (trianglesDictionary.ContainsKey(vertexIndexA) && trianglesDictionary.ContainsKey(vertexIndexB))
        {
            List<int[]> trianglesContainingVertexA = trianglesDictionary[vertexIndexA];
            foreach (int[] triangleContainingVertexA in trianglesContainingVertexA)
            {
                sharedTriangleCount += triangleContainingVertexA.Contains(vertexIndexB) ? 1 : 0;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }

        return sharedTriangleCount == 1;
    }
}