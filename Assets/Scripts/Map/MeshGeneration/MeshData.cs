using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshData
{
    private Dictionary<int, List<int[]>> trianglesDictionary;
    private HashSet<int> checkedEdgeVertexIndices;

    public Mesh Mesh
    {
        get
        {
            Mesh mesh = null;

            if (Vertices != null && Triangles != null)
            {
                mesh = new Mesh();
                mesh.vertices = Vertices.ToArray();
                mesh.triangles = Triangles.ToArray();
                mesh.RecalculateNormals();
            }

            return mesh;
        }
    }

    public HashSet<int> NonEdgeVertexIndices { get; set; }
    public List<Vector3> Vertices { get; private set; }
    public List<int> Triangles { get; private set; }

    public MeshData()
    {
        trianglesDictionary = new Dictionary<int, List<int[]>>();
        checkedEdgeVertexIndices = new HashSet<int>();

        NonEdgeVertexIndices = new HashSet<int>();
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
    }

    public void AddTriangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
    {
        AddTriangle(new int[] { vertexIndexA, vertexIndexB, vertexIndexC });
    }

    public void AddTriangle(int[] triangle)
    {
        Triangles.AddRange(triangle);

        foreach (int vertexIndex in triangle)
        {
            if (trianglesDictionary.ContainsKey(vertexIndex))
            {
                trianglesDictionary[vertexIndex].Add(triangle);
            }
            else
            {
                trianglesDictionary.Add(vertexIndex, new List<int[]> { triangle });
            }
        }
    }

    public IEnumerable<IEnumerable<int>> GetOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < Vertices.Count; vertexIndex++)
        {
            if (!NonEdgeVertexIndices.Contains(vertexIndex) && !checkedEdgeVertexIndices.Contains(vertexIndex))
            {
                checkedEdgeVertexIndices.Add(vertexIndex);

                int? nextOutlineVertexIndex = GetConnectedOutlineVertex(vertexIndex);
                if (nextOutlineVertexIndex != null)
                {
                    yield return GetOutline(vertexIndex);
                }
            }
        }
    }

    private IEnumerable<int> GetOutline(int vertexIndex)
    {
        yield return vertexIndex;

        int? nextOutlineVertexIndex = GetConnectedOutlineVertex(vertexIndex);
        while (nextOutlineVertexIndex != null && nextOutlineVertexIndex != vertexIndex)
        {
            checkedEdgeVertexIndices.Add((int)nextOutlineVertexIndex);
            yield return (int)nextOutlineVertexIndex;
            nextOutlineVertexIndex = GetConnectedOutlineVertex((int)nextOutlineVertexIndex);
        }

        yield return vertexIndex;
    }

    private int? GetConnectedOutlineVertex(int vertexIndexA)
    {
        int? vertexIndex = null;

        if (trianglesDictionary.ContainsKey(vertexIndexA))
        {
            foreach (int[] triangle in trianglesDictionary[vertexIndexA])
            {
                foreach (int vertexIndexB in triangle)
                {
                    bool sameVertexIndex = vertexIndexA == vertexIndexB;
                    bool alreadyChecked = checkedEdgeVertexIndices.Contains(vertexIndexB);
                    bool isOutlineEdge = IsOutlineEdge(vertexIndexA, vertexIndexB);

                    if (!sameVertexIndex && !alreadyChecked && isOutlineEdge)
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
        if (trianglesDictionary.ContainsKey(vertexIndexA) && vertexIndexA != vertexIndexB)
        {
            return trianglesDictionary[vertexIndexA].Count(t => t.Contains(vertexIndexB)) == 1;
        }

        return false;
    }
}