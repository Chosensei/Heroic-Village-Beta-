using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    BoxCollider colliderWeapon;
    [SerializeField] private GameObject objWeapon; 
    void Start()
    {
        colliderWeapon = objWeapon.GetComponent<BoxCollider>();
        colliderWeapon.enabled = false;
    }

    

    private void Attack()
    {
        colliderWeapon.enabled = true;
    }
}
