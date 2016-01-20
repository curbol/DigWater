using System.Collections;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

    [Range(-360, 360)]
    [SerializeField]
    private int startRayAngle = -180;

    [Range(-360, 360)]
    [SerializeField]
    private int endRayAngle = 10;

    [Range(0, 1)]
    [SerializeField]
    private float heatRate = 0.4F;

    private void Start()
    {
        StartCoroutine(EmitRays());
    }

    private IEnumerator EmitRays()
    {
        while (true)
        {
            for (int i = startRayAngle; i <= endRayAngle; i++)
            {
                Vector2 direction = Vector2Extentions.Vector2FromAngle(i);
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity);
                float heatPercent = 1;

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    Heatable heatableObject = raycastHit.transform.GetComponent<Heatable>() as Heatable;
                    if (heatableObject == null)
                        break;

                    if (showGizmos)
                        Debug.DrawLine((Vector2)transform.position, raycastHit.point, new Color(0.5F, 0.5F, 0.2F, 0.2F));

                    heatableObject.AddHeat(heatRate * heatPercent);
                    heatPercent *= heatableObject.HeatPenetration;
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        for (int i = startRayAngle; i <= endRayAngle; i++)
        {
            Gizmos.color = new Color(0.5F, 0.5F, 0.2F, 0.3F);
            Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2Extentions.Vector2FromAngle(i) * 10);
        }
    }
}