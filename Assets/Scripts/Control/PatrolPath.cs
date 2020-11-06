using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointPointRadius = .25f;                

        private void Start()
        {            
            
        }

        private void OnDrawGizmos()
        {            
            for (int i = 0; i < transform.childCount; i++)
            { 
                int j = GetNextIndex(i);                
                Vector3 waypoint = GetWaypoint(i);
                Vector3 nextWaypoint = GetWaypoint(j);
                Gizmos.DrawSphere(waypoint, waypointPointRadius);
                Gizmos.DrawLine(waypoint, nextWaypoint);
            }
        }

        public int GetNextIndex(int i)
        {
            if (i+1 >= transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }


        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
