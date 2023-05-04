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
    
    public bool isUnderAtk { get; set; } = false;
    public bool isDestroyed { get; set; } = false;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invisibleTimer;

    // Coroutine to handle showing and hiding the wall HP UI
    Coroutine hpCoroutine;

    void Start()
    {
        currentWallHP = maxWallHP;
    }

    void Update()
    {
        if (isUnderAtk && isInvincible)
        {
            // Enable wall HP UI 
            //UIManager.Instance.WallUIObjects[wallIndex].SetActive(true);
            invisibleTimer -= Time.deltaTime;
            if (invisibleTimer < 0)
                isInvincible = false;
        }
    }
    public bool IsDead()
    {
        return currentWallHP <= 0;
    }
    public void TakeDamage(int amount, int index)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invisibleTimer = timeInvincible; // reset timer for invulnerability
            currentWallHP -= amount;
            // show the HP bar for the wall currently being attacked
            if (hpCoroutine == null)
            {
                hpCoroutine = StartCoroutine(ShowWallHP(index));
            }
            else
            {
                StopCoroutine(hpCoroutine);
                hpCoroutine = StartCoroutine(ShowWallHP(index));
            }
            UIManager.Instance.UpdateWallHP(index, currentWallHP, maxWallHP);
            isUnderAtk = false;
        }
        if (currentWallHP <= 0)
        {
            isDestroyed = true; 
            isUnderAtk = false;
            // Maybe can call the enemies clear target mwthod here?

        }

    }
    IEnumerator ShowWallHP(int index)
    {
        UIManager.Instance.WallUIObjects[index].SetActive(true);

        yield return new WaitForSeconds(5f);

        UIManager.Instance.WallUIObjects[index].SetActive(false);
        hpCoroutine = null;
    }

}
