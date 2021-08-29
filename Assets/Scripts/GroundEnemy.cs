using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy_Base
{
    NavMeshAgent agent;


    new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
        agent.speed = moveSpeed;
    }


    private void Update()
    {
        // AgentStuckAvoid();

    }
    /*
    public void AgentStuckAvoid()
    {
        if (isWalking && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.speed > 0.3)
        {
            Debug.LogWarning("enemy Repathing!!");
            agent.enabled = false;
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed;
        }
    }
    */

}
