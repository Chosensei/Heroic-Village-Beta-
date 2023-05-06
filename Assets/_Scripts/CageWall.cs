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
    public float CurrentWallHP { get => currentWallHP; set => currentWallHP = value; }
    public float MaxWallHP { get => maxWallHP; set => maxWallHP = value; }
    public int UpgradeLevel { get => upgradeLevel; set => upgradeLevel = value; }
    public int MaxLevel { get => maxUpgradeLevel; private set => maxUpgradeLevel = value; }

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invisibleTimer;
    int upgradeLevel = 1;
    int maxUpgradeLevel = 9; 
    // Coroutine to handle showing and hiding the wall HP UI
    Coroutine hpCoroutine;


    void Start()
    {
        CurrentWallHP = MaxWallHP;
    }

    void Update()
    {
        if (isUnderAtk && isInvincible)
        {
            // Enable wall HP UI 
            UIManager.Instance.WallUIObjects[wallIndex].SetActive(true);
            invisibleTimer -= Time.deltaTime;
            if (invisibleTimer < 0)
                isInvincible = false;
        }

    }
    public bool IsDead()
    {
        return CurrentWallHP <= 0;
    }
    public void TakeDamage(int amount, int index)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invisibleTimer = timeInvincible; // reset timer for invulnerability
            CurrentWallHP -= amount;
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
            UIManager.Instance.UpdateWallHP(index, CurrentWallHP, MaxWallHP);
            isUnderAtk = false;
        }
        if (CurrentWallHP <= 0)
        {
            CurrentWallHP = 0; 
            isDestroyed = true; 
            isUnderAtk = false;
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
