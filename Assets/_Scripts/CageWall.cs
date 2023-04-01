using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageWall : MonoBehaviour
{
    [Header("Serializable Stats")]
    [SerializeField] private float currentWallHP;
    [SerializeField] private float maxWallHP;
    [SerializeField] private int wallIndex;

    private EnemyTarget enemyTarget;
    public bool isUnderAtk { get; set; } = false;
    public bool isDestroyed { get; set; } = false;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invisibleTimer;

    void Start()
    {
        currentWallHP = maxWallHP;
        enemyTarget = GetComponent<EnemyTarget>();
        //this.wallIndex = enemyTarget.wallTargetIndex;
    }

    void Update()
    {
        if (isUnderAtk && isInvincible)
        {
            invisibleTimer -= Time.deltaTime;
            if (invisibleTimer < 0)
                isInvincible = false;
        }

    }
    public void TakeDamage(int amount, int index)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invisibleTimer = timeInvincible; // reset timer for invulnerability
            currentWallHP -= amount;
            UIManager.Instance.UpdateWallHP(index, currentWallHP, maxWallHP);
            isUnderAtk = false;
        }
        if (currentWallHP <= 0)
        {
            isDestroyed = true; 
            isUnderAtk = false;
        }

    }
}
