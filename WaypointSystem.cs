using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour
{
    [Header("General")]

    [Tooltip("How fast the object should move between waypoints.")]
    [Range(0.1f, 10f)] [SerializeField] private float speed = 1f;

    [Tooltip("How long should we wait until we move to the next waypoint.")]
    [SerializeField] private float waitingTimer = 1f;

    [Tooltip("List of waypoints for the object to move towards.")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    // The amount of waypoints the player has reached.
    private int _points = 0;

    // The combine wait time for "waitingTimer" and global time.
    private float _timer = 0;
    

    private void Awake() 
    {
        if (_points != 0) _points = 0;
    }

    private void Update() 
    {
        MoveToNextWaypoint();
    }

    // Move the gameObject to the desired location.
    private void MoveToNextWaypoint()
    {
        float waypointMoveSpeed = speed * Time.deltaTime;

        // Check if the points don't match the amount of waypoints && the stop timer doesn't match the timer. 
        if (_points != waypoints.Count && Time.time >= _timer)
        {
            // Move the gameObject to the disired location.
            transform.position = Vector3.MoveTowards(transform.position, waypoints[_points].position, waypointMoveSpeed);

            // If the gameObject has reached this location of the waypoint.
            if (transform.position == waypoints[_points].position)
            {
                ++_points;                                                                                  // Plus 1 to "_points".
                _timer = Time.time + waitingTimer;                                       // plus current game time to "waitingTimer". 
            }
        }
        // Check if the points does match the amount of waypoints && the stop timer doesn't match the timer. 
        else if (_points == waypoints.Count && Time.time >= _timer)
        {
            // Move the gameObject to the first waypoint.
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, waypointMoveSpeed);

            // If the gameObject has reached the first waypoint.
            if (transform.position == waypoints[0].position)
            {
                _points = 0;                                                                                // Reset "_points".
                _points = Time.time + waitingTimer;                                      // plus current game time to "waitingTimer". 
            }
        }
    }

    // Display each waypoint inside the editor.
    private void OnDrawGizmos() 
    {
        if (waypoints.Count > 0)
        {
            // Display all waypoint sphere as blue.
            Gizmos.color = Color.blue;
            foreach (Transform T in waypoints)
                Gizmos.DrawSphere(T.position, .1f);

            // Display all directional lines are white.
            Gizmos.color = Color.white;
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
    