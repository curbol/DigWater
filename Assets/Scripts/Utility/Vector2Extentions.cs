using UnityEngine;

public static class Vector2Extentions
{
    public static Vector2 Vector2FromAngle(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}