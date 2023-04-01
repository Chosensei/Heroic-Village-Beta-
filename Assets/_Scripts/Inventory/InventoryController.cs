using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }
    public ConsumableController consumableController;
    public PlayerWeaponController pwc;
    [Tooltip("Set in inspector")]
    public InventoryUIDetails inventoryDetailsPanel;
    public List<Item> playerItems = new List<Item>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        //inventoryDetailsPanel = GameObject.Find("Inventory_Details").GetComponent<InventoryUIDetails>();

    }
    private void Start()
    {
        pwc = GetComponent<PlayerWeaponController>();
        consumableController = GetComponent<ConsumableController>();
        // Testing
        GiveItem("sword");
        GiveItem("potion_log");
        GiveItem("bow");

    }
    //    potionlog = new Item(new List<BaseStat>(), "Potion Info", "Drink this to log something cool!", "Drink", "LogPotion", false);
    //    List<BaseStat> swordStats = new List<BaseStat>();
    //    swordStats.Add(new BaseStat(6, "Power", "Your sword power level."));
    //    sword = new Item(swordStats, "Sword");

    //    List<BaseStat> stickStats = new List<BaseStat>();
    //    stickStats.Add(new BaseStat(3, "Power", "Your stick power level."));
    //    stick = new Item(stickStats, "Stick");

    //    List<BaseStat> bowStats = new List<BaseStat>();
    //    bowStats.Add(new BaseStat(2, "Power", "Your bow power level."));
    //    bow = new Item(bowStats, "Bow");
    
    public void GiveItem(string itemSlug)
    {
        Item item = ItemDatabase.Instance.GetItem(itemSlug); 
        // Add the correct item from db to player item
        playerItems.Add(ItemDatabase.Instance.GetItem(itemSlug));
        // ensure that player item has item in it
        UIEventHandler.ItemAddedToInventory(item);
        Debug.Log(playerItems.Count + " items in inventory. Added: " + itemSlug);
    }
    public void GiveItem(Item item)
    {
        playerItems.Add(item);
        UIEventHandler.ItemAddedToInventory(item);
    }
    public void GiveItem(List<Item> items)
    {
        playerItems.AddRange(items);
        UIEventHandler.ItemAddedToInventory(items);
    }
    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }
    public void EquipItem(Item itemToEquip)
    {
        pwc.EquipWeapon(itemToEquip); 
    }
    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.ConsumeItem(itemToConsume);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    pwc.EquipWeapon(sword);
        //    consumableController.ConsumeItem(potionlog);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    pwc.EquipWeapon(stick);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    pwc.EquipWeapon(bow);
        //}
    }
}
