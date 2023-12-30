using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    public void PerformTask();
    public bool TaskIsCompleted();
}
