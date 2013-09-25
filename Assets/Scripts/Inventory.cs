using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    public int money;

    ItemDatabase itemDB;
    Dictionary<int, int> itemCounts;

    void Awake ()
    {
        itemDB = (ItemDatabase)GameObject.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
        itemCounts = new Dictionary<int, int> ();
        itemCounts.Add (ItemDatabase.RADISH_SEEDS, 9);
    }

    // Stub method for testing 
    void Update ()
    {
        if (Input.GetKeyDown ("p")) {
            AddItem (ItemDatabase.RADISH_SEEDS, 1);
        }
    }

    /*
  * Add specified amount of money to player.
  */
    public void AddMoney (int amount)
    {
        money += amount;
    }

    /*
  * Take away specified amount of money from player.
  */
    public bool RemoveMoney (int amount)
    {
        if (HasMoney (amount)) {
            money -= amount;
            return true;
        } 
        Debug.LogWarning (String.Format (
         "Tried to remove more money ({0}) than player had ({1}). " +
         "Perform hasMoney check to protect this call.", amount, money));
        return false;
    }

    /*
  * Check if player has specified amount of money. Return
  * true if they do.
  */
    public bool HasMoney (int amount)
    {
        return money < amount ? false : true;
    }

    /*
     * Add the provided amount of an item if possible. Return true if addded, otherwise false.
     */
    public bool AddItem (int itemID, int count)
    {
        if (!HasItem (itemID)) {
            itemCounts.Add (itemID, count);
            return true;
        } else {
            if (itemDB.GetItem (itemID).maxCount >= itemCounts [itemID] + count) {
                itemCounts [itemID] += count;
                return true;
            }
            return false;
        }
    }

    /*
  * Remove the provided amount of an item if possible. Return true if removed.
  */
    public bool RemoveItem (int itemID, int count)
    {
        if (!HasItem (itemID)) {
            Debug.LogWarning ("Tried to remove item that never existed in inventory.");
            return false;
        } else if (HasCountOfItem (itemID, count)) {
            if (itemCounts [itemID] - count == 0) {
                itemCounts.Remove (itemID);
            } else {
                itemCounts [itemID] -= count;
            }
            return true;
        }
        return false;
    }

    /*
     * Return whether the inventory contains the specified item using itemID.
     */
    public bool HasItem (int itemID)
    {
        return itemCounts.ContainsKey (itemID);
    }

    /*
     * Return true if the inventory contains at least the provided count of
     * a specified item.
     */
    public bool HasCountOfItem (int itemID, int count)
    {
        return itemCounts [itemID] >= count;
    }

    /*
  * Return the count of a provided item, 0 if it does not exist
  * (or is 0).
  */
    public int GetItemCount (int itemID)
    {
        if (!HasItem (itemID)) {
            return 0;
        }
        return itemCounts [itemID];
    }

    /*
  * Return all the items owned by the player inventory, including their counts.
  */
    public Dictionary<int, int> GetItems ()
    {
        return itemCounts;
    }
}