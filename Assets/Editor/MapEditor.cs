using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    private bool canClearMap;

    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = target as MapGenerator;

        canClearMap = GUILayout.Toggle(canClearMap, "Can Clear Map");
        bool clearMap = GUILayout.Button("Clear Map");
        bool generateMap = GUILayout.Button("Generate Map");

        if (canClearMap && clearMap)
        {
            MapManager.Map.Clear();
            mapGenerator.GenerateMap();
        }

        if (generateMap)
        {
            mapGenerator.GenerateMap();
        }

        DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        MapGenerator mapGenerator = target as MapGenerator;

        if (Application.isPlaying || !mapGenerator.enableSoilDrawing)
            return;

        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            if (MapManager.Map != null)
            {
                Vector2 mousePosition = Event.current.mousePosition + new Vector2(0, -2 * Event.current.mousePosition.y + SceneView.currentDrawingSceneView.camera.pixelHeight);
                Vector2 sceneMousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
                Coordinate coordinateToDig = MapManager.Map.GetCoordinateFromPosition(sceneMousePosition);

                MapManager.Map.Draw(coordinateToDig, mapGenerator.drawingSoilType, mapGenerator.drawRadius);

                if (mapGenerator.MapHolder == null)
                {
                    mapGenerator.GenerateMap();
                }

                MapController soilMapController = mapGenerator.MapHolder.GetComponent<MapController>();

                if (soilMapController != null)
                {
                    soilMapController.RedrawSoilMesh();
                }

                Event.current.Use();
            }
        }
        else if (Event.current.type == EventType.Layout)
        {
            //This somehow allows e.Use() to actually function and block mouse input
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }
    }
}