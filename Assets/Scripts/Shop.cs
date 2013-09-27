using UnityEngine;
using System.Collections;
using System;

public class Shop : MonoBehaviour
{
    Inventory playerInventory;
    Inventory shopInventory;
    ItemDatabase itemDB;
    string[] itemDisplayText;
    ShopState state;
    PlayerNum playerNum;

    // GUI properties
    int SHOP_HEIGHT = (int)(Screen.height * 0.75);
    int SHOP_WIDTH = (int)(Screen.width * 0.75);
    int LEFT_START = 0;
    int WIDTH_PADDING = Screen.width / 8;
    readonly int HEIGHT_PADDING = Screen.height / 8;
    readonly int BTN_HEIGHT = 40;
    readonly int BTN_WIDTH = 120;
    const int UNSELECTED = -1;
    const int INFINITE = int.MaxValue;

    enum ShopState
    {
        NONE,
        BUYING,
        SELLING
    }

    void Start ()
    {
        shopInventory = GetComponent<Inventory> ();
        itemDB = (ItemDatabase)GameObject.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
        shopInventory.AddItem (ItemIDs.RADISH_SEEDS, INFINITE);
        shopInventory.AddItem (ItemIDs.ONION_SEEDS, INFINITE);
        shopInventory.AddItem (ItemIDs.POTATO_SEEDS, INFINITE);
    }

    /*
     * Print and process the Shopping dialog.
     */
    void OnGUI ()
    {
        if (state == ShopState.NONE) {
            return;
        }
        if (GameObject.FindGameObjectsWithTag ("Player").Length == 2) {
            if (playerNum == PlayerNum.TWO)
                LEFT_START = Screen.width / 2;
            else
                LEFT_START = 0;
            SHOP_WIDTH = (int)(Screen.width * (0.75 / 2));
            WIDTH_PADDING = Screen.width / 16;
        }

        GUI.skin.button.wordWrap = true;
        GUI.BeginGroup (new Rect (LEFT_START + WIDTH_PADDING, HEIGHT_PADDING, SHOP_WIDTH, SHOP_HEIGHT));
        int gridSelection = UNSELECTED;
        if (state == ShopState.BUYING) {
            GUI.Box (new Rect (0, 0, SHOP_WIDTH, SHOP_HEIGHT), "BUYING");
            itemDisplayText = RetrieveItemTexts (shopInventory);
            gridSelection = GUI.SelectionGrid (new Rect (0, 10, SHOP_WIDTH, SHOP_HEIGHT - 10 - BTN_HEIGHT),
             gridSelection, itemDisplayText, 8);
            if (gridSelection != UNSELECTED) {
                if (CanBuy (gridSelection, 1)) {
                    BuyItem (gridSelection, 1);
                } else {
                    Debug.Log ("User tried to buy something they couldn't. This is where we'd handle that.");
                }
            }
            if (GUI.Button (new Rect (SHOP_WIDTH - BTN_WIDTH * 2, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
              new GUIContent ("Sell"))) {
                StartSelling (playerNum);
            }
        }
        if (state == ShopState.SELLING) {
            itemDisplayText = RetrieveItemTexts (playerInventory);
            GUI.Box (new Rect (0, 0, SHOP_WIDTH, SHOP_HEIGHT), "SELLING");
            gridSelection = GUI.SelectionGrid (new Rect (0, 10, SHOP_WIDTH, SHOP_HEIGHT - 10 - BTN_HEIGHT),
             gridSelection, itemDisplayText, 8);
            if (gridSelection != UNSELECTED) {
                SellItem (gridSelection, 1);
            }
            if (GUI.Button (new Rect (SHOP_WIDTH - BTN_WIDTH * 2, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
              new GUIContent ("Buy"))) {
                StartBuying (playerNum);
            }
        }
        if (GUI.Button (new Rect (SHOP_WIDTH - BTN_WIDTH, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
          new GUIContent ("Stop Shopping"))) {
            StopShopping ();
        }
        GUI.EndGroup ();
    }

    /*
     * Sell an item by its position in the grid. Remove the item and
     * add the money to the player's money.
     */
    private void SellItem (int grid, int count)
    {
        Item item = itemDB.GetItemByName (itemDisplayText [grid].Split ('\n') [0]);
        playerInventory.RemoveItem (item.id, count);
        playerInventory.AddMoney (item.sellPrice * count);
    }

    /*
     * Check if a player can buy the amount of item specified.
     */
    private bool CanBuy (int grid, int count)
    {
        Item item = itemDB.GetItemByName (itemDisplayText [grid].Split ('\n') [0]);
        int totalCost = item.price * count;
        return playerInventory.HasMoney (totalCost);
    }

    /*
     * Buy an item by its position in the grid. Remove the item from
     * the shopkeeper's inventory and add it to the player's. Return
     * true if purchase succeeded.
     */
    private bool BuyItem (int grid, int count)
    {
        Item item = itemDB.GetItemByName (itemDisplayText [grid].Split ('\n') [0]);
        int totalCost = item.price * count;
        if (playerInventory.HasMoney (totalCost)) {
            playerInventory.AddItem (item.id, count);
            shopInventory.RemoveItem (item.id, count);
            playerInventory.RemoveMoney (totalCost);
            return true;
        }
        return false;
    }

    /*
     * Retrieve all the owned items and get the item names to display in
     * the shopping gui.
     */
    private string[] RetrieveItemTexts (Inventory inventory)
    {
        string[] itemTexts = new string[inventory.GetItems ().Count];
        int index = 0;
        Item item;
        foreach (int itemID in inventory.GetItems().Keys) {
            item = itemDB.GetItem (itemID);
            itemTexts [index] = item.itemName + "\n";
            if (state == ShopState.BUYING) {
                itemTexts [index] += String.Format ("Price: {0}\n\n", item.price);
            } else if (state == ShopState.SELLING) {
                itemTexts [index] += String.Format ("Price: {0}\n\n", item.sellPrice);
            }
            itemTexts [index] += item.description;
            index++;
        }
        return itemTexts;
    }

    public void StartBuying (PlayerNum player)
    {
        playerNum = player;
        switch (player) {
        case PlayerNum.ONE:
            playerInventory = (Inventory)GameObject.Find ("Player1").GetComponent<Inventory> ();
            break;
        case PlayerNum.TWO:
            playerInventory = (Inventory)GameObject.Find ("Player2").GetComponent<Inventory> ();
            break;
        }
        state = ShopState.BUYING;
    }

    public void StartSelling (PlayerNum player)
    {
        playerNum = player;
        switch (playerNum) {
        case PlayerNum.ONE:
            playerInventory = (Inventory)GameObject.Find ("Player1").GetComponent<Inventory> ();
            break;
        case PlayerNum.TWO:
            playerInventory = (Inventory)GameObject.Find ("Player2").GetComponent<Inventory> ();
            break;
        }
        state = ShopState.SELLING;
    }

    public void StopShopping ()
    {
        state = ShopState.NONE;
    }
}
