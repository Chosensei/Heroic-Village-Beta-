using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Tower Upgrade", order = 1)]
public class TowerUpgrade : ScriptableObject
{
    [System.Serializable]
    public struct Upgrade
    {
        [Header("General Buff")]
        public float damageBoost;
        public float fireRateBoost;
        public float firingRangeBoost;
        [Header("Catapult")]
        public float aoeRangeBoost;
        public float aoeBlastBoost;
        public float animSpeedBoost;
        [Header("Wizard")]
        public float dotBoost;
        public float slowMvtBoost;
        public float stunDurationBoost;
        [Header("Village House")]
        public int incomeBoost;
        [Header("Others")]
        public int saleAmount;
        public int upgradeAmount; 

    }

    public Upgrade[] upgrades;
}
