using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAITester : MonoBehaviour
{
    NavMeshAgent navAgent;
    GameObject target; 
    DetectClosest dc;
    //float distToTarget;
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        dc = GetComponent<DetectClosest>();
    }

    void Update()
    {
        #region FOR DEBUGGING

        #endregion
        if (target == null)
            target = dc.ClosestTarget;

        AIMover();
    }
    private void AIMover()
    {
        //distToTarget = Vector3.Distance(target.transform.position, transform.position);
        navAgent.stoppingDistance = 10;
        navAgent.SetDestination(target.transform.position);
    }
}

