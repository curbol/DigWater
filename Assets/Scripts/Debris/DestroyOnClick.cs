using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    public void Destroy()
    {
        GameObject.Destroy(this);
    }
}