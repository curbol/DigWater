using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
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

    public void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            e.Use();

            MapGenerator mapGenerator = target as MapGenerator;
            if (mapGenerator.CurrentMap != null)
            {
                Vector2 mousePosition = Event.current.mousePosition + new Vector2(0, -2 * Event.current.mousePosition.y + SceneView.currentDrawingSceneView.camera.pixelHeight);
                Vector2 sceneMousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
                Coordinate coordinateToDig = mapGenerator.CurrentMap.GetCoordinateFromPosition(sceneMousePosition);

                mapGenerator.CurrentMap.Draw(coordinateToDig, SoilType.Dirt, 4);

                if (mapGenerator.MapHolder == null)
                {
                    mapGenerator.GenerateMap();
                }

                SoilMapController soilMapController = mapGenerator.MapHolder.GetComponent<SoilMapController>();

                if (soilMapController != null)
                {
                    soilMapController.RedrawSoilMesh();
                }

                Event.current.Use();
            }
        }
        else if (e.type == EventType.Layout)
        {
            //This somehow allows e.Use() to actually function and block mouse input
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }
    }
}