using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSmithShop : MonoBehaviour
{
    [SerializeField] private int baseUpgradePrice = 10;
    [SerializeField] private int upgradePriceMultiplier = 2;
    [SerializeField] private int maxUpgrades = 10;
    [SerializeField] private TMP_Text currentPowerText;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text upgradeDamagePercentAmt;

    [SerializeField] private Button buyButton;
    [SerializeField] private NPC npc; 
    private int currentUpgradeLevel = 1;
    private int currentUpgradePrice;
    private int currentUpgradePower = 0;
    private float powerPercentageInc = 0.1f;

    private GameObject sword;

    private void Start()
    {
        // Find the player object in the scene
        sword = GameObject.FindGameObjectWithTag("Sword");
        if (sword == null)
        {
            Debug.LogError("Could not find player object.");
            buyButton.interactable = false;
        }

        // Calculate the initial upgrade price
        currentUpgradePrice = baseUpgradePrice;
        UpdatePowerText();
        UpdateUpgradePriceText();
        UpdateLevelText();
        UpdateCurrentUpgradePowerPercentage();
    }

    public void UpgradeAttackPower()
    {
        if (sword != null && currentUpgradeLevel < maxUpgrades)
        {
            // Check if the player has enough gold to buy the upgrade
            if (GMDebug.Instance.MoneyInBank >= currentUpgradePrice)
            {
                // Subtract the gold from the player and upgrade their attack power
                GMDebug.Instance.MoneyInBank -= currentUpgradePrice;
                currentUpgradeLevel++;
                currentUpgradePower = Mathf.CeilToInt(sword.GetComponent<PlayerWeapon>().swordDamage * powerPercentageInc);
                sword.GetComponent<PlayerWeapon>().swordDamage += currentUpgradePower;
                sword.GetComponent<PlayerWeapon>().IncreaseAttackPower(sword.GetComponent<PlayerWeapon>().swordDamage);

                // Calculate the new upgrade price
                currentUpgradePrice = baseUpgradePrice * (int)Mathf.Pow(upgradePriceMultiplier, currentUpgradeLevel);
                UpdateUpgradePriceText();
                UpdateCurrentUpgradePowerPercentage();
                UpdateLevelText();
                UpdatePowerText();

                npc.GetComponent<Animator>().SetTrigger("AfterBuying");
                npc.GetComponent<Animator>().SetTrigger("Idle");
                DialogueManager.Instance.ShowThankYouMessage(); 
            }
        }
    }
    private void UpdateLevelText()
    {
        currentLevelText.text = currentUpgradeLevel.ToString(); 
    }

    private void UpdatePowerText()
    {
        currentPowerText.text = sword.GetComponent<PlayerWeapon>().swordDamage.ToString();
        Debug.Log($"Current damage: {sword.GetComponent<PlayerWeapon>().swordDamage}");
    }

    private void UpdateUpgradePriceText()
    {
        upgradePriceText.text = currentUpgradePrice.ToString();
    }
    private void UpdateCurrentUpgradePowerPercentage()
    {
        switch (currentUpgradeLevel)
        {
            case 1:
                upgradeDamagePercentAmt.text = "10 %";
                break;
            case 2:
                upgradeDamagePercentAmt.text = "20 %";
                break;
            case 3:
                upgradeDamagePercentAmt.text = "15 %";
                break;
            case 4:
                upgradeDamagePercentAmt.text = "15 %";
                break;
            case 5:
                upgradeDamagePercentAmt.text = "15 %";
                break;
            case 6:
                upgradeDamagePercentAmt.text = "10 %";
                break;
            case 7:
                upgradeDamagePercentAmt.text = "10 %";
                break;
            case 8:
                upgradeDamagePercentAmt.text = "15 %";
                break;
            case 9:
                upgradeDamagePercentAmt.text = "15 %";
                break;
            case 10:
                upgradeDamagePercentAmt.text = "10 %";
                break;
        }
    }

}
