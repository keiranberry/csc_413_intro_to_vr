using UnityEditor;
using UnityEngine;

public class PositionInWorldHelper : EditorWindow
{
    private string objectName = "";
    private string objectLocation = "";
    private bool acceptClick = true;


    [MenuItem("Helper Functions/Object Position on Left Mouse Click")]
    static void InitWindow()
    {
        PositionInWorldHelper window = (PositionInWorldHelper)GetWindowWithRect(typeof(PositionInWorldHelper), new Rect(0, 0, 300, 75));
        window.Show();
    }

    void OnGUI()
    {
        //add elements
        acceptClick = EditorGUILayout.Toggle("Accept  Left Click: ", acceptClick);
        EditorGUILayout.LabelField("Object: ", objectName);
        EditorGUILayout.LabelField("World Position: ", objectLocation);

        //turn on and off watching for  Left click
        if (acceptClick)
        {
            SceneView.duringSceneGui -= OnSceneEvent;
            SceneView.duringSceneGui += OnSceneEvent;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneEvent;
        }
        
    }

    void OnSceneEvent(SceneView scene)
    {
        Event evt = Event.current;

        //detect middle mouse press
        if (evt.type == EventType.MouseDown && evt.button == 0)
        {
            //clear old data
            objectName = "";
            objectLocation = "";

            Vector3 mousePos = evt.mousePosition;

            //rescale to screen resolution
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            //find point in world space
            Ray ray = scene.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               objectName = hit.collider.name;
               objectLocation =  hit.point.ToString();
            }

            //update window
            Repaint();
        }
    }
}