using System.Collections.Generic;
using UnityEngine;

public static class FindComponents
{
    public static T GetSafeComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError("Expected to find component of type " + typeof(T) + " but found none", gameObject);
        }

        return component;
    }

    public static I GetInterfaceComponent<I>(this GameObject gameObject) where I : class
    {
        I component = gameObject.GetComponent(typeof(I)) as I;

        if (component == null)
        {
            Debug.LogError("Expected to find component of type " + typeof(I) + " but found none", gameObject);
        }

        return component;
    }

    public static List<I> FindObjectsOfInterface<I>() where I : class
    {
        MonoBehaviour[] monoBehaviours = MonoBehaviour.FindObjectsOfType<MonoBehaviour>();
        List<I> list = new List<I>();

        foreach (MonoBehaviour behaviour in monoBehaviours)
        {
            I component = behaviour.GetComponent(typeof(I)) as I;

            if (component != null)
            {
                list.Add(component);
            }
        }

        return list;
    }
}