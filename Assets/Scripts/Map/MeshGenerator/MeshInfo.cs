using System.Collections.Generic;
using UnityEngine;

public class MeshInfo
{
    public List<Vector3> Vertices { get; set; }
    public List<int> Triangles { get; set; }

    public MeshInfo()
    {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
    }
}