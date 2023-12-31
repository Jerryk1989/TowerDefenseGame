using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildTask : ITask
{
    public Transform startingLocation;
    public Vector3 buildLocation;
    public GameObject prefabToBuild;
    
    private bool isTaskCompleted = false;

    public void PerformTask(TaskWorker worker)
    {
        worker.MoveToLocation(prefabToBuild.transform, () =>
        {
            Debug.Log("Moved to location!!!");
        });
    }

    public bool TaskIsCompleted()
    {
        return isTaskCompleted;
    }
}
