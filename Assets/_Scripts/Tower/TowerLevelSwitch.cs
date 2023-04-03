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
    public float baseFiringRange = 100f;
    public int baseSellingPrice = 0;
    public int baseUpgradePrice = 0; 
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
    public bool VillageHouse = false; 
    public bool ArcherTower = false;
    public bool CatapultTower = false; 
    public bool WizardTower = false;  
    // Can transfer these into another script responsible for handling tower upgrades 
    public int counter = 0;
    public int currentTowerLevel = 1;
    public int maxTowerLevel = 5;

    private int upgradedIncomeAmount; 
    private float upgradedDamageAmount;
    private float upgradedFireRateAmount;
    private float upgradedFiringRangeAmount;
    private void Start()
    {
        //towerUpgrade = GetComponent<TowerUpgrade>();
        upgradedIncomeAmount = baseIncome + towerData.upgrades[currentTowerLevel - 1].incomeBoost;
        upgradedDamageAmount = baseDamage + towerData.upgrades[currentTowerLevel - 1].damageBoost;
        upgradedFireRateAmount = baseFireRate - towerData.upgrades[currentTowerLevel - 1].fireRateBoost;
        upgradedFiringRangeAmount = baseFiringRange + towerData.upgrades[currentTowerLevel - 1].firingRangeBoost;
        baseSellingPrice = towerData.upgrades[currentTowerLevel - 1].saleAmount;
        baseUpgradePrice = towerData.upgrades[currentTowerLevel - 1].upgradeAmount; 

    }
    private void OnMouseDown()
    {
        UIManager.Instance.ShowUpgradeMenu(this); 
        //BuildingShopManager.Instance.buildingUpgradeUIMenu.SetActive(true);
        UIManager.Instance.currentSelectedBuilding = this.gameObject;

        DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseIncome, upgradedIncomeAmount, baseDamage, baseFireRate, baseFiringRange, upgradedDamageAmount, upgradedFireRateAmount, upgradedFiringRangeAmount);
        //if (ArcherTower)
        //{
        //    print("current tower level: " + currentTowerLevel + "\n");
        //    DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseDamage, baseFireRate, baseFiringRange, baseDamage + towerData.upgrades[currentTowerLevel - 1].damageBoost, baseFireRate - towerData.upgrades[currentTowerLevel - 1].fireRateBoost, baseFiringRange + towerData.upgrades[currentTowerLevel - 1].firingRangeBoost);
        //}

    }
    public void UpgradeTestMessage()
    {
        print("initiating Upgrade ");
    }
    public void UpgradeTowerPrefab()
    {
        // Update Money in bank
        GameManager.Instance.MoneyInBank -= baseUpgradePrice;
        UIManager.Instance.moneyText.text = GameManager.Instance.MoneyInBank.ToString();

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
        GameManager.Instance.MoneyInBank += baseSellingPrice;
        UIManager.Instance.moneyText.text = GameManager.Instance.MoneyInBank.ToString(); 
        Destroy(this.gameObject);
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
        int incomeboost = towerData.upgrades[currentLevel - 1].incomeBoost;
        float damageBoost = towerData.upgrades[currentLevel - 1].damageBoost;
        float fireRateBoost = towerData.upgrades[currentLevel - 1].fireRateBoost;
        float rangeBoost = towerData.upgrades[currentLevel - 1].firingRangeBoost;
        //float aoeRangeBoost = towerData.upgrades[currentLevel - 1].aoeRangeBoost;
        //float aoeBlastBoost = towerData.upgrades[currentLevel - 1].aoeBlastBoost;
        //int incomeBoost = towerData.upgrades[currentLevel - 1].incomeBoost;

        int resellValue = towerData.upgrades[currentLevel - 1].saleAmount;
        int upgradeValue = towerData.upgrades[currentLevel - 1].upgradeAmount;
        baseSellingPrice = resellValue;
        baseUpgradePrice = upgradeValue;

        //baseIncome *= incomeBoost; 
        //baseAoeRadius += aoeRangeBoost;
        //baseAoeBlastForce *= aoeBlastBoost;
        //baseDamage += damageBoost;
        //baseFireRate -= fireRateBoost;
        //baseFiringRange += rangeBoost;
        upgradedIncomeAmount = baseIncome + incomeboost; 
        upgradedDamageAmount = baseDamage + damageBoost;
        upgradedFireRateAmount = baseFireRate - fireRateBoost;
        upgradedFiringRangeAmount = baseFiringRange + rangeBoost;

        //int nextLevel = currentTowerLevel++;
        DisplayCurrentTowerStats(currentTowerLevel, currentTowerLevel + 1, baseSellingPrice, baseUpgradePrice, baseIncome, upgradedIncomeAmount, baseDamage, baseFireRate, baseFiringRange, upgradedDamageAmount, upgradedFireRateAmount, upgradedFiringRangeAmount);
        baseIncome = upgradedIncomeAmount; 
        baseDamage = upgradedDamageAmount;
        baseFireRate = upgradedFireRateAmount;
        baseFiringRange = upgradedFiringRangeAmount;
        
    }
    public void DisplayCurrentTowerStats(int currentTowerLevel, int nextLevel, int sellPrice, int upgradeCost, int income, int upgradedIncome, float baseDamage, float baseFireRate, float baseFiringRange, float upgradedDamage, float upgradedFireRate, float upgradedFiringRange)
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
            //BuildingShopManager.Instance.BuildingName1.text = "CATAPULT TOWER";
            //BuildingShopManager.Instance.BuildingName2.text = "CATAPULT TOWER";
            //BuildingShopManager.Instance.StatText1.text = "DAMAGE:";
            //BuildingShopManager.Instance.StatText2.text = "FIRE RATE";
            //BuildingShopManager.Instance.StatText3.text = "AOE RANGE";
            //BuildingShopManager.Instance.StatText4.text = "IMPACT";
        }
        if (WizardTower == true) 
        {
            //BuildingShopManager.Instance.BuildingName1.text = "WIZARD TOWER";
            //BuildingShopManager.Instance.BuildingName2.text = "WIZARD TOWER";
            //BuildingShopManager.Instance.StatText1.text = "";
            //BuildingShopManager.Instance.StatText2.text = "";
            //BuildingShopManager.Instance.StatText3.text = "";
            //BuildingShopManager.Instance.StatText4.text = "";
        }


        // Don't forget to account for village house too! It doesn't have damage/ firerate / range, except income value
    }

}
