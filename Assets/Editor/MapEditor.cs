using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor
{
    private bool buttonClickRequired;

    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;
        buttonClickRequired = GUILayout.Toggle(buttonClickRequired, "Require Button Click to Generate Map");
        bool buttonClicked = GUILayout.Button("Generate Map");
        bool randomSeed = GUILayout.Button("Random Seed");

        if (randomSeed)
        {
            map.seed = new System.Random().Next(0, 1000);
            map.GenerateMap();
        }

        if (DrawDefaultInspector() && !buttonClickRequired || buttonClicked)
        {
            map.GenerateMap();
        }
    }
}
