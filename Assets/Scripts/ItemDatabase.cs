using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour
{
    // TODO: Store items in a structure that can grow as we add items to the file
    public Item[] items = new Item[10];

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

    public void AddItem(Item item)
    {
        items[item.id] = item;
    }

}
