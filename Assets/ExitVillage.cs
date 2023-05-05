using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVillage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if player has left town door
            GMDebug.Instance.hasLeftTown = true; 
        }
    }

}
