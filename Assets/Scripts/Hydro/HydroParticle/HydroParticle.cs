using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Vibration))]
public class HydroParticle : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

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
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator UpdateState()
    {
        while (true)
        {
            if (HydroManager.HeatProperties.AmbientTemperatureChangePercentage <= 0.25F)
                UpdateStateFirstQuarter();
            else if (HydroManager.HeatProperties.AmbientTemperatureChangePercentage <= 0.50F)
                UpdateStateSecondQuarter();
            else if (HydroManager.HeatProperties.AmbientTemperatureChangePercentage <= 0.75F)
                UpdateStateThirdQuarter();
            else if (HydroManager.HeatProperties.AmbientTemperatureChangePercentage <= 1)
                UpdateStateForthQuarter();

            yield return null;
        }
    }

    private void UpdateStateFirstQuarter()
    {
    }

    private void UpdateStateSecondQuarter()
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
    }

    private void UpdateStateThirdQuarter()
    {
        if (!(CurrentBehavior is VaporBehavior) && !InCloudZone && WaterBehavior.PercentToVaporizationPoint >= 1)
        {
            CurrentBehavior = VaporBehavior;
            CurrentBehavior.InitializeState();
        }
        else if (!(CurrentBehavior is CloudBehavior) && InCloudZone && WaterBehavior.PercentToVaporizationPoint >= 1)
        {
            CurrentBehavior = CloudBehavior;
            CurrentBehavior.InitializeState();
        }
    }

    private void UpdateStateForthQuarter()
    {
        if (!(CurrentBehavior is VaporBehavior) && !InCloudZone && WaterBehavior.PercentToVaporizationPoint >= 1)
        {
            CurrentBehavior = VaporBehavior;
            CurrentBehavior.InitializeState();
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || CurrentBehavior == null || CurrentBehavior.HeatableObject == null)
            return;

        float medianTemp = HydroManager.HeatProperties.MaximumTemperature / 2;

        if (CurrentBehavior.HeatableObject.Temperature <= medianTemp)
        {
            Gizmos.color = Color.Lerp(new Color(0, 1, 0), new Color(1, 1, 0), CurrentBehavior.HeatableObject.Temperature / medianTemp);
        }
        else
        {
            Gizmos.color = Color.Lerp(new Color(1, 1, 0), new Color(1, 0, 0), (CurrentBehavior.HeatableObject.Temperature - medianTemp) / medianTemp);
        }

        foreach (RecyclableObject recyclableObject in GetComponentsInChildren<RecyclableObject>())
        {
            Gizmos.DrawSphere(recyclableObject.transform.position, 0.5F);
        }
    }
}