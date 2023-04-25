using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Attack Stat")]
    public float swordDamage = 10f;
    public BoxCollider weaponCollider;

    public float IncreaseAttackPower(float damageInc)
    {
        return swordDamage + damageInc; 
    }
    private void OnTriggerEnter(Collider other)
    {
        // If the collider hits an enemy, damage it
        if (other.tag == "Enemy")
        {
            print("sword hit!");
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage(gameObject, IncreaseAttackPower(swordDamage));
            }
        }
    }
}
