using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{

    public void MovementOnDirection(Vector3 dir, NavMeshAgent navMeshAgent)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = dir * navMeshAgent.speed;
    }

    public void MovementToTheSelectionPosition(Vector3 selectionPos, float stoppingDistance, NavMeshAgent navMeshAgent)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.stoppingDistance = stoppingDistance;
        if(navMeshAgent.SetDestination(selectionPos))
            navMeshAgent.SetDestination(selectionPos);
    }

    public void StopMovement(NavMeshAgent navMeshAgent)
    {
        navMeshAgent.isStopped = true;
    }
}
