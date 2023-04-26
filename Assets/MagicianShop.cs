using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Abilities.Effects;
using RPG.Abilities;

public class MagicianShop : MonoBehaviour
{
    public Button magicBuffButton; 
    //Tier 1
    public Button FireT1Button;
    public Button EarthT1Button;
    public Button ThunderT1Button;
    //Tier 2
    public Button FireT2Button;
    public Button EarthT2Button;
    public Button ThunderT2Button;
    //Tier 3
    public Button FireT3Button;

    public TMP_Text CurrentSelection;
    public TMP_Text CurrentUpgradeLevel;

    public TMP_Text DamageValue;
    public TMP_Text ManaCostValue;
    public TMP_Text CooldownValue;

    public bool Tier1Fire = false;
    public bool Tier1Earth = false;
    public bool Tier1Thunder = false;

    [SerializeField] private int baseUpgradePrice = 10;

    private int currentUpgradePrice;
    private int fireballUpgradeLevel = 1;
    private int stoneshotUpgradeLevel = 1;
    private int shockboltUpgradeLevel = 1;

    // maximum number of upgrades
    public int maxUpgrades = 5; 
    public int upgradePriceIncrement = 50;

    // Using references from Ability script
    public Ability abilityTier1; 
    public float cooldownTime;
    public float manaCost;
    public int healthChange;


    void Start()
    {
        SetupSelectionButtons();
        // Calculate the initial upgrade price
        currentUpgradePrice = baseUpgradePrice;
    }

    void Update()
    {

    }
    private void BuffMagic()
    {
        // Update Money in bank
        GMDebug.Instance.MoneyInBank -= currentUpgradePrice;
    }
    private void SetupSelectionButtons()
    {
        magicBuffButton.onClick.AddListener(() =>
        { 

        });

        FireT1Button.onClick.AddListener(() =>
        {
            Tier1Fire = true;
            fireballUpgradeLevel++;
        });

        EarthT1Button.onClick.AddListener(() =>
        {
            Tier1Earth = true;
            stoneshotUpgradeLevel++;
        }); 
        
        ThunderT1Button.onClick.AddListener(() =>
        {
            Tier1Thunder = true;
            shockboltUpgradeLevel++;
        });
    }
    public void DisplayCurrentMagicStats(int curLevel, int damage, int manaCost, int cooldown)
    {
        if (Tier1Fire)
        {
            // Set texts
            CurrentSelection.text = "Fireball";
            CurrentUpgradeLevel.text = curLevel.ToString();
            DamageValue.text = damage.ToString();
            ManaCostValue.text = manaCost.ToString();
            CooldownValue.text = cooldown.ToString(); 
}
        if (Tier1Earth)
        {
            // Set texts
            CurrentSelection.text = "Stone shot";
            CurrentUpgradeLevel.text = curLevel.ToString();
            DamageValue.text = damage.ToString();
            ManaCostValue.text = manaCost.ToString();
            CooldownValue.text = cooldown.ToString();
        }
        if (Tier1Thunder)
        {
            // Set texts
            CurrentSelection.text = "Shock bolt";
            CurrentUpgradeLevel.text = curLevel.ToString();
            DamageValue.text = damage.ToString();
            ManaCostValue.text = manaCost.ToString();
            CooldownValue.text = cooldown.ToString();
        }
    }

}
