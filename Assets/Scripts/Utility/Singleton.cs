using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    instance = new GameObject(nameof(T)).AddComponent<T>();
                    Debug.LogError("An instance of " + typeof(T) + " was not found, so an instance was created.");
                }
            }

            return instance;
        }
    }
}