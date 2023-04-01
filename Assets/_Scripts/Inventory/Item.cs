using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json; 

public class Item 
{
    public enum ItemTypes { Weapon, Consumable, Quest }
    public List<BaseStat> Stats { get; set; }
    public string ObjectSlug { get; set; }  // to help find the proper prefab in our folder
    public string Description { get; set; }
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes ItemType { get; set; }
    public string ActionName { get; set; }
    public string ItemName { get; set; }
    public bool ItemModifier { get; set; }

    public Item(List<BaseStat> stats, string objectSlug)
    {
        this.Stats = stats;
        this.ObjectSlug = objectSlug;
    }
    [JsonConstructor]
    public Item(List<BaseStat> stats, string objectSlug, string description, ItemTypes itemType, string actionName, string itemName, bool itemModifier)
    {
        this.Stats = stats;
        this.ObjectSlug = objectSlug;
        this.Description = description;
        this.ItemType = itemType;
        this.ActionName = actionName;
        this.ItemName = itemName;
        this.ItemModifier = itemModifier;
    }

}
