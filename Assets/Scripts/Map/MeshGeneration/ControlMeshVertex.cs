using UnityEngine;

public class ControlMeshVertex : MeshVertex
{
    public bool IsActive { get; set; }
    public MeshVertex UpVertex { get; private set; }
    public MeshVertex RightVertex { get; private set; }

    public ControlMeshVertex(Vector2 position, bool isActive) : base(position)
    {
        IsActive = isActive;
        UpVertex = new MeshVertex(position + Vector2.up * 0.5F);
        RightVertex = new MeshVertex(position + Vector2.right * 0.5F);
    }
}