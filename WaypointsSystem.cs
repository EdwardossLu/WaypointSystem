using System.Collections.Generic;
using UnityEngine;

namespace WaypointSystem
{
    public class WaypointsSystem : MonoBehaviour, IWaypoint
    {
        // The list of waypoints the gameObject can move towards
        public List<Transform> waypointList = new List<Transform>();
        // The current waypoint
        private int _currentPoint = 0;
        // Get the lastest transform without calling the list
        public Transform GetLastestLocation => waypointList[_currentPoint];

        /* Debug */
        // Gizmo colours
        public Color pointColour = Color.white;
        public Color lineColour = Color.white;
        // Toggle gizmos
        public bool toggleDebug = true;
        // Gizmo sphere size
        public float _sphereSize = 0.2f;
        
        /// <summary>
        /// Check if gameObject is near the desired waypoint.
        /// </summary>
        /// <param name="self">The transform of the gameObject.</param>
        /// <param name="distance">The distance between self and current waypoint.</param>
        public virtual bool IsNearToWaypoint(Transform self, float distance)
        {
            // Returns false when the gameObject is near the current waypoint.
            return Vector3.Distance(self.position, waypointList[_currentPoint].position) > distance;
        }
        
        /// <summary>
        /// Check if gameObject is near the desired waypoint.
        /// </summary>
        /// <param name="self">The transform of the gameObject.</param>
        public virtual float IsNearToWaypoint(Transform self)
        {
            // Get the distance between (self)s position and current waypoint.
            return Vector3.Distance(self.position, waypointList[_currentPoint].position);
        }

        /// <summary>
        /// Set the new waypoint. This will automatically loop back to the start when there is no new waypoints.
        /// </summary>
        /// <param name="index">Manually set a new waypoint from index. Leave empty if you want it to move to the next waypoint based on (_currentPoint).</param>
        public virtual void NewWaypoint(int index = -1)
        {
            // Waypoint index check
            if (index >= 0)
            {
                // Ensures the index is not out of range
                if (index > waypointList.Count)
                {
                    Debug.LogWarning("There are no waypoints to move towards from the index you specified.");
                    return;
                }
                
                // Specify which waypoint the user wants to do to
                _currentPoint = index;
            }
            else
            {
                // Plus the current waypoint
                ++_currentPoint;
                // Reset (_currentPoint) when it's greater then what's available in the list 
                if (_currentPoint > waypointList.Count - 1) _currentPoint = 0;
            }
        }

        /// <summary>
        /// Add new waypoint
        /// </summary>
        /// <param name="pos">Set position of the waypoint. If null, the new waypoint will be positioned at position zero.</param>
        public virtual void AddPoint(Transform pos = null)
        {
            // Create an empty gameObject
            GameObject point = new GameObject("Point " + waypointList.Count);
            // Parent (point) to this gameObject
            point.transform.parent = this.transform;
            // Add it to the list of waypoints
            waypointList.Add(point.transform);

            /* Set (point) position based on:
             * - If a Vector3 has been passed
             * - If not, placed it on the position of this gameObject.
             */
            point.transform.localPosition = pos ? pos.localPosition : Vector3.zero;
        }

        /// <summary>
        /// Clear all available waypoints.
        /// </summary>
        public void ClearAllWaypoints()
        {
            // Clear the list data
            waypointList.Clear();
            // Destroy all gameObjects that are a children of this gameObject
            for (int i = transform.childCount; i > 0; i--)
            {
                // Don't destroy anything when there is nothing to destroy
                if (!transform.GetChild(0)) return;
                
                GameObject obj = transform.GetChild(i - 1).gameObject;
                DestroyImmediate(obj);
            }
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Displays gizmos in the Unity Viewport.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Toggle the gizmos from the inspector
            if (!toggleDebug) return;
            // Don't show gizmos when there's no waypoints to show
            if (waypointList.Count < 0) return;
            
            // Change gizmo sphere colour
            Gizmos.color = pointColour;                 
            // Change the colour on all waypoints available
            foreach (Transform T in waypointList)
            {
                Gizmos.DrawSphere(T.position, _sphereSize);
            }
                
            // Change gizmo line colour
            Gizmos.color = lineColour;
            // Show a line connecting to all waypoints in order
            for (int i = 0; i < waypointList.Count - 1; i++)
            {
                Gizmos.DrawLine(waypointList[i].position, waypointList[i + 1].position);
            }
            
            // Ensure there is more then 3 waypoint available before showing a line looping back to the first waypoint
            if (waypointList.Count < 2) return;
            Gizmos.DrawLine(waypointList[0].position, waypointList[waypointList.Count - 1].position);
        }
        #endif
    }   
}
