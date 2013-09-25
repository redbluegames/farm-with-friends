using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	public Inventory playerInventory;
	public ItemDatabase itemDB;

	private bool selling;

	private readonly int SHOP_HEIGHT = (3 * Screen.height/4);
	private readonly int SHOP_WIDTH = (3 * Screen.width/4);
	private readonly int WIDTH_PADDING = Screen.width/8;
	private readonly int HEIGHT_PADDING = Screen.height/8;

	private readonly int BTN_HEIGHT = 20;
	private readonly int BTN_WIDTH = 100;

	private readonly int UNSELECTED = -1;

	private string[] itemNames;

	void Update()
	{
		if (Input.GetKeyDown("i"))
		{
			selling = true;
		}
	}

	/*
	 * Print and process the Shopping dialog.
	 */
	void OnGUI()
	{
		if (selling)
		{
			int gridSelection = UNSELECTED;
			itemNames = parseItemNames();
			GUI.BeginGroup(new Rect(WIDTH_PADDING, HEIGHT_PADDING, SHOP_WIDTH, SHOP_HEIGHT));
			GUI.Box(new Rect(0, 0, SHOP_WIDTH, SHOP_HEIGHT), "Welcome Traveler");
			gridSelection = GUI.SelectionGrid(new Rect(0, 10, SHOP_WIDTH, SHOP_HEIGHT - 30),
				gridSelection, itemNames, 8);
			if (gridSelection != UNSELECTED){
				sellItem(gridSelection);
			}
			if (GUI.Button(new Rect(SHOP_WIDTH - BTN_WIDTH, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
				new GUIContent("Stop Shopping")))
			{
				selling = false;
			}
			GUI.EndGroup();
		}
	}

	/*
	 * Sell an item by it's position in the grid. Remove the item and
	 * add the money to the player's money.
	 */
	private void sellItem(int grid)
	{
		playerInventory.removeItem(itemDB.GetItem(itemNames[grid]).id);
		playerInventory.addMoney(itemDB.GetItem(itemNames[grid]).sellPrice);
	}

	/*
	 * Retrieve all the owned items and get the item names to display in
	 * the shopping gui.
	 */
	private string[] parseItemNames()
	{
		string[] names = new string[playerInventory.getItems().Count];
		int index = 0;
		foreach (int itemID in playerInventory.getItems().Keys)
		{
			names[index] = itemDB.GetItem(itemID).itemName;
			index++;
		}
		return names;
	}
}
