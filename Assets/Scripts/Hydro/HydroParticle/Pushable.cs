using UnityEngine;

public class Pushable : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;
    private Rigidbody2D RigidBody
    {
        get
        {
            if (rigidBody == null)
            {
                rigidBody = gameObject.GetSafeComponent<Rigidbody2D>();
            }

            return rigidBody;
        }
    }

    public void AddForce(Vector2 force)
    {
        RigidBody.AddForce(force);
    }
}