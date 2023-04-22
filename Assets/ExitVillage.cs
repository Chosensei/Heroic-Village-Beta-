using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVillage : MonoBehaviour
{
    [SerializeField] GameObject exitLocation;
    [SerializeField] GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You are at the Exit");
            // Transport player out of town
            other.transform.position = exitLocation.transform.position;
            //GameManager.Instance.hasLeftVillage = true;
            //GameManager.Instance.isRestPhase = false; 
        }
    }
    //if (other.CompareTag("Player"))
}
