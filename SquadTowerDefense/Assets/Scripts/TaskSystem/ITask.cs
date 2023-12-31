using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    public void PerformTask(TaskWorker worker);
    public bool TaskIsCompleted();
}
