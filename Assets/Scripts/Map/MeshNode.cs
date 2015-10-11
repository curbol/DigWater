using UnityEngine;

public class MeshNode
{
    public Vector3 Position { get; set; }
    public int? VertexIndex { get; set; }

    public MeshNode(Vector3 position)
    {
        Position = position;
    }
}
