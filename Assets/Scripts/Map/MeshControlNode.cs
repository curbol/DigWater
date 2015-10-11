using UnityEngine;

public class MeshControlNode : MeshNode
{
    public bool Active { get; set; }
    public MeshNode AboveNode { get; set; }
    public MeshNode RightNode { get; set; }

    public MeshControlNode(Vector3 position, bool active, float squareSize) : base(position)
    {
        Active = active;
        AboveNode = new MeshNode(Position + Vector3.up * squareSize / 2f);
        RightNode = new MeshNode(Position + Vector3.right * squareSize / 2f);
    }
}