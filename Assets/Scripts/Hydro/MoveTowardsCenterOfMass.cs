using System.Collections;
using UnityEngine;

public class MoveTowardsCenterOfMass : MonoBehaviour
{
    [SerializeField]
    private float force = 0.1F;

    private void Start()
    {
        StartCoroutine(AddForceTowardsCenterOfMass());
    }

    private IEnumerator AddForceTowardsCenterOfMass()
    {
        while (true)
        {
            Pushable[] childObjects = GetChildObjects();
            Vector2 childObjectsCenter = GetCenter(childObjects);

            foreach (Pushable childObject in childObjects)
            {
                Vector2 directionToMove = (childObjectsCenter - (Vector2)childObject.transform.position).normalized;

                childObject.AddForce(directionToMove * force);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private Pushable[] GetChildObjects()
    {
        return gameObject.GetComponentsInChildren<Pushable>();
    }

    private Vector2 GetCenter(Pushable[] transforms)
    {
        Vector2 sum = Vector2.zero;

        foreach (Pushable pushable in transforms)
        {
            sum += (Vector2)pushable.transform.position;
        }

        return sum / transforms.Length;
    }
}