using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Temporary Stat")]
    public float damage = 10f;
    public BoxCollider weaponCollider;

    private void OnTriggerEnter(Collider other)
    {
        // If the collider hits an enemy, damage it
        if (other.tag == "Enemy")
        {
            print("sword hit!");
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage(gameObject, damage);
            }
        }
    }
}
