using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class WaterOasisController : MonoBehaviour 
{
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
	}
}
