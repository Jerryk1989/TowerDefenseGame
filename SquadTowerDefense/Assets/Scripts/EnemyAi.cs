using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyAi : MonoBehaviour
{
    public List<GameObject> pathWaypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    public float waypointProximityThreshold = 1.0f;

    private Transform currentWayPoint;
    private int currentWayPointIndex;
    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        if(pathWaypoints == null || !pathWaypoints.Any())
            Debug.LogError("You need to add path waypoints to the pathWaypoints list.");

        agent = GetComponent<NavMeshAgent>();
        
        if(agent == null)
            Debug.LogError("Could not find NavMesh agent.");
        
        GetNextWaypoint(0);
        agent.SetDestination(pathWaypoints[0].transform.position);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentWayPoint.position) < 3f)
        {
            if (currentWayPointIndex + 1 < pathWaypoints.Count)
            {
                GetNextWaypoint(currentWayPointIndex + 1);

                if (target != currentWayPoint)
                {
                    agent.SetDestination(currentWayPoint.position);
                }
            }
        }
    }
    
    private void GetNextWaypoint(int waypointIndex)
    {
        currentWayPoint = pathWaypoints[waypointIndex].transform;
        currentWayPointIndex = waypointIndex;
    }
}
