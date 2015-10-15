using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> Vertices { get; private set; }
    public List<int> Triangles { get; private set; }

    public MeshData()
    {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
    }
}