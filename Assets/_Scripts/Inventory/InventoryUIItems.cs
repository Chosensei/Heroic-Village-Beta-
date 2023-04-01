using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItems : MonoBehaviour
{
    public Item item;
    public Text itemText; 
    public Image itemImage;
    public void SetItem(Item item)
    {
        this.item = item;
        SetupItemValues();
    }

    private void SetupItemValues()
    {
        //itemText.text = item.ItemName;
        //itemImage.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
        //transform.FindChild("Item_Name").GetComponent<Text>().text = item.ItemName; 
    }
    public void OnSelectItemButton()
    {
        InventoryController.Instance.SetItemDetails(item, GetComponent<Button>());
    }
}
