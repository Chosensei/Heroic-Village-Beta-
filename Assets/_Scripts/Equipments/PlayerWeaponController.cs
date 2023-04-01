using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject WeaponPivot;   // aka the player's hand
    public GameObject EquippedWeapon { get; set; }
    private Transform spawnProjectile; 

    private CharacterStats characterStats;
    private IWeapon iEquippedWeapon;

    [System.Obsolete]
    private void Start()
    {
        spawnProjectile = transform.FindChild("ProjectileSpawn"); 
        characterStats = GetComponent<Player>().characterStats; // characterstats is on the component of player 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PerformWeaponAttack();
        }
    }
    public void EquipWeapon(Item itemToEquip)
    {
        if (EquippedWeapon != null)
        {
            characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(WeaponPivot.transform.GetChild(0).gameObject);     // Player only have one weapon in his hand at all times so this is fine
        }
        EquippedWeapon = Instantiate(Resources.Load<GameObject>("Weapons/" + itemToEquip.ObjectSlug),
            WeaponPivot.transform.position, WeaponPivot.transform.rotation);

        iEquippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        // Temporary way of setting new weapon stats onto player
        iEquippedWeapon.Stats = itemToEquip.Stats;
        // check if weapon has a projectile
        if (EquippedWeapon.GetComponent<IProjectileWeapon>() != null)
        {
            EquippedWeapon.GetComponent<IProjectileWeapon>().ProjectileSpawn = spawnProjectile;
        }
        // setting player hand as the new weapon parent
        EquippedWeapon.transform.SetParent(WeaponPivot.transform);
        // setting weapon stats to character stats
        characterStats.AddStatBonus(itemToEquip.Stats);
        // testing
        Debug.Log(iEquippedWeapon.Stats[0].BaseValue + " " + iEquippedWeapon.Stats[0].StatName + " " + iEquippedWeapon.Stats[0].StatDescription);
    }
    public void PerformWeaponAttack()
    {
        // Gets called whenever player activates a attack key
        iEquippedWeapon.PerformAttack(); 
    }
}
