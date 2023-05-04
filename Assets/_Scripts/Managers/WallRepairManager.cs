using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WallRepairManager : MonoBehaviour
{
    public CageWall[] walls;
    public CageWall currentWall;
    public float repairPercentage = 0.1f;
    public float baseRepairCost = 100f;
    public float baseUpgradeCost = 500f;
    public Button repairButton1, repairButton2, repairButton3; 
    public Button upgradeButton1, upgradeButton2, upgradeButton3;
    public Button confirmRepair;
    public Button confirmUpgrade; 
    public GameObject repairMenu;
    public GameObject upgradeMenu; 
    public Slider repairSlider;
    public TMP_Text w1CurHP, w2CurHP, w3CurHP;
    public TMP_Text w1MaxHP, w2MaxHP, w3MaxHP;
    public TMP_Text repairPrice; 
    public TMP_Text repairPercentAmount;
    //private int upgradeLevel = 0;
    private int daysSinceInflation = 0;
    private int repairCost;
    private int goldPerPercent = 10;
    private float inflationMultiplier = 1.1f;
    private float repairPercent; 

    void Start()
    {
        // Set up button click events
        repairButton1.onClick.AddListener(delegate { OpenRepairMenu(walls[0]); });
        repairButton2.onClick.AddListener(delegate { OpenRepairMenu(walls[1]); });
        repairButton3.onClick.AddListener(delegate { OpenRepairMenu(walls[2]); });

        //upgradeButton1.onClick.AddListener(delegate { OpenUpgradesMenu(wallOne); });
        //upgradeButton2.onClick.AddListener(delegate { OpenUpgradesMenu(wallTwo); });
        //upgradeButton3.onClick.AddListener(delegate { OpenUpgradesMenu(wallThree); });
        
        confirmRepair.onClick.AddListener(delegate { OnConfirmButtonClicked(); });
        //confirmRepair.onClick.AddListener(delegate { RepairWall(); });

        // Set up slider event
        repairSlider.onValueChanged.AddListener(delegate { OnRepairSliderChanged(currentWall); });
        //repairSlider.onValueChanged.AddListener(delegate { UpdateSliderPercentageAndPrice(); });
        
        // Hide repair and upgrades menu initially
        //repairMenu.SetActive(false);
        //upgradeMenu.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        // Set the walls current HP and Max HP stats for each wall
        w1CurHP.text = walls[0].CurrentWallHP.ToString();
        w2CurHP.text = walls[1].CurrentWallHP.ToString();
        w3CurHP.text = walls[2].CurrentWallHP.ToString();

        w1MaxHP.text = $"/  {walls[0].MaxWallHP.ToString()}";
        w2MaxHP.text = $"/  {walls[1].MaxWallHP.ToString()}";
        w3MaxHP.text = $"/  {walls[2].MaxWallHP.ToString()}";

        if (currentWall != null)
        {
            // Disable repair button if wall is at full health
            if (currentWall == walls[0])
            {
                if (!CanRepairDamage()) repairButton1.image.color = Color.gray;
                else repairButton1.image.color = Color.white;
            }
            else if (currentWall == walls[1])
            {
                if (!CanRepairDamage()) repairButton2.image.color = Color.gray;
                else repairButton1.image.color = Color.white;
            }
            else if (currentWall == walls[2])
            {
                if (!CanRepairDamage()) repairButton3.image.color = Color.gray;
                else repairButton1.image.color = Color.white;
            }


            // Disable upgrade button if wall is at max upgrade level
            if (currentWall.UpgradeLevel == currentWall.MaxLevel)
            {
                upgradeButton1.interactable = false;
                upgradeButton2.interactable = false;
                upgradeButton3.interactable = false;
            }
            else
            {
                upgradeButton1.interactable = true;
                upgradeButton2.interactable = true;
                upgradeButton3.interactable = true;
            }
        }
        if (!CanAffordRepair() && !CanRepairDamage())
        {
            confirmRepair.interactable = false;
        }
        

        // Check for inflation after a certain number of days
        daysSinceInflation++;

        // If more than 5 days have passed, increase inflation multiplier
        if (daysSinceInflation >= 5)
        {
            inflationMultiplier *= 1.1f;

            // Increase repair and upgrade costs by inflation multiplier
            baseRepairCost *= inflationMultiplier;
            baseUpgradeCost *= inflationMultiplier;

        }
    }

    // Function to open repair menu
    public void OpenRepairMenu(CageWall cw)
    {
        currentWall = cw;

        // Set slider value to 0 initially
        repairSlider.value = 0f;
    }

    public void OnRepairSliderChanged(CageWall cw)
    {
        UpdateRepairCost(cw);
        UpdateConfirmButton();
    }
    // Function to repair wall
    private void UpdateRepairCost(CageWall cw)
    {
        currentWall = cw;

        // Update the slider value on the repair amount text, reflected as %
        repairPercent = repairSlider.value / repairSlider.maxValue * 100f;
        repairPercentAmount.text = string.Format("{0}%", repairPercent.ToString("F0"));
        float wallDamagePercent = (currentWall.CurrentWallHP / currentWall.MaxWallHP) * 100f;

        // Calculate the maximum percentage of damage that can be repaired
        float maxRepairPercent = (100f - wallDamagePercent);
        if (repairPercent > maxRepairPercent)
        {
            repairPercent = maxRepairPercent;
            repairSlider.value = repairPercent / 100f * repairSlider.maxValue;
        }

        repairCost = Mathf.RoundToInt(repairPercent * goldPerPercent);
        repairPrice.text = repairCost.ToString("F0");
    }

    private void UpdateConfirmButton()
    {
        confirmRepair.interactable = CanAffordRepair();
    }
    private bool CanAffordRepair()
    {
        return repairCost <= GMDebug.Instance.MoneyInBank;
    }
    private bool CanRepairDamage()
    {
        return currentWall.CurrentWallHP != currentWall.MaxWallHP; 
    }
    private void PayForRepair()
    {
        GMDebug.Instance.MoneyInBank -= repairCost;
        UIManager.Instance.moneyText.text = GMDebug.Instance.MoneyInBank.ToString();
    }
    private void ApplyRepair()
    {
        //int repairPercent = (int)repairSlider.value;
        int repairAmount = Mathf.RoundToInt(currentWall.MaxWallHP * repairPercent / 100);
        currentWall.CurrentWallHP = Mathf.Min(currentWall.CurrentWallHP + repairAmount, currentWall.MaxWallHP);
        int wallIndex = Array.IndexOf(walls, currentWall);
        // TODO: Update the wall's health UI element
        UIManager.Instance.UpdateWallHP(wallIndex, currentWall.CurrentWallHP, currentWall.MaxWallHP);
        print("current wall HP = " + currentWall.CurrentWallHP);

    }
    // Function to close repair menu
    public void CloseRepairMenu()
    {
        // Hide repair menu
        repairMenu.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        if (CanAffordRepair() && CanRepairDamage())
        {
            PayForRepair();
            ApplyRepair();
            repairSlider.value = 0f;    // reset slider position to 0

            // Revive wall if destroyed previously as long as there's more than 1 hp
            if (currentWall.isDestroyed)
            {
                if (currentWall.CurrentWallHP > 0)
                {
                    if (!currentWall.gameObject.activeSelf)
                    {
                        currentWall.gameObject.SetActive(true);
                    }
                    currentWall.isDestroyed = false;
                }
            }
        }
        else
        {
            // Disable button indicating the player cannot afford the repair
            confirmRepair.interactable = false;
        }
    }
    
}
