using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoints : MonoBehaviour
{
    public float GetPercentage()
    {
        return 100 * GetFraction();
    }

    public float GetFraction()
    {
        return GetComponent<EnemyBehavior>().enemyData.currentHealth / GetComponent<EnemyBehavior>().enemyData.maxHealth;
    }
}
