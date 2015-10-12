using UnityEngine;

public class MeshControlNode : MeshNode
{
    public bool Active { get; set; }

    public MeshControlNode(Vector3 position, bool active, float squareSize) : base(position)
    {
        Active = active;
    }
}