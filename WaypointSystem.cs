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

    private int _point = 0;

    private float _timer = 0;
    

    private void Awake() 
    {
        if (_point != 0) _point = 0;
    }

    private void Update() 
    {
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        float waypointMoveSpeed = speed * Time.deltaTime;

        if (_point != waypoints.Count && Time.time >= _timer)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[_point].position, waypointMoveSpeed);

            if (transform.position == waypoints[_point].position)
            {
                ++_point;
                _timer = Time.time + waitingTimer;
            }
        }
        else if (_point == waypoints.Count && Time.time >= _timer)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, waypointMoveSpeed);

            if (transform.position == waypoints[0].position)
            {
                _point = 0;
                _timer = Time.time + waitingTimer;
            }
        }
    }

    private void OnDrawGizmos() 
    {
        if (waypoints.Count > 0)
        {
            Gizmos.color = Color.blue;

            foreach (Transform T in waypoints)
                Gizmos.DrawSphere(T.position, .1f);

            Gizmos.color = Color.white;

            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
    