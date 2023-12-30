using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildTask : ITask
{
    public Vector3 buildLocation;
    public GameObject prefabToBuild;
    
    private bool isTaskCompleted = false;
    
    public void PerformTask()
    {
        Debug.Log("Performing the build task.");
    }

    public bool TaskIsCompleted()
    {
        return isTaskCompleted;
    }
}
