using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVillage : MonoBehaviour
{
    [SerializeField] Transform exitLocation; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("You are at the Exit");
            // Transport player out of town
            other.transform.position = exitLocation.position;
            //GameManager.Instance.hasLeftVillage = true;
            //GameManager.Instance.isRestPhase = false; 
        }
    }

}
