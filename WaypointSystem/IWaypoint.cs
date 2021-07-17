using UnityEngine;

namespace WaypointSystem
{
    public interface IWaypoint
    {
        public bool IsNearToWaypoint(Transform self, float distance);
        public float IsNearToWaypoint(Transform self);
        public void NewWaypoint(int index = -1);
        public void AddPoint(Transform pos = null);
    }
}