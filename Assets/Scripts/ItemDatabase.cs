using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour
{
    public static readonly int RADISH = 0;
    public static readonly int RADISH_SEEDS = 1;
    public static readonly int ONION = 2;
    public static readonly int ONION_SEEDS = 3;
    public static readonly int POTATO = 4;
    public static readonly int POTATO_SEEDS = 4;
    public Item[] items = new Item[10];

    void Awake ()
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
        radishSeeds.description = "Time to grow: Short.\nWater needed: Daily";
        radishSeeds.maxCount = 100;
        radishSeeds.price = 5;
        radishSeeds.sellPrice = 2;
        radishSeeds.id = RADISH_SEEDS;
        items [RADISH_SEEDS] = radishSeeds;

        Item onion = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onion.name = "Onion";
        onion.itemName = "Onion";
        onion.maxCount = 100;
        onion.price = 30;
        onion.sellPrice = 20;
        onion.id = ONION;
        items [ONION] = onion;

        Item onionSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onionSeeds.name = "Onion Seeds";
        onionSeeds.itemName = "Onion Seeds";
        onionSeeds.description = "Time to grow: Long.\nWater needed: Drought Only";
        onionSeeds.maxCount = 100;
        onionSeeds.price = 5;
        onionSeeds.sellPrice = 2;
        onionSeeds.id = ONION_SEEDS;
        items [ONION_SEEDS] = onionSeeds;

        Item potato = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potato.name = "Potato";
        potato.itemName = "Potato";
        potato.maxCount = 100;
        potato.price = 100;
        potato.sellPrice = 80;
        potato.id = POTATO;
        items [POTATO] = potato;

        Item potatoSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potatoSeeds.name = "Potato Seeds";
        potatoSeeds.itemName = "Potato Seeds";
        potatoSeeds.description = "Time to grow: Very Long.\nWater needed: Daily";
        potatoSeeds.maxCount = 100;
        potatoSeeds.price = 5;
        potatoSeeds.sellPrice = 2;
        potatoSeeds.id = POTATO_SEEDS;
        items [POTATO_SEEDS] = potatoSeeds;
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
    public Item GetItemByName (string name)
    {
        foreach (Item item in items) {
            if (item.itemName.Equals (name, StringComparison.Ordinal)) {
                return item;
            }
        }
        Debug.LogWarning ("GetItemByName did not find an item called " + name);
        return null;
    }
}
