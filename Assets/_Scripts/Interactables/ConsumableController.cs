using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    CharacterStats stats;
    private void Start()
    {
        stats = GetComponent<Player>().characterStats;
    }

    public void ConsumeItem(Item itemToConsume)
    {
        ConsumeItem(itemToConsume, Instantiate(Resources.Load<GameObject>("Consumables/" + itemToConsume.ObjectSlug)));
    }

    public void ConsumeItem(Item item, GameObject itemToSpawn)
    {
        // if an item has modifier on it  
        if (item.ItemModifier)
        {
            itemToSpawn.GetComponent<IConsumable>().Consume(stats);
        }
        else 
            itemToSpawn.GetComponent<IConsumable>().Consume();
    }
}
