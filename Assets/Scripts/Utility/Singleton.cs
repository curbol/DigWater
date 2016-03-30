using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    protected static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    Debug.LogError("An instance of " + typeof(T) + " was not found, so an instance was created.");
                }
            }

            return instance;
        }
    }
}