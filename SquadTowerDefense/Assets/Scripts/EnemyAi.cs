using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyAi : MonoBehaviour
{
    private void Start()
    {
        var targetGameObject = GameObject.FindWithTag("Target");

        if (targetGameObject == null)
        {
            Debug.LogError("Could not find the target.  Make sure it has the appropriate Target tag.");
            return;
        }
            

        var aiSetter = GetComponent<AIDestinationSetter>();
        
        if(aiSetter == null)
        {
            Debug.LogError("Cannot find the AI Desitination Setter component.");
            return;
        }

        aiSetter.target = targetGameObject.transform;
    }
}
