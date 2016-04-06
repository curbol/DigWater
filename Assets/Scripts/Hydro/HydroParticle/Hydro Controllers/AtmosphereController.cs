﻿using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class AtmosphereController : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

	[SerializeField]
	private Heatable heatable;
	private Heatable Heatable
	{
		get
		{
			if (heatable == null)
				heatable = gameObject.GetSafeComponent<Heatable>();

			return heatable;
		}
	}

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
			if (Heatable == null)
                return false;

			return Heatable.Temperature >= HeatManager.EvaporationPoint;
        }
    }

    public bool TemperatureInFirstQuarter
    {
        get
        {
            return Mathf.Abs(HeatManager.AmbientHeatRate) <= 0.25F;
        }
    }

    public bool TemperatureInSecondQuarter
    {
        get
        {
            return Mathf.Abs(HeatManager.AmbientHeatRate) > 0.25F && Mathf.Abs(HeatManager.AmbientHeatRate) <= 0.5F;
        }
    }

    public bool TemperatureInThirdQuarter
    {
        get
        {
            return Mathf.Abs(HeatManager.AmbientHeatRate) > 0.5F && Mathf.Abs(HeatManager.AmbientHeatRate) <= 0.75F;
        }
    }

    public bool TemperatureInForthQuarter
    {
        get
        {
            return Mathf.Abs(HeatManager.AmbientHeatRate) > 0.75F;
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
            else if (!(CurrentBehavior is EvaporationBehavior) && !TemperatureInFirstQuarter && IsEvaporated && !transform.InCloudZone())
            {
                CurrentBehavior = EvaporationBehavior;
            }
            else if (!(CurrentBehavior is CondensationBehavior) && !TemperatureInForthQuarter && IsEvaporated && transform.InCloudZone())
            {
                CurrentBehavior = CondensationBehavior;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
		if (!showGizmos || Heatable == null)
            return;

        float medianTemp = HeatManager.MaximumTemperature / 2;

		if (Heatable.Temperature <= medianTemp)
        {
			Gizmos.color = Color.Lerp(new Color(0, 1, 0), new Color(1, 1, 0), Heatable.Temperature / medianTemp);
        }
        else
        {
			Gizmos.color = Color.Lerp(new Color(1, 1, 0), new Color(1, 0, 0), (Heatable.Temperature - medianTemp) / medianTemp);
        }

		Gizmos.DrawSphere(Heatable.transform.position, 0.5F);
    }
}