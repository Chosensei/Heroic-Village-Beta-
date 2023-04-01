using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;
    public InventoryUIItems itemContainer { get; set; }
    List<InventoryUIItems> itemUIList = new List<InventoryUIItems>();
    bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }

    void Start()
    {
        // double check this!! 
        itemContainer = Resources.Load<InventoryUIItems>("UI/Item_Container");
        // subscribing the event
        UIEventHandler.OnItemAddedToInventory += ItemAdded; 
        // Set panel to inactive at the start
        inventoryPanel.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // when is true it becomes false & vice versa 
            menuIsActive = !menuIsActive; 
            // Inventory panel open / closes when I is pressed
            inventoryPanel.gameObject.SetActive(menuIsActive);
        }
    }
    // Notify and set changes on the Ui when new items has been added / used 
    public void ItemAdded(Item item)
    {
        // create a new item container when new items are added! 
        InventoryUIItems emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        itemUIList.Add(emptyItem);
        // set new container as child of scrollview parent object
        emptyItem.transform.SetParent(scrollViewContent);
    }
}
