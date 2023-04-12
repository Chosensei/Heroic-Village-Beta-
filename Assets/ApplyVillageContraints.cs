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
            GameManager.Instance.hasReturnedToVillage = true;
            GameManager.Instance.hasLeftVillage = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add restrictions
            GameManager.Instance.hasReturnedToVillage = false;
        }
    }

}