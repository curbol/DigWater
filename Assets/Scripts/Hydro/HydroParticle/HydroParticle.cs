using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Vibration))]
public class HydroParticle : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    [SerializeField]
    private WaterBehavior waterBehavior;
    private WaterBehavior WaterBehavior
    {
        get
        {
            if (waterBehavior == null)
                waterBehavior = gameObject.GetSafeComponent<WaterBehavior>();

            return waterBehavior;
        }
    }

    [SerializeField]
    private VaporBehavior vaporBehavior;
    private VaporBehavior VaporBehavior
    {
        get
        {
            if (vaporBehavior == null)
                vaporBehavior = gameObject.GetSafeComponent<VaporBehavior>();

            return vaporBehavior;
        }
    }

    [SerializeField]
    private CloudBehavior cloudBehavior;
    private CloudBehavior CloudBehavior
    {
        get
        {
            if (cloudBehavior == null)
                cloudBehavior = gameObject.GetSafeComponent<CloudBehavior>();

            return cloudBehavior;
        }
    }

    public HydroStateBehavior CurrentBehavior { get; private set; }

    public bool InCloudZone
    {
        get
        {
            return transform.MapY() >= HydroManager.CloudProperties.CloudLevelLowerBound && transform.MapY() <= HydroManager.CloudProperties.CloudLevelUpperBound;
        }
    }

    private void Start()
    {
        CurrentBehavior = WaterBehavior;
        CurrentBehavior.InitializeState();

        StartCoroutine(UpdateState());
        StartCoroutine(RunPhysicsBehavior());
        StartCoroutine(RunGraphicsBehavior());
        StartCoroutine(RunTemperatureBehavior());
    }

    private IEnumerator RunPhysicsBehavior()
    {
        while (true)
        {
            CurrentBehavior.RunPhysicsBehavior();
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator RunGraphicsBehavior()
    {
        while (true)
        {
            CurrentBehavior.RunGraphicsBehavior();
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator RunTemperatureBehavior()
    {
        while (true)
        {
            CurrentBehavior.RunTemperatureBehavior();
            yield return null;
        }
    }

    private IEnumerator UpdateState()
    {
        while (true)
        {
            if (!(CurrentBehavior is WaterBehavior) && WaterBehavior.PercentToVaporizationPoint < 1)
            {
                CurrentBehavior = WaterBehavior;
                CurrentBehavior.InitializeState();
            }
            else if (!(CurrentBehavior is VaporBehavior) && !InCloudZone && WaterBehavior.PercentToVaporizationPoint >= 1)
            {
                CurrentBehavior = VaporBehavior;
                CurrentBehavior.InitializeState();
            }
            else if (!(CurrentBehavior is CloudBehavior) && InCloudZone && WaterBehavior.PercentToVaporizationPoint >= 1)
            {
                CurrentBehavior = CloudBehavior;
                CurrentBehavior.InitializeState();
            }

            yield return null;
        }
    }
}