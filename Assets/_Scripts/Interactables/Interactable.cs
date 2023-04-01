using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    // References 
    [HideInInspector]
    public NavMeshAgent playerAgent;
    protected bool hasInteracted;
    bool isEnemy; 
    public virtual void MoveToInteraction(NavMeshAgent playerAgent)
    {
        isEnemy = gameObject.tag == "Enemy";
        hasInteracted = false;
        this.playerAgent = playerAgent;
        // Distance check to interactables
        playerAgent.stoppingDistance = 30f;
        playerAgent.destination = transform.position;

    }
    private void Update()
    {
        if (!hasInteracted && playerAgent != null && !playerAgent.pathPending)
        {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                if(!isEnemy)
                    Interact();
                hasInteracted = true;
            }
        }
    }
    //Ensure look direction
    public virtual void Interact()
    {
        Debug.Log("interacting with base class");
    }
}
