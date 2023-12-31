using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class TaskWorker : MonoBehaviour
{
    private ITask currentTask;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private CharacterController controller;
    public float speed = 25;
    public float nextWaypointDistance = 2;
    public bool reachedEndOfPath;
    
    private Action onMoveCompleted;
    
    private void Awake()
    {
        currentTask = null;
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        TaskManager.Instance.AddWorkerToQueue(this);
    }

    public void MoveToLocation(Transform taskLocation, Action onMoveCompleted)
    {
        Debug.Log("MoveToLocation called");
        this.onMoveCompleted = onMoveCompleted;
        seeker.StartPath(transform.position, taskLocation.position, OnPathComplete);
        
        StartCoroutine(WaitForWorkerToGetToLocation());
    }

    private IEnumerator WaitForWorkerToGetToLocation()
    {
        while (!reachedEndOfPath)
        {
            yield return null;
        }
        
        onMoveCompleted?.Invoke();
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        DoPathing();
    }

    private void DoPathing()
    {
        if (path == null)
            return;

        reachedEndOfPath = false;
        float distanceToWaypoint;
        
        while (true) {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    reachedEndOfPath = true;
                    path = null;
                    return;
                }
            } else {
                break;
            }
        }
        
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;
        
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * speedFactor;
        
        controller.SimpleMove(velocity);
    }
}
