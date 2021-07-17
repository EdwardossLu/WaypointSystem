using UnityEngine;
using UnityEditor;

namespace WaypointSystem
{
    [CustomEditor(typeof(WaypointsSystem))]
    public class WaypointEditor : Editor
    {
        private WaypointsSystem _waypoints = null;

        private void OnEnable() => _waypoints = (WaypointsSystem) target;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginVertical("GroupBox");
            _waypoints.toggleDebug = EditorGUILayout.BeginToggleGroup
                (new GUIContent("Show Debug", "Disable or Enable all gizmos"), _waypoints.toggleDebug);
            
            _waypoints.pointColour = EditorGUILayout.ColorField
                (new GUIContent("Point Color", "Change point color"), _waypoints.pointColour);
            _waypoints.lineColour = EditorGUILayout.ColorField
                (new GUIContent("Line Color", "Change line color"), _waypoints.lineColour);
            
            EditorGUILayout.Space(5);
            
            _waypoints._sphereSize = EditorGUILayout.Slider
                (new GUIContent("Sphere Size", "Change the size of the gizmo spheres"), _waypoints._sphereSize, 0, 20f);

            SceneView.RepaintAll();
            
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Space(5);
            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("Waypoints", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(new GUIContent("Add Point", "Add new waypoint"))) AddNewPoint();
            if (GUILayout.Button(new GUIContent("Clear All Waypoints", "Clear all waypoints available"))) ClearAllWaypoints();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            
            SerializedObject list = new SerializedObject(target);
            SerializedProperty locations = list.FindProperty("waypointList");
            list.Update();
            EditorGUILayout.PropertyField(locations, true);
            list.ApplyModifiedProperties();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void AddNewPoint() => _waypoints.AddPoint();
        private void ClearAllWaypoints() => _waypoints.ClearAllWaypoints();
    }   
}
