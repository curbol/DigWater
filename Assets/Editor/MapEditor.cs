using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor
{
    private bool buttonClickRequired = false;
    private bool buttonClicked;

    public override void OnInspectorGUI()
    {
        buttonClickRequired = GUILayout.Toggle(buttonClickRequired, "Require Button Click to Generate Map");
        buttonClicked = GUILayout.Button("Generate Map");

        if (buttonClicked || !buttonClickRequired && DrawDefaultInspector())
        {
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
        }
    }
}
