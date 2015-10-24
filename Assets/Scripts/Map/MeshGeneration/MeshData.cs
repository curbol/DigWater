using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshData
{
    private List<Vector4> tangents;
    private List<Vector2> uv;
    private Dictionary<int, List<int[]>> trianglesDictionary;
    private HashSet<int> checkedEdgeVertexIndices;
    private float width;
    private float height;

    public List<Vector3> Vertices { get; set; }
    public List<int> Triangles { get; set; }
    public HashSet<int> NonEdgeVertexIndices { get; set; }

    public MeshData(float width, float height)
    {
        tangents = new List<Vector4>();
        uv = new List<Vector2>();
        trianglesDictionary = new Dictionary<int, List<int[]>>();
        checkedEdgeVertexIndices = new HashSet<int>();
        this.width = width;
        this.height = height;

        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        NonEdgeVertexIndices = new HashSet<int>();
    }

    public Vector3[] GetVertices()
    {
        return Vertices.ToArray();
    }

    public void AddVertex(Vector3 vertex)
    {
        Vertices.Add(vertex);
        tangents.Add(new Vector4(1f, 0f, 0f, -1f));

        float percentX = Mathf.InverseLerp(-width / 2f, width / 2f, vertex.x);
        float percentY = Mathf.InverseLerp(-height / 2f, height / 2f, vertex.y);
        uv.Add(new Vector2(percentX, percentY));
    }

    public int[] GetTriangles()
    {
        return Triangles.ToArray();
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

    public Vector4[] GetTangents()
    {
        return tangents.ToArray();
    }

    public Vector2[] GetUV()
    {
        return uv.ToArray();
    }

    public IEnumerable<IEnumerable<int>> GetOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < Vertices.Count; vertexIndex++)
        {
            if (!checkedEdgeVertexIndices.Contains(vertexIndex) && !NonEdgeVertexIndices.Contains(vertexIndex))
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