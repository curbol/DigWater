using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        StartCoroutine(EmitRays());
    }

    private IEnumerator EmitRays()
    {
        while (true)
        {
            var hitObjects = new List<object>();
            for (int i = startRayAngle; i <= endRayAngle; i++)
            {
                Vector2 direction = i.DegreeToVector();
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity);
                float heatPercent = 1;

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    Heatable heatableObject = raycastHit.transform.GetComponent<Heatable>() as Heatable;
                    if (heatableObject == null || hitObjects.Contains(heatableObject))
                        continue;

                    if (showGizmos)
                        Debug.DrawLine((Vector2)transform.position, raycastHit.point, new Color(0.5F, 0.5F, 0.2F, 0.2F));

                    heatableObject.AddHeat(HydroManager.GetProperties<HeatProperties>().SunHeat * heatPercent * Time.deltaTime);
                    heatPercent *= heatableObject.HeatPenetration;

                    hitObjects.Add(heatableObject);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        for (int i = startRayAngle; i <= endRayAngle; i++)
        {
            Gizmos.color = new Color(0.5F, 0.5F, 0.2F, 0.3F);
            Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + i.DegreeToVector() * 10);
        }
    }
}