using UnityEngine;

public class TowerUpgradeTest : MonoBehaviour
{
    private const int MaxTowerLevel = 5;
    private const float BaseDamage = 10f;
    private const float BaseFireRate = 5f;
    private const float BaseFiringRange = 100f;

    private TowerUpgrade towerUpgrade;
    private GameObject[] towerLevelUpPrefabs;
    private int counter = 0;
    private int currentTowerLevel = 0;

    public event System.Action<float, float, float> TowerUpgraded;

    private void Start()
    {
        towerUpgrade = GetComponent<TowerUpgrade>();
        towerLevelUpPrefabs = new GameObject[MaxTowerLevel];

        for (int i = 0; i < MaxTowerLevel; i++)
        {
            towerLevelUpPrefabs[i] = transform.GetChild(i).gameObject;
            towerLevelUpPrefabs[i].SetActive(false);
        }

        towerLevelUpPrefabs[0].SetActive(true);
    }

    public void UpgradeTowerPrefab()
    {
        if (counter < towerLevelUpPrefabs.Length - 1)
        {
            counter++;
            currentTowerLevel++;
            SwitchObject(counter);
            UpgradeTowerStats(currentTowerLevel);
        }
    }

    void SwitchObject(int lvl)
    {
        for (int i = 0; i < MaxTowerLevel; i++)
        {
            if (i == lvl)
            {
                towerLevelUpPrefabs[i].SetActive(true);
            }
            else
            {
                towerLevelUpPrefabs[i].SetActive(false);
            }
        }
    }

    private void UpgradeTowerStats(int currentLevel)
    {
        if (currentLevel > towerUpgrade.upgrades.Length)
        {
            Debug.LogWarning("No upgrade available for current level");
            return;
        }

        var upgrade = towerUpgrade.upgrades[currentLevel - 1];

        float damage = BaseDamage + upgrade.damageBoost;
        float fireRate = BaseFireRate - upgrade.fireRateBoost;
        float firingRange = BaseFiringRange + upgrade.firingRangeBoost;

        TowerUpgraded?.Invoke(damage, fireRate, firingRange);
    }
}