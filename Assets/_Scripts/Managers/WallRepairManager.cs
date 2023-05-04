using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallRepairManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float repairPercentage = 0.1f;
    public float baseRepairCost = 100f;
    public float baseUpgradeCost = 500f;
    public int maxUpgradeLevel = 5;
    public Button repairButton;
    public Button upgradeButton;
    public GameObject repairMenu;
    public Slider repairSlider;

    private int upgradeLevel = 0;
    private int daysSinceInflation = 0;
    private float inflationMultiplier = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Set up button click events
        repairButton.onClick.AddListener(OpenRepairMenu);
        upgradeButton.onClick.AddListener(UpgradeWall);

        // Set up slider event
        repairSlider.onValueChanged.AddListener(delegate { RepairWall(); });

        // Hide repair menu initially
        repairMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Disable repair button if wall is at full health
        if (currentHealth == maxHealth)
        {
            repairButton.interactable = false;
        }
        else
        {
            repairButton.interactable = true;
        }

        // Disable upgrade button if wall is at max upgrade level
        if (upgradeLevel == maxUpgradeLevel)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }

        // Check for inflation after a certain number of days
        daysSinceInflation++;

        // assume 1 month = 30 days
        if (daysSinceInflation >= 10)   // >= day 10
        {
            // Increase repair and upgrade costs by inflation multiplier
            baseRepairCost *= inflationMultiplier;
            baseUpgradeCost *= inflationMultiplier;

            // Reset days since inflation
            //daysSinceInflation = 0;
        }
        if (daysSinceInflation >= 20) // >= day 20
        {

        }
    }

    // Function to open repair menu
    void OpenRepairMenu()
    {
        // Show repair menu
        repairMenu.SetActive(true);

        // Set slider max value to percentage of wall health that can be repaired
        repairSlider.maxValue = (maxHealth - currentHealth) / maxHealth / repairPercentage;

        // Set slider value to 0 initially
        repairSlider.value = 0f;
    }

    // Function to close repair menu
    void CloseRepairMenu()
    {
        // Hide repair menu
        repairMenu.SetActive(false);
    }

    // Function to repair wall
    void RepairWall()
    {
        // Calculate repair cost based on slider value and repair percentage
        int repairCost = Mathf.RoundToInt(baseRepairCost * repairSlider.value * maxHealth * repairPercentage);

        // Check if player has enough gold to pay for repair
        if (GMDebug.Instance.MoneyInBank >= repairCost)
        {
            // Deduct gold from player's account
            GMDebug.Instance.MoneyInBank -= repairCost;

            // Increase wall health by slider value times repair percentage
            currentHealth = Mathf.Min(currentHealth + maxHealth * repairPercentage * repairSlider.value, maxHealth);
        }
    }

    // Function to upgrade wall
    void UpgradeWall()
    {
        // Calculate upgrade cost with inflation
        int upgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(inflationMultiplier, upgradeLevel));

        // Check if player has enough gold to pay for upgrade
        if (GMDebug.Instance.MoneyInBank >= upgradeCost)
        {
            // Deduct gold from player's account
            GMDebug.Instance.MoneyInBank -= upgradeCost;
            // Increase max health of wall by 10%
            maxHealth *= 1.1f;

            // Increase current health of wall by 10%
            currentHealth *= 1.1f;

            // Increase upgrade level
            upgradeLevel++;
        }
    }
}
