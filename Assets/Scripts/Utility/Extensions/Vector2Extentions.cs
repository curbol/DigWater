using UnityEngine;

public static class Vector2Extentions
{
    public static Vector2 DegreeToVector(this int degrees)
    {
        return degrees.DegreeToVector();
    }

    public static Vector2 DegreeToVector(this float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static Vector2 SetX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.y);
    }

    public static Vector2 SetY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, y);
    }

    public static Collider2D[] GetNeighbors(this Vector3 position, float radius)
    {
        return ((Vector2)position).GetNeighbors(radius);
    }

    public static Collider2D[] GetNeighbors(this Vector2 position, float radius)
    {
        return Physics2D.OverlapCircleAll(position, radius);
    }

    public static Collider2D[] GetNeighbors(this Vector3 position, float radius, LayerMask layerMask)
    {
        return ((Vector2)position).GetNeighbors(radius);
    }

    public static Collider2D[] GetNeighbors(this Vector2 position, float radius, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(position, radius, layerMask);
    }
}