using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    public int money;
    ItemDatabase itemDB;
    Dictionary<int, int> itemCounts;
    public int equippedItemKey = -1;
    int equippedItemIndex = -1;

    void Awake ()
    {
        itemCounts = new Dictionary<int, int> ();
        itemDB = (ItemDatabase)GameObject.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
        itemCounts.Add (ItemDatabase.RADISH_SEEDS, 9);
    }

    void Start()
    {
        // Equip the radish seeds that were put in on Awake
        EquipNextItem ();
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
        bool isAddSuccessful;
        if (!HasItem (itemID)) {
            itemCounts.Add (itemID, count);
            isAddSuccessful = true;
        } else {
            if (itemDB.GetItem (itemID).maxCount >= itemCounts [itemID] + count) {
                itemCounts [itemID] += count;
                isAddSuccessful = true;
            }
            isAddSuccessful = false;
        }

        return isAddSuccessful;
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
                EquipNextItem();
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

    /*
     * Iterates through the dictionary of items, storing the key of an
     * "Equipped" item
     */
    public void EquipNextItem ()
    {
        // If there are no items in the inventory, equip a null item
        if(itemCounts.Count == 0)
        {
            equippedItemIndex = -1;
            equippedItemKey = -1;
            return;
        }

        // Increment the item index, which marks our place in the Dictionary
        equippedItemIndex ++;
        if (equippedItemIndex >= itemCounts.Count) {
            equippedItemIndex = 0;
        }

        // Search the dictionary for the next item
        int i = 0;
        foreach (KeyValuePair<int, int> item in itemCounts) {
            if (i == equippedItemIndex) {
                equippedItemKey = item.Key;
                break;
            }
            i++;
         }
    }

    public Item GetEquippedItem ()
    {
        if (equippedItemIndex == -1) {
            return null;
        }
        return itemDB.GetItem (equippedItemKey);
    }
}