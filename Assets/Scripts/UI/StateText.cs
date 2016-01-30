using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StateText : MonoBehaviour
{
    [SerializeField]
    private DetectHydroStates detectHydroStates;

    [SerializeField]
    private Text text;
    private Text Text
    {
        get
        {
            if (text == null)
                text = gameObject.GetSafeComponent<Text>();

            return text;
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateStateText());
    }

    private IEnumerator UpdateStateText()
    {
        while (true)
        {
            if (detectHydroStates == null)
                break;

            string stateText = "";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Metaball")))
                stateText += "Liquid   ";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Vapor")))
                stateText += "Vapor   ";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Cloud")))
                stateText += "Condensation   ";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Rain")))
                stateText += "Precipitation";

            Text.text = stateText;

            yield return new WaitForEndOfFrame();
        }
    }
}