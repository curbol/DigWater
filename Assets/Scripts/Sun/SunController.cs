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
    private float HeatPenetration = 0.85F;

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
                Vector2 direction = Vector2FromAngle(i);
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity);
                float heatPercent = 1;

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    WaterParticle waterParticle = raycastHit.transform.GetComponent<WaterParticle>() as WaterParticle;
                    if (waterParticle == null)
                        break;

                    Debug.DrawLine((Vector2)transform.position, raycastHit.point, new Color(0.5F, 0.5F, 0.2F, 0.2F));
                    waterParticle.Heat(heatPercent);
                    heatPercent *= HeatPenetration;

                    if (waterParticle.State == WaterState.Water || heatPercent <= 0)
                        break;
                }
            }

            yield return null;
        }
    }

    private Vector2 Vector2FromAngle(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        for (int i = startRayAngle; i <= endRayAngle; i++)
        {
            Gizmos.color = new Color(0.5F, 0.5F, 0.2F, 0.3F);
            Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2FromAngle(i) * 10);
        }
    }
}