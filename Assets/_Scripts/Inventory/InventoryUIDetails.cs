using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIDetails : MonoBehaviour
{
    Item item;
    Button selectedItemButton, itemInteractButton;
    Text itemNameText, itemDescriptionText, itemInteractButtonText;
    public Text statText; 
    private void Start()
    {
        itemNameText = transform.Find("Item_Name").GetComponent<Text>();
        itemDescriptionText = transform.Find("Item_Description").GetComponent<Text>();
        itemInteractButton = transform.GetComponentInChildren<Button>();
        itemInteractButtonText = itemInteractButton.GetComponentInChildren<Text>();
        gameObject.SetActive(false);    // deactivate item details panel 
    }
    public void SetItem(Item item, Button selectedButton)
    {

        gameObject.SetActive(true);     // activate item details panel 
        statText.text = "";             // empty out the text 
        if (item.Stats != null)         // Display item stat values 
        {
            foreach (BaseStat stat in item.Stats)
                statText.text += stat.StatName + ": " + stat.BaseValue + "\n"; 
        }
        itemInteractButton.onClick.RemoveAllListeners();
        this.item = item;
        selectedItemButton = selectedButton; 
        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.Description;
        itemInteractButtonText.text = item.ActionName;
        itemInteractButton.onClick.AddListener(OnItemInteract);
    }
    /// <summary>
    /// Checks for what kinds of items it is before doing its designated action
    /// </summary>
    public void OnItemInteract()
    {
        Debug.Log(item.ItemType.ToString());

        if (item.ItemType == Item.ItemTypes.Consumable)
        {
            InventoryController.Instance.ConsumeItem(item);
            Destroy(selectedItemButton.gameObject);
        }
        else if (item.ItemType == Item.ItemTypes.Weapon)
        {
            InventoryController.Instance.EquipItem(item);
            Destroy(selectedItemButton.gameObject);
        }
        item = null;
        gameObject.SetActive(false); // deactivate item details panel 
    }
}
