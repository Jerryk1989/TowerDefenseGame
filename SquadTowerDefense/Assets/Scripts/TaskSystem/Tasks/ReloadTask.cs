using UnityEngine;

public class ReloadTask : ITask
{
    private bool isTaskCompleted = false;
    
    public void PerformTask()
    {
        Debug.Log("Performing the reload task.");
    }

    public bool TaskIsCompleted()
    {
        return isTaskCompleted;
    }
}
