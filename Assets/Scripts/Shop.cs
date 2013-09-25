using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour
{
    Inventory playerInventory;
    Inventory shopInventory;
    ItemDatabase itemDB;
    string[] itemNames;
    ShopState state;
    readonly int SHOP_HEIGHT = (3 * Screen.height / 4);
    readonly int SHOP_WIDTH = (3 * Screen.width / 4);
    readonly int WIDTH_PADDING = Screen.width / 8;
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
        shopInventory.AddItem (ItemDatabase.RADISH_SEEDS, INFINITE);
    }

    /*
     * Print and process the Shopping dialog.
     */
    void OnGUI ()
    {
        if (state == ShopState.NONE) {
            return;
        }
        GUI.BeginGroup (new Rect (WIDTH_PADDING, HEIGHT_PADDING, SHOP_WIDTH, SHOP_HEIGHT));
        int gridSelection = UNSELECTED;
        if (state == ShopState.BUYING) {
            GUI.Box (new Rect (0, 0, SHOP_WIDTH, SHOP_HEIGHT), "BUYING");
            itemNames = RetrieveItemNames (shopInventory);
            gridSelection = GUI.SelectionGrid (new Rect (0, 10, SHOP_WIDTH, SHOP_HEIGHT - 10 -BTN_HEIGHT),
             gridSelection, itemNames, 8);
            if (gridSelection != UNSELECTED) {
                if (CanBuy (gridSelection, 1)) {
                    BuyItem (gridSelection, 1);
                } else {
                    Debug.Log ("User tried to buy something they couldn't. This is where we'd handle that.");
                }
            }
            if (GUI.Button (new Rect (SHOP_WIDTH - BTN_WIDTH * 2, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
              new GUIContent ("Sell"))) {
                StartSelling ();
            }
        }
        if (state == ShopState.SELLING) {
            itemNames = RetrieveItemNames (playerInventory);
            GUI.Box (new Rect (0, 0, SHOP_WIDTH, SHOP_HEIGHT), "SELLING");
            gridSelection = GUI.SelectionGrid (new Rect (0, 10, SHOP_WIDTH, SHOP_HEIGHT - 10 -BTN_HEIGHT),
             gridSelection, itemNames, 8);
            if (gridSelection != UNSELECTED) {
                SellItem (gridSelection, 1);
            }
            if (GUI.Button (new Rect (SHOP_WIDTH - BTN_WIDTH * 2, SHOP_HEIGHT - BTN_HEIGHT, BTN_WIDTH, BTN_HEIGHT),
              new GUIContent ("Buy"))) {
                StartBuying ();
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
        Item item = itemDB.GetItem(itemNames[grid]);
        playerInventory.RemoveItem (item.id, count);
        playerInventory.AddMoney (item.sellPrice * count);
    }

    /*
     * Check if a player can buy the amount of item specified.
     */
    private bool CanBuy (int grid, int count)
    {
        Item item = itemDB.GetItem(itemNames[grid]);
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
        Item item = itemDB.GetItem(itemNames[grid]);
        int totalCost = item.price * count;
        if (playerInventory.HasMoney(totalCost)) {
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
    private string[] RetrieveItemNames (Inventory inventory)
    {
        string[] names = new string[inventory.GetItems ().Count];
        int index = 0;
        foreach (int itemID in inventory.GetItems().Keys) {
            names [index] = itemDB.GetItem (itemID).itemName;
            index++;
        }
        return names;
    }

    public void StartBuying ()
    {
        playerInventory = (Inventory)GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ();
        state = ShopState.BUYING;
    }

    public void StartSelling ()
    {
        playerInventory = (Inventory)GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ();
        state = ShopState.SELLING;
    }

    public void StopShopping ()
    {
        state = ShopState.NONE;
    }
}
