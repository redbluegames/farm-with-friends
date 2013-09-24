using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour {

	public static readonly int RADISH = 0;
	public static readonly int RADISH_SEEDS = 1;

	public Item[] items = new Item[10];

	void Start()
	{
		Item radish = (Item) ScriptableObject.CreateInstance(typeof(Item));
		radish.name = "Radish";
		radish.itemName = "Radish";
		radish.maxCount = 100;
		radish.price = 30;
		radish.sellPrice = 20;
		items[RADISH] = radish;

		Item radishSeeds = (Item) ScriptableObject.CreateInstance(typeof(Item));
		radishSeeds.name = "Radish Seeds";
		radishSeeds.itemName = "Radish Seeds";
		radishSeeds.maxCount = 100;
		radishSeeds.price = 5;
		radishSeeds.sellPrice = 2;
		items[RADISH_SEEDS] = radishSeeds;
	}

	public Item GetItem(int itemID)
	{
		return items[itemID];
	}
}
