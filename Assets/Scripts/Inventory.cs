using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {

	public int money;
	public ItemDatabase itemDB;

	private Dictionary<int, int> itemCounts;

	void Start ()
	{
		itemCounts = new Dictionary<int, int>();
		itemCounts.Add(ItemDatabase.RADISH_SEEDS, 9);
	}
	
	// Stub method for testing 
	void Update ()
	{
		if(Input.GetKeyDown("p"))
		{
			addItem(ItemDatabase.RADISH_SEEDS);
		}
	}

	/*
	 * Add specified amount of money to player.
	 */
	public void addMoney(int amount)
	{
		money += amount;
	}

	/*
	 * Take away specified amount of money from player.
	 */
	public bool removeMoney(int amount)
	{
		if (hasMoney(amount)){
			money -= amount;
			return true;
		}
		Debug.LogWarning(String.Format(
			"Tried to remove more money ({0}) than player had ({1})." +
			"Perform hasMoney check to protect this call.", amount, money));
		return false;
	}

	/*
	 * Check if player has specified amount of money. Return
	 * true if they do.
	 */
	public bool hasMoney(int amount)
	{
		return money < amount ? false : true;
	}

	/*
	 * Add an item if possible. Return true if added.
	 */
	public bool addItem(int itemID)
	{
		int value = 0;
		if (!hasItem(itemID))
		{
			itemCounts.Add(itemID, 1);
			return true;
		} else if (itemCounts.TryGetValue(itemID, out value))
		{
			// TODO Max count??? Might be item dependent?
			itemCounts[itemID] = value + 1;
			return true;
		}
		return false;
	}
	
	/*
	 * Remove an item if possible. Return true if removed.
	 */
	public bool removeItem(int itemID)
	{
		int value = 0;
		if (!hasItem(itemID))
		{
			Debug.LogWarning("Tried to remove item that never existed in inventory.");
			return false;
		} else if (itemCounts.TryGetValue(itemID, out value))
		{
			if (value - 1 < 0) {
				itemCounts.Remove(itemID);
				return false;
			} else if (value -1 == 0) {
				itemCounts.Remove(itemID);
				return true;
			}else {
				itemCounts[itemID] = value - 1;
				return true;
			}
		}
		return false;
	}

	/*
	 * Return whether the inventory contains the specified item using itemID.
	 */
	public bool hasItem(int itemID)
	{
		return itemCounts.ContainsKey(itemID);
	}

	/*
	 * Return the count of a provided item, 0 if it does not exist
	 * (or is 0).
	 */
	public int getItemCount(int itemID)
	{
		if (!hasItem(itemID))
		{
			return 0;
		}
		return itemCounts[itemID];
	}

	/*
	 * Return all the items owned by the player inventory, including their counts.
	 */
	public Dictionary<int, int> getItems()
	{
		return itemCounts;
	}
}