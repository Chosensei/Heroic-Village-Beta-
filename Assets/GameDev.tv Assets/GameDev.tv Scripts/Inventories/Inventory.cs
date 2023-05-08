using System;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;

        // STATE
        //InventorySlot[] slots;
        // TEMP
        InventoryItem[] slots;
        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action inventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            //slots[i].item = item;
            //slots[i].number += number;
            slots[i] = item;

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot];
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public InventoryItem GetNumberInSlot(int slot)
        {
            return slots[slot];
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>

        public void RemoveFromSlot(int slot)
        {
            slots[slot] = null;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item)
        {
            if (slots[slot] != null)
            {
                return AddToFirstEmptySlot(item); ;
            }

            slots[slot] = item;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        // PRIVATE
        // This is where you can add item directly into your inventory via the item ID
        private void Awake()
        {
            slots = new InventoryItem[inventorySize];
            slots[0] = InventoryItem.GetFromID("106774e5-f53c-42df-b800-e39278eba7fb");
            slots[1] = InventoryItem.GetFromID("0f9487c6-d0d4-48e7-9d4a-d49dde96c5d4");
            slots[2] = InventoryItem.GetFromID("2ecaa6fb-d2ba-4ce5-9bf8-d01dbf7ecf8b");
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(InventoryItem item)
        {
            return FindEmptySlot();
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        //private int FindStack(InventoryItem item)
        //{
        //    if (!item.IsStackable())
        //    {
        //        return -1;
        //    }

        //    for (int i = 0; i < slots.Length; i++)
        //    {
        //        if (object.ReferenceEquals(slots[i].item, item))
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        //[System.Serializable]
        //private struct InventorySlotRecord
        //{
        //    public string itemID;
        //    public int number;
        //}

        object ISaveable.CaptureState()
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i] != null)
                {
                    slotStrings[i] = slots[i].GetItemID();
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (string[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i] = InventoryItem.GetFromID(slotStrings[i]);
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
    }
}