using System.Collections;
using UnityEngine;

public class OutOfBoundsDestroy : RecyclableObject
{
    private void Start()
    {
        StartCoroutine(UpdateDeath());
    }

    private IEnumerator UpdateDeath()
    {
        while (true)
        {
            if (!transform.InMapRegion())
            {
                if (OnDeath != null)
                    OnDeath();

                Destroy(gameObject);
            }

            yield return new WaitForSeconds(2);
        }
    }
}