using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class HydroParticle : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

    [SerializeField]
    private LiquidBehavior liquidBehavior;
    private LiquidBehavior LiquidBehavior
    {
        get
        {
            if (liquidBehavior == null)
                liquidBehavior = gameObject.GetSafeComponent<LiquidBehavior>();

            return liquidBehavior;
        }
    }

    [SerializeField]
    private EvaporationBehavior evaporationBehavior;
    private EvaporationBehavior EvaporationBehavior
    {
        get
        {
            if (evaporationBehavior == null)
                evaporationBehavior = gameObject.GetSafeComponent<EvaporationBehavior>();

            return evaporationBehavior;
        }
    }

    [SerializeField]
    private CondensationBehavior condensationBehavior;
    private CondensationBehavior CondensationBehavior
    {
        get
        {
            if (condensationBehavior == null)
                condensationBehavior = gameObject.GetSafeComponent<CondensationBehavior>();

            return condensationBehavior;
        }
    }

    public bool IsEvaporated
    {
        get
        {
            if (CurrentBehavior == null || CurrentBehavior.HeatableObject == null)
                return false;

            return CurrentBehavior.HeatableObject.Temperature >= HydroManager.Heat.EvaporationPoint;
        }
    }

    public bool InCloudZone
    {
        get
        {
            return transform.MapY() >= HydroManager.Cloud.CloudLevelLowerBound && transform.MapY() <= HydroManager.Cloud.CloudLevelUpperBound;
        }
    }

    public bool TemperatureInFirstQuarter
    {
        get
        {
            return HydroManager.Heat.AmbientTemperatureChangePercentage <= 0.25F;
        }
    }

    public bool TemperatureInSecondQuarter
    {
        get
        {
            return HydroManager.Heat.AmbientTemperatureChangePercentage > 0.25F && HydroManager.Heat.AmbientTemperatureChangePercentage <= 0.5F;
        }
    }

    public bool TemperatureInThirdQuarter
    {
        get
        {
            return HydroManager.Heat.AmbientTemperatureChangePercentage > 0.5F && HydroManager.Heat.AmbientTemperatureChangePercentage <= 0.75F;
        }
    }

    public bool TemperatureInForthQuarter
    {
        get
        {
            return HydroManager.Heat.AmbientTemperatureChangePercentage > 0.75F;
        }
    }

    private HydroBehavior currentBehavior;
    public HydroBehavior CurrentBehavior
    {
        get
        {
            return currentBehavior;
        }

        private set
        {
            if (currentBehavior != null)
                currentBehavior.StopBehavior();

            currentBehavior = value;
            if (currentBehavior != null)
            {
                currentBehavior.InitializeState();
                currentBehavior.StartBehavior();
            }
        }
    }

    private void Start()
    {
        CurrentBehavior = LiquidBehavior;

        StartCoroutine(UpdateState());
    }

    private IEnumerator UpdateState()
    {
        while (true)
        {
            if (!(CurrentBehavior is LiquidBehavior) && TemperatureInSecondQuarter && !IsEvaporated)
            {
                CurrentBehavior = LiquidBehavior;
            }
            else if (!(CurrentBehavior is EvaporationBehavior) && !TemperatureInFirstQuarter && IsEvaporated && !InCloudZone)
            {
                CurrentBehavior = EvaporationBehavior;
            }
            else if (!(CurrentBehavior is CondensationBehavior) && !TemperatureInForthQuarter && IsEvaporated && InCloudZone)
            {
                CurrentBehavior = CondensationBehavior;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || CurrentBehavior == null || CurrentBehavior.HeatableObject == null)
            return;

        float medianTemp = HydroManager.Heat.MaximumTemperature / 2;

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