using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WallUpgradeManager : MonoBehaviour
{
    public CageWall [] walls;
    public CageWall currentWall;
    public Button[] upgradeButtons;
    public TMP_Text upgradePrice;
    public TMP_Text[] wallUpgradeTexts;

    // Upgrade cost parameters
    public int baseUpgradeCost = 500;
    [Range(0.1f, 1f)]
    public float upgradePercentIncrease = 0.1f;

    void Start()
    {
        // Set up upgrade button click events
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int wallIndex = i;
            upgradeButtons[i].onClick.AddListener(delegate { UpgradeWall(wallIndex); });
        }
    }


    void Update()
    {
        UpdateWallInfo();

        // Disable upgrade button if wall is at max upgrade level
        for (int i = 0; i < walls.Length; i++)
        {
            CageWall wall = walls[i];
            if (wall.UpgradeLevel == wall.MaxLevel)
            {
                upgradeButtons[i].interactable = false;
            }
            else
            {
                upgradeButtons[i].interactable = true;
            }
        }
    }

    // Function to upgrade a wall
    void UpgradeWall(int wallIndex)
    {
        CageWall wall = walls[wallIndex];
        // Get the current upgrade level of the wall
        int currentUpgradeLevel = wall.GetComponent<CageWall>().UpgradeLevel;
        int maxUpgradeLevel = wall.GetComponent<CageWall>().MaxLevel;

        // Calculate the cost of the upgrade
        int upgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1 + upgradePercentIncrease, currentUpgradeLevel));

        // Check if the player has enough gold to upgrade the wall
        if (GMDebug.Instance.MoneyInBank >= upgradeCost && currentUpgradeLevel < maxUpgradeLevel)
        {
            // Subtract the upgrade cost from the player's gold
            GMDebug.Instance.MoneyInBank -= upgradeCost;
            // Update bank account
            UIManager.Instance.moneyText.text = GMDebug.Instance.MoneyInBank.ToString(); 
            // Increase the wall's max health by 10%
            wall.GetComponent<CageWall>().MaxWallHP *= 1.1f;

            // Increase the wall's upgrade level
            wall.GetComponent<CageWall>().UpgradeLevel++;

            // Update wall info text
            UpdateWallInfo();
        }

    }
    // Update the wall health and upgrade level and cost UI elements
    void UpdateWallInfo()
    {
        // WALL 1
        CageWall wall1 = walls[0].GetComponent<CageWall>();
        int wall1UpgradeLevel = wall1.UpgradeLevel;
        float wall1MaxHP = wall1.MaxWallHP;
        float wall1NewMaxHP = Mathf.RoundToInt(wall1MaxHP * (1 + upgradePercentIncrease));
        int wall1UpgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1 + upgradePercentIncrease, wall1UpgradeLevel));
        
        // Update texts UI
        wallUpgradeTexts[0].text = $"{wall1UpgradeLevel}";
        wallUpgradeTexts[1].text = $"{wall1MaxHP:F0}";
        wallUpgradeTexts[2].text = $"{wall1NewMaxHP:F0}";
        wallUpgradeTexts[9].text = $"{wall1UpgradeCost}";
        
        // Display when reach max level
        if (wall1.UpgradeLevel == wall1.MaxLevel)
        {
            wallUpgradeTexts[0].text = "MAX";
            wallUpgradeTexts[2].text = "-";
            wallUpgradeTexts[9].text = "-";
        }

        // WALL 2
        CageWall wall2 = walls[1].GetComponent<CageWall>();
        int wall2UpgradeLevel = wall2.UpgradeLevel;
        float wall2MaxHP = wall2.MaxWallHP;
        float wall2NewMaxHP = Mathf.RoundToInt(wall2MaxHP * (1 + upgradePercentIncrease));
        int wall2UpgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1 + upgradePercentIncrease, wall2UpgradeLevel));
        
        // Update texts UI
        wallUpgradeTexts[3].text = $"{wall2UpgradeLevel}";
        wallUpgradeTexts[4].text = $"{wall2MaxHP:F0}";
        wallUpgradeTexts[5].text = $"{wall2NewMaxHP:F0}";
        wallUpgradeTexts[10].text = $"{wall2UpgradeCost}";

        // Display when reach max level
        if (wall2.UpgradeLevel == wall2.MaxLevel)
        {
            wallUpgradeTexts[3].text = "MAX";
            wallUpgradeTexts[5].text = "-";
            wallUpgradeTexts[10].text = "-";
        }

        // WALL 3
        CageWall wall3 = walls[2].GetComponent<CageWall>();
        int wall3UpgradeLevel = wall3.UpgradeLevel;
        float wall3MaxHP = wall3.MaxWallHP;
        float wall3NewMaxHP = Mathf.RoundToInt(wall3MaxHP * (1 + upgradePercentIncrease));

        // Set a different starting upgrade price for wall 3
        int wall3UpgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1 + upgradePercentIncrease, wall3UpgradeLevel)) * 3;
        
        // Update texts UI
        wallUpgradeTexts[6].text = $"{wall3UpgradeLevel}";
        wallUpgradeTexts[7].text = $"{wall3MaxHP:F0}";
        wallUpgradeTexts[8].text = $"{wall3NewMaxHP:F0}";
        wallUpgradeTexts[11].text = $"{wall3UpgradeCost}";

        // Display when reach max level
        if (wall3.UpgradeLevel == wall3.MaxLevel)
        {
            wallUpgradeTexts[6].text = "MAX";
            wallUpgradeTexts[8].text = "-";
            wallUpgradeTexts[11].text = "-";
        }
    }
}
