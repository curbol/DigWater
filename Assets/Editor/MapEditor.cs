using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor
{
    private bool buttonClickRequired;

    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = target as MapGenerator;
        buttonClickRequired = GUILayout.Toggle(buttonClickRequired, "Require Button Click to Generate Map");
        bool buttonClicked = GUILayout.Button("Generate Map");
        bool randomSeed = GUILayout.Button("Random Seed");

        if (randomSeed && mapGenerator.CurrentMap != null)
        {
            mapGenerator.CurrentMap.Seed = new System.Random().Next(0, 1000);
            mapGenerator.GenerateMap();
        }

        if (DrawDefaultInspector() && !buttonClickRequired || buttonClicked)
        {
            mapGenerator.GenerateMap();
        }
    }
}
