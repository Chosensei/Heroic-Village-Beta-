using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyVillageContraints : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add restrictions
            //GMDebug.Instance.battleStarted = false;
            //GMDebug.Instance.hasLeftTown = true;
            //GameManager.Instance.hasReturnedToVillage = true;
            //GameManager.Instance.hasLeftVillage = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add restrictions
            //GMDebug.Instance.battleStarted = true;
            //GMDebug.Instance.hasLeftTown = false;

            //GameManager.Instance.hasReturnedToVillage = false;
        }
    }

}
