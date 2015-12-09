using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = target as MapGenerator;

        if (GUILayout.Button("Clear Map"))
        {
            mapGenerator.CurrentMap.Clear();
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
            if (mapGenerator.CurrentMap != null)
            {
                Vector2 mousePosition = Event.current.mousePosition + new Vector2(0, -2 * Event.current.mousePosition.y + SceneView.currentDrawingSceneView.camera.pixelHeight);
                Vector2 sceneMousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
                Coordinate coordinateToDig = mapGenerator.CurrentMap.GetCoordinateFromPosition(sceneMousePosition);

                mapGenerator.CurrentMap.Draw(coordinateToDig, mapGenerator.selectedDrawingSoilType, mapGenerator.drawRadius);

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
        else if (Event.current.type == EventType.Layout)
        {
            //This somehow allows e.Use() to actually function and block mouse input
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }
    }
}