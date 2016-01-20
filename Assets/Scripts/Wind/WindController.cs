using System.Collections;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

    [Range(0, 50)]
    [SerializeField]
    private int emmissionHeight = 10;

    [Range(0, 2)]
    [SerializeField]
    private float magnitude = 0.1F;

    private void Start()
    {
        StartCoroutine(EmitWind());
    }

    private IEnumerator EmitWind()
    {
        while (true)
        {
            for (int i = -emmissionHeight / 2; i <= emmissionHeight / 2; i++)
            {
                Vector2 startPosition = transform.position + Vector3.up * i;
                Vector2 direction = Vector2.left;
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(startPosition, direction, Mathf.Infinity);

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    PushableObject pushableObject = raycastHit.transform.GetComponent<PushableObject>() as PushableObject;
                    if (pushableObject != null)
                    {
                        if (showGizmos)
                            Debug.DrawLine(startPosition, raycastHit.point, new Color(0F, 0.8F, 1F, 0.8F));

                        pushableObject.AddForce(direction * magnitude);
                        break;
                    }
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Vector2 bottomPosition = transform.position + Vector3.down * emmissionHeight / 2;
        for (float i = 0; i <= emmissionHeight; i += 0.5F)
        {
            Gizmos.color = new Color(0F, 0.8F, 1F, 0.8F);
            Vector2 startPosition = bottomPosition + Vector2.up * i;
            Vector2 direction = Vector2.left * 10 * Mathf.Abs(Mathf.Cos(i));
            Gizmos.DrawLine(startPosition, startPosition + direction);
        }
    }
}