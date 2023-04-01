using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class TowerLevelSwitch : MonoBehaviour
{
    public TowerUpgrade towerData;
    [Header("General")]
    public float baseDamage = 10f;
    public float baseFireRate = 5f;
    public float baseFiringRange = 100f;
    public int baseSellingPrice = 0; 
    [Header("Catapult")]
    public float baseAoeRadius;
    public float baseAoeBlastForce;
    [Header("Wizard")]
    public float baseDot;
    public float baseSlowMvt;
    public float baseStunDuration;
    [Header("Village")]
    public int baseIncome = 100; 

    // Storing different levels'
    public GameObject[] towerLevelUpPrefabs;
    private TowerUpgrade towerUpgrade;
    public bool WizardTower = false;  
    // Can transfer these into another script responsible for handling tower upgrades 
    public int counter = 0;
    public int currentTowerLevel = 0;
    public int maxTowerLevel = 5;

    private void Start()
    {
        //towerUpgrade = GetComponent<TowerUpgrade>();

    }
    private void OnMouseDown()
    {
        UIManager.Instance.ShowUpgradeMenu(this);
        UIManager.Instance.currentSelectedBuilding = this.gameObject; 
    }
    public void UpgradeTestMessage()
    {
        print("initiating Upgrade ");
    }
    public void UpgradeTowerPrefab()
    {
        // Check if we're safe to upgrade (We haven't reached the last level)
        if (counter < towerLevelUpPrefabs.Length - 1)
        {
            // Increase count
            counter++; 
            // Increase current level
            currentTowerLevel++;
            print("current tower level: " + currentTowerLevel + "\n");
            // Switch to the updated level
            SwitchObject(counter);
            // Switch to the updated level

            UpgradeTowerStats(currentTowerLevel);
        }
    }
    public void SellTowerPrefab()
    {
        GameManager.Instance.MoneyInBank += baseSellingPrice;
        Destroy(this);
    }
    public void ChangeWizardElementType(string elementType)
    { 
        switch (elementType)
        {
            case "Fire":
                SwitchObject(0);   
                break;
            case "Lightning":
                SwitchObject(1);    
                break;
            case "Ice":
                SwitchObject(2);   
                break;
        }
    }
    void SwitchObject(int lvl)
    {
        // 1 / 5
        // Count from zero the last level in our array
        for (int i = 0; i < maxTowerLevel; i++)
        {
            // Check if we're in the desired level, then activate
            towerLevelUpPrefabs[i].SetActive(i == lvl);
        }
    }

    public void UpgradeTowerStats(int currentLevel)
    {
        //if (currentLevel > towerUpgrade.upgrades.Length)
        //{
        //    Debug.LogWarning("No upgrade available for current level");
        //    return;
        //}

        //var upgrade = towerUpgrade.upgrades[currentLevel - 1];

        float damageBoost = towerData.upgrades[currentLevel - 1].damageBoost;
        float fireRateBoost = towerData.upgrades[currentLevel - 1].fireRateBoost;
        float rangeBoost = towerData.upgrades[currentLevel - 1].firingRangeBoost;
        float aoeRangeBoost = towerData.upgrades[currentLevel - 1].aoeRangeBoost;
        float aoeBlastBoost = towerData.upgrades[currentLevel - 1].aoeBlastBoost;
        int incomeBoost = towerData.upgrades[currentLevel - 1].incomeBoost;

        baseIncome *= incomeBoost; 
        baseAoeRadius += aoeRangeBoost;
        baseAoeBlastForce *= aoeBlastBoost;
        baseDamage += damageBoost;
        baseFireRate -= fireRateBoost;
        baseFiringRange += rangeBoost;
    }
}
