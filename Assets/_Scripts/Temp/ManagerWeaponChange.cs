using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerWeaponChange : MonoBehaviour
{
    public Transform WeaponPivot;           // aka think of it like the Player's Hand
    private WeaponManager _weaponManager;
    private int _indexPreviousWeapon; 

    void Start()
    {
        _weaponManager = GameObject.Find("Manager Weapon").GetComponent<WeaponManager>();

        // player default weapon 
        GameObject tempDefaultWeapon = _weaponManager.Weapons[0];
        Instantiate(tempDefaultWeapon, WeaponPivot);
        _indexPreviousWeapon = 0; 

    }
    // TESTING PURPOSES
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(0);
        }
    }
    public void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex != _indexPreviousWeapon)
        {
            Destroy(WeaponPivot.GetChild(0).gameObject);
            GameObject tempWeapon = _weaponManager.Weapons[weaponIndex];
            Instantiate(tempWeapon, WeaponPivot);   
            _indexPreviousWeapon = weaponIndex;
        }
    }
}
