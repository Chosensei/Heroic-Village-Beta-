using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class WeaponSmithShop : MonoBehaviour
{
    [SerializeField] private int baseUpgradePrice = 10;
    [SerializeField] private int upgradePriceMultiplier = 2;
    [SerializeField] private int maxUpgrades = 10;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private Button buyButton;

    private int currentUpgradeLevel = 0;
    private int currentUpgradePrice;
    private int currentAttackPower = 10;

    private GameObject player;

    private void Start()
    {
        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Could not find player object.");
            buyButton.interactable = false;
        }

        // Calculate the initial upgrade price
        currentUpgradePrice = baseUpgradePrice;
        UpdateUpgradePriceText();
    }

    public void UpgradeAttackPower()
    {
        if (player != null && currentUpgradeLevel < maxUpgrades)
        {
            // Check if the player has enough gold to buy the upgrade
            if (GMDebug.Instance.MoneyInBank >= currentUpgradePrice)
            {
                // Subtract the gold from the player and upgrade their attack power
                GMDebug.Instance.MoneyInBank -= currentUpgradePrice;
                currentUpgradeLevel++;
                currentAttackPower += 1;
                player.GetComponent<PlayerWeapon>().IncreaseAttackPower(currentAttackPower);

                // Calculate the new upgrade price
                currentUpgradePrice = baseUpgradePrice * (int)Mathf.Pow(upgradePriceMultiplier, currentUpgradeLevel);
                UpdateUpgradePriceText();
            }
        }
    }

    private void UpdateUpgradePriceText()
    {
        upgradePriceText.text = "Upgrade Attack Power (" + currentUpgradePrice + " gold)";
    }
}
