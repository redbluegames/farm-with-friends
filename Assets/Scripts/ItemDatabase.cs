using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour
{
    public static readonly int RADISH = 0;
    public static readonly int RADISH_SEEDS = 1;
    public Item[] items = new Item[10];

    void Start ()
    {
        Item radish = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radish.name = "Radish";
        radish.itemName = "Radish";
        radish.maxCount = 100;
        radish.price = 30;
        radish.sellPrice = 20;
        radish.id = RADISH;
        items [RADISH] = radish;

        Item radishSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radishSeeds.name = "Radish Seeds";
        radishSeeds.itemName = "Radish Seeds";
        radishSeeds.maxCount = 100;
        radishSeeds.price = 5;
        radishSeeds.sellPrice = 2;
        radishSeeds.id = RADISH_SEEDS;
        items [RADISH_SEEDS] = radishSeeds;
    }

    /*
  * Retrieve an item from the database by id. Constant time.
  */
    public Item GetItem (int itemID)
    {
        return items [itemID];
    }

    /*
  * Retrieve an item from the database by name. This iterates through
  * all items os it's linearly expensive.
  */
    public Item GetItem (string name)
    {
        foreach (Item item in items) {
            if (item.itemName.Equals (name, StringComparison.Ordinal)) {
                return item;
            }
        }
        return null;
    }
}
