using UnityEngine;

public class MeshVertex
{
    public Vector3 Position { get; private set; }
    public int? VertexIndex { get; set; }

    public MeshVertex(Vector2 position)
    {
        Position = position;
    }
}