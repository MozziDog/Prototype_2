using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : Enemy_Base
{
    Vector3 targetPositionForAir;
    Vector3 tempPos;

    new void Start()
    {
        base.Start();
        //tempPos = transform.TransformPoint(GameManagerObject.transform.position);
        targetPositionForAir = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }


    private void Update()
    {
        // AgentStuckAvoid();
        if (isWalking)
            AirMove();

    }
    /*
    public void AgentStuckAvoid()
    {
        if (isWalking && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.speed < 0.3)
        {
            Debug.LogWarning("enemy Repathing!!");
            agent.enabled = false;
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed;
        }
    }
    */
    public void AirMove()
    {
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        transform.position = Vector3.MoveTowards(this.transform.position, targetPositionForAir, moveSpeed * Time.deltaTime);

    }


    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);


    }




}
