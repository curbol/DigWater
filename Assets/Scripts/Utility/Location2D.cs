using System;
using UnityEngine;

[Serializable]
public class Location2D
{
    [SerializeField]
    private Vector2 topRight;
    public Vector2 TopRight
    {
        get
        {
            if (topRight.x < bottomLeft.x)
                topRight.x = bottomLeft.x;

            if (topRight.y < bottomLeft.y)
                topRight.y = bottomLeft.y;

            return topRight;
        }
    }

    [SerializeField]
    private Vector2 bottomLeft;
    public Vector2 BottomLeft
    {
        get
        {
            return bottomLeft;
        }
    }

    public bool Contains(Vector2 position)
    {
        return position.x >= BottomLeft.x
            && position.y >= BottomLeft.y
            && position.x <= TopRight.x
            && position.y <= TopRight.y;
    }
}