using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TaskManager : MonoBehaviour
{
    private static TaskManager _instance;
    
    private Queue<ITask> tasks = new Queue<ITask>();
    
    public static TaskManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject managerObject = new GameObject("TaskManager");
                _instance = managerObject.AddComponent<TaskManager>();
            }

            return _instance;
        }
    }

    public void AddTask(ITask task)
    {
        tasks.Enqueue(task);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        if (tasks.Count > 0)
        {
            ITask task = tasks.Dequeue();
            
            task.PerformTask();
        }
    }
}
