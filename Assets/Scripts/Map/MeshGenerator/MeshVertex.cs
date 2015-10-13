using UnityEngine;

public class MeshVertex
{
    public Vector3 Position { get; set; }
    public int? VertexIndex { get; set; }

    public MeshVertex(Vector3 position)
    {
        Position = position;
    }
}
