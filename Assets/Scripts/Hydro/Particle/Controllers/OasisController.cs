using UnityEngine;

[DisallowMultipleComponent]
public class OasisController : MonoBehaviour
{
    [SerializeField]
    private OasisLiquidBehavior liquidBehavior;
    private OasisLiquidBehavior LiquidBehavior
    {
        get
        {
            if (liquidBehavior == null)
                liquidBehavior = gameObject.GetSafeComponent<OasisLiquidBehavior>();

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