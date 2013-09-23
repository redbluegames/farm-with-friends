using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public const string RADISH = "radish";
	public const string RADISH_SEED = "radishSeed";
	
	private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
	
	void Start ()
	{
		itemCounts.Add(RADISH_SEED, 9);
	}
	
	// Stub method for testing 
	void Update ()
	{
		if(Input.GetKeyDown("p"))
		{
			addItem(RADISH_SEED);
		}
	}
	
	/*
	 * Add an item if possible. Return true if added.
	 */
	public bool addItem(string item)
	{
		int value = 0;
		if (!itemCounts.ContainsKey(item))
		{
			itemCounts.Add(item, 1);
			return true;
		} else if (itemCounts.TryGetValue(item, out value))
		{
			// TODO Max count??? Might be item dependent?
			itemCounts[item] = value + 1;
			return true;
		}
		return false;
	}
	
	/*
	 * Remove an item if possible. Return true if removed.
	 */
	public bool removeItem(string item)
	{
		int value = 0;
		if (!itemCounts.ContainsKey(item))
		{
			Debug.LogError("Tried to remove item that never existed in inventory.");
			return false;
		} else if (itemCounts.TryGetValue(item, out value))
		{
			if (value - 1 < 0) {
				itemCounts[item] = 0;
				return false;
			} else {
				itemCounts[item] = value - 1;
				return true;
			}
		}
		return false;
	}
	
	/*
	 * Return the current count of a given item, adding the key
	 * if necessary.
	 */
	public int getItemCount(string item)
	{
		if (!itemCounts.ContainsKey(item))
		{
			itemCounts.Add(item, 0);
		}
		return itemCounts[item];
	}
}