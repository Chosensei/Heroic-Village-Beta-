using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health = 100;
    public string damageTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(damageTag))
        {
            health -= 5;
        }
    }
}
