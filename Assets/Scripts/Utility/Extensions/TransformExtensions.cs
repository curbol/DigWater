using UnityEngine;

public static class TransformExtensions
{
    public static void SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static void SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public static void SetZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public static GameObject CreateUniqueChildGameObject(this Transform transform, string gameObjectName)
    {
        if (transform.FindChild(gameObjectName))
        {
            Object.DestroyImmediate(transform.FindChild(gameObjectName).gameObject);
        }

        GameObject gameObjectHolder = new GameObject(gameObjectName);
        gameObjectHolder.transform.parent = transform;

        return gameObjectHolder;
    }
}