using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class TowerLevelSwitch : MonoBehaviour
{
    public TowerUpgrade towerData;
    [Header("General")]
    public float baseDamage = 10f;
    public float baseFireRate = 5f;
    public float baseFiringRange = 50f;
    public int baseSellingPrice = 0;
    public int baseUpgradePrice = 0; 
    [Header("Catapult")]
    public float baseAoeRadius = 5;
    public float baseAoeBlastForce;
    public float baseAnimationSpeed;
    [Header("Wizard")]
    public float baseEffectDuration; 
    public float baseDot;
    public float baseSlowMvt;
    public float baseStun;
    public GameObject stunCircleZone = null; 
    [Header("Village")]
    public int baseIncome = 100;

    // Storing different levels'
    public GameObject[] towerLevelUpPrefabs;
    private TowerUpgrade towerUpgrade;
    public bool VillageHouse = false; 
    public bool ArcherTower = false;
    public bool CatapultTower = false; 
    public bool WizardTower = false;
    private bool fireType = false;
    private bool lightningType = false;
    private bool iceType = false;

    // Can transfer these into another script responsible for handling tower upgrades 
    public int counter = 0;
    public int currentTowerLevel = 1;
    public int maxTowerLevel = 5;

    private int upgradedIncomeAmount; 
    private float upgradedDamageAmount;
    private float upgradedFireRateAmount;
    private float upgradedFiringRangeAmount;
    private float upgradedAoeRangeAmount;
    private float upgradedAoeBlastForceAmount;
    private float upgradedEffectDurationAmount;
    private float upgradedDOTAmount; 

    private void Start()
    {
        //towerUpgrade = GetComponent<TowerUpgrade>();
        upgradedIncomeAmount = baseIncome + towerData.upgrades[currentTowerLevel - 1].incomeBoost;
        upgradedDamageAmount = baseDamage + towerData.upgrades[currentTowerLevel - 1].damageBoost;
        upgradedFireRateAmount = baseFireRate - towerData.upgrades[currentTowerLevel - 1].fireRateBoost;
        upgradedFiringRangeAmount = baseFiringRange + towerData.upgrades[currentTowerLevel - 1].firingRangeBoost;
        baseSellingPrice = towerData.upgrades[currentTowerLevel - 1].saleAmount;
        baseUpgradePrice = towerData.upgrades[currentTowerLevel - 1].upgradeAmount;
        upgradedAoeRangeAmount = baseAoeRadius + towerData.upgrades[currentTowerLevel - 1].aoeRangeBoost;
        upgradedAoeBlastForceAmount = baseAoeBlastForce + towerData.upgrades[currentTowerLevel - 1].aoeBlastBoost;
        upgradedEffectDurationAmount = baseEffectDuration - towerData.upgrades[currentTowerLevel - 1].effectDurationBoost;
        upgradedDOTAmount = baseDot + towerData.upgrades[currentTowerLevel - 1].dotBoost;
        Debug.Log($"baseIncome: {baseIncome}"); 
        Debug.Log($"baseDamage: {baseDamage}");
        Debug.Log($"baseFirerate: {baseFireRate}");
        Debug.Log($"baseEffectDuration: {baseEffectDuration}");
        Debug.Log($"baseDOT: {baseDot}");

    }
    private void OnMouseDown()
    {
        UIManager.Instance.ShowUpgradeMenu(this); 
        UIManager.Instance.currentSelectedBuilding = this.gameObject;

        if (WizardTower == true)
        {
            UIManager.Instance.SwitchElementUIMenu.gameObject.SetActive(true); 
            Debug.Log("Wizard tower selected");
        } 
        else { UIManager.Instance.SwitchElementUIMenu.gameObject.SetActive(false); }

        DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseIncome, upgradedIncomeAmount, 
            baseDamage, baseFireRate, baseFiringRange, baseAoeRadius, baseAoeBlastForce, upgradedDamageAmount, upgradedFireRateAmount, 
            upgradedFiringRangeAmount, upgradedAoeRangeAmount, upgradedAoeBlastForceAmount, upgradedEffectDurationAmount, upgradedDOTAmount);

    }
    public void UpgradeTestMessage()
    {
        print("initiating Upgrade ");
    }
    public void UpgradeTowerPrefab()
    {
        // Update Money in bank
        GMDebug.Instance.MoneyInBank -= baseUpgradePrice;
        UIManager.Instance.moneyText.text = GMDebug.Instance.MoneyInBank.ToString();

        // Check if we're safe to upgrade (We haven't reached the last level)
        if (counter < towerLevelUpPrefabs.Length - 1)
        {
            // Increase count
            counter++; 
            // Increase current level
            currentTowerLevel++;
            // Debug
            print("current tower level: " + currentTowerLevel + "\n");

            // Switch to the updated level
            SwitchObject(counter);

            // Upgrade tower
            UpgradeTowerStats(currentTowerLevel);
        }
    }
    public void SellTowerPrefab()
    {
        GMDebug.Instance.MoneyInBank += baseSellingPrice;
        UIManager.Instance.moneyText.text = GMDebug.Instance.MoneyInBank.ToString();
        Destroy(this.gameObject);
    }
    public void ChangeWizardElementType(string elementType)
    { 
        switch (elementType)
        {
            case "Fire":
                SwitchObject(0);
                fireType = true; 
                lightningType = false;
                iceType = false;
                break;
            case "Lightning":
                SwitchObject(1);
                fireType = false;
                lightningType = true;
                iceType = false;
                break;
            case "Ice":
                SwitchObject(2);
                fireType = false;
                lightningType = false;
                iceType = true;
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
        int incomeboost = towerData.upgrades[currentLevel - 1].incomeBoost;
        float damageBoost = towerData.upgrades[currentLevel - 1].damageBoost;
        float fireRateBoost = towerData.upgrades[currentLevel - 1].fireRateBoost;
        float rangeBoost = towerData.upgrades[currentLevel - 1].firingRangeBoost;
        float aoeRangeBoost = towerData.upgrades[currentLevel - 1].aoeRangeBoost;
        float aoeBlastBoost = towerData.upgrades[currentLevel - 1].aoeBlastBoost;
        float catapultAnimBoost = towerData.upgrades[currentLevel - 1].animSpeedBoost;
        float effectDurationBoost = towerData.upgrades[currentTowerLevel - 1].effectDurationBoost;
        float dotBoost = towerData.upgrades[currentTowerLevel - 1].dotBoost; 

        int resellValue = towerData.upgrades[currentLevel - 1].saleAmount;
        int upgradeValue = towerData.upgrades[currentLevel - 1].upgradeAmount;
        baseSellingPrice = resellValue;
        baseUpgradePrice = upgradeValue;

        // FOR CATAPULT ONLY
        baseAnimationSpeed = catapultAnimBoost; 

        upgradedIncomeAmount = baseIncome + incomeboost; 
        upgradedDamageAmount = baseDamage + damageBoost;
        upgradedFireRateAmount = baseFireRate - fireRateBoost;
        upgradedFiringRangeAmount = baseFiringRange + rangeBoost;
        upgradedAoeRangeAmount = baseAoeRadius + aoeRangeBoost;
        upgradedAoeBlastForceAmount = baseAoeBlastForce + aoeBlastBoost;
        upgradedEffectDurationAmount = baseEffectDuration - effectDurationBoost;
        upgradedDOTAmount = baseDot + dotBoost;
        //int nextLevel = currentTowerLevel++;
        DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseIncome, upgradedIncomeAmount,
        baseDamage, baseFireRate, baseFiringRange, baseAoeRadius, baseAoeBlastForce, upgradedDamageAmount, upgradedFireRateAmount,
        upgradedFiringRangeAmount, upgradedAoeRangeAmount, upgradedAoeBlastForceAmount, upgradedEffectDurationAmount, upgradedDOTAmount);
        //DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseIncome, upgradedIncomeAmount, baseDamage, baseFireRate, baseFiringRange, baseAoeRadius, baseAoeBlastForce, upgradedDamageAmount, upgradedFireRateAmount, upgradedFiringRangeAmount, upgradedAoeRangeAmount, upgradedAoeBlastForceAmount);
        baseIncome = upgradedIncomeAmount; 
        baseDamage = upgradedDamageAmount;
        baseFireRate = upgradedFireRateAmount;
        baseFiringRange = upgradedFiringRangeAmount;
        baseAoeRadius = upgradedAoeRangeAmount;
        baseAoeBlastForce = upgradedAoeBlastForceAmount;
        baseEffectDuration = upgradedEffectDurationAmount;
        baseDot = upgradedDOTAmount; 
    }
    public void DisplayCurrentTowerStats(int currentTowerLevel, int nextLevel, int sellPrice, int upgradeCost, int income, int upgradedIncome, float baseDamage, float baseFireRate, float baseFiringRange, float baseAoeRange, float baseAoeBlastForce, float upgradedDamage, float upgradedFireRate, float upgradedFiringRange, float upgradedAoeRange, float upgradedAoeBlastForce, float upgradedFxDuration, float upgradedDot)
    {
        UIManager.Instance.CurrentLevelValue.text = currentTowerLevel.ToString();
        //UIManager.Instance.NextLevelValue.text = nextLevel.ToString();

        if (currentTowerLevel <= maxTowerLevel) { UIManager.Instance.NextLevelValue.text = nextLevel.ToString(); }

        if (VillageHouse == true) 
        {
            // Set texts
            UIManager.Instance.BuildingName1.text = "VILLAGE HOUSE";
            UIManager.Instance.BuildingName2.text = "VILLAGE HOUSE";
            UIManager.Instance.StatText1.text = "INCOME:";
            UIManager.Instance.StatText2.text = "";
            UIManager.Instance.StatText3.text = "";
            UIManager.Instance.StatText4.text = "";
            UIManager.Instance.UpgradedStatText1.text = "INCOME:";
            UIManager.Instance.UpgradedStatText2.text = "";
            UIManager.Instance.UpgradedStatText3.text = "";
            UIManager.Instance.UpgradedStatText4.text = "";

            // Set values
            UIManager.Instance.StatValue1.text = income.ToString();
            UIManager.Instance.StatValue2.text = "";
            UIManager.Instance.StatValue3.text = "";
            UIManager.Instance.StatValue4.text = "";
            UIManager.Instance.UpgradedStatValue1.text = upgradedIncome.ToString();
            UIManager.Instance.UpgradedStatValue2.text = "";
            UIManager.Instance.UpgradedStatValue3.text = "";
            UIManager.Instance.UpgradedStatValue4.text = "";
            UIManager.Instance.SellPriceValue.text = sellPrice.ToString();
            UIManager.Instance.UpgradeCostValue.text = upgradeCost.ToString();
        }
        if (ArcherTower == true) 
        {
            // Set texts
            UIManager.Instance.BuildingName1.text = "ARCHER TOWER";
            UIManager.Instance.BuildingName2.text = "ARCHER TOWER";
            UIManager.Instance.StatText1.text = "DAMAGE:";
            UIManager.Instance.StatText2.text = "FIRE RATE:";
            UIManager.Instance.StatText3.text = "RANGE:";
            UIManager.Instance.StatText4.text = "";
            UIManager.Instance.UpgradedStatText1.text = "DAMAGE:";
            UIManager.Instance.UpgradedStatText2.text = "FIRE RATE:";
            UIManager.Instance.UpgradedStatText3.text = "RANGE:";
            UIManager.Instance.UpgradedStatText4.text = "";
            

            // Set values
            UIManager.Instance.StatValue1.text = baseDamage.ToString();
            UIManager.Instance.StatValue2.text = baseFireRate.ToString();
            UIManager.Instance.StatValue3.text = baseFiringRange.ToString();
            UIManager.Instance.StatValue4.text = "";
            UIManager.Instance.UpgradedStatValue1.text = upgradedDamage.ToString();
            UIManager.Instance.UpgradedStatValue2.text = upgradedFireRate.ToString();
            UIManager.Instance.UpgradedStatValue3.text = upgradedFiringRange.ToString();
            UIManager.Instance.UpgradedStatValue4.text = "";
            
            UIManager.Instance.SellPriceValue.text = sellPrice.ToString();
            UIManager.Instance.UpgradeCostValue.text = upgradeCost.ToString();
        }
        if (CatapultTower == true) 
        {
            // Set texts
            UIManager.Instance.BuildingName1.text = "CATAPULT CANNON";
            UIManager.Instance.BuildingName2.text = "CATAPULT CANNON";
            UIManager.Instance.StatText1.text = "DAMAGE:";
            UIManager.Instance.StatText2.text = "FIRE RATE:";
            UIManager.Instance.StatText3.text = "AOE RANGE";
            UIManager.Instance.StatText4.text = "IMPACT";
            UIManager.Instance.UpgradedStatText1.text = "DAMAGE:";
            UIManager.Instance.UpgradedStatText2.text = "FIRE RATE:";
            UIManager.Instance.UpgradedStatText3.text = "AOE RANGE";
            UIManager.Instance.UpgradedStatText4.text = "IMPACT";


            // Set values
            UIManager.Instance.StatValue1.text = baseDamage.ToString();
            UIManager.Instance.StatValue2.text = baseFireRate.ToString();
            UIManager.Instance.StatValue3.text = baseAoeRadius.ToString();
            UIManager.Instance.StatValue4.text = baseAoeBlastForce.ToString();
            UIManager.Instance.UpgradedStatValue1.text = upgradedDamage.ToString();
            UIManager.Instance.UpgradedStatValue2.text = upgradedFireRate.ToString();
            UIManager.Instance.UpgradedStatValue3.text = upgradedAoeRange.ToString();
            UIManager.Instance.UpgradedStatValue4.text = upgradedAoeBlastForce.ToString();

            UIManager.Instance.SellPriceValue.text = sellPrice.ToString();
            UIManager.Instance.UpgradeCostValue.text = upgradeCost.ToString();
        }
        if (WizardTower == true) 
        {
            // Set texts
            UIManager.Instance.BuildingName1.text = "WIZARD TOWER";
            UIManager.Instance.BuildingName2.text = "WIZARD TOWER";
            UIManager.Instance.StatText1.text = "DAMAGE OVER TIME:";
            UIManager.Instance.StatText2.text = "EFFECT DURATION:";
            UIManager.Instance.StatText3.text = "SPECIAL EFFECT:";
            UIManager.Instance.UpgradedStatText1.text = "DAMAGE OVER TIME:";
            UIManager.Instance.UpgradedStatText2.text = "EFFECT DURATION:";
            UIManager.Instance.UpgradedStatText3.text = "SPECIAL EFFECT:";

            // Set values
            UIManager.Instance.StatValue1.text = baseDot.ToString();
            UIManager.Instance.StatValue2.text = baseEffectDuration.ToString();
            if (fireType) { UIManager.Instance.StatValue3.text = "DEALS BURNING"; 
                UIManager.Instance.UpgradedStatValue3.text = "DEALS BURNING"; 
            }
            if (lightningType) { UIManager.Instance.StatValue3.text = "SLOWS ENEMIES";
                UIManager.Instance.StatValue3.text = "SLOWS ENEMIES";
            }
            if (iceType) { UIManager.Instance.StatValue3.text = "STUNS AND DAMAGE ENEMIES IN ZONE";
                UIManager.Instance.StatValue3.text = "STUNS AND DAMAGE ENEMIES IN ZONE";
            }
            UIManager.Instance.UpgradedStatValue1.text = upgradedFxDuration.ToString();
            UIManager.Instance.UpgradedStatValue2.text = upgradedDot.ToString();

            UIManager.Instance.SellPriceValue.text = sellPrice.ToString();
            UIManager.Instance.UpgradeCostValue.text = upgradeCost.ToString();
        }

    }

}
