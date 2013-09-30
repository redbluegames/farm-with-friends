using UnityEngine;
using System.Collections;
using System;

public class Shop : MonoBehaviour
{
    Inventory playerInventory;
    Inventory shopInventory;
    ItemDatabase itemDB;
    string[] itemNames;
    string[] itemDescriptions;
    ShopState state;
    int playerNum;

    // GUI properties
    int shopHeight = (int)(Screen.height * 0.75);
    int shopWidth = (int)(Screen.width * 0.75);
    int leftStart = 0;
    int widthMargins = Screen.width / 8;
    readonly int LABEL_H = Screen.height / 18;
    readonly int VERT_MARGINS = Screen.height / 8;
    readonly int PADDING = 5;
    readonly int BTN_H = Screen.height / 16;
    readonly int BTN_W = Screen.width / 8;
    readonly int SCROLL_W = 20;
    int focusId;
    bool focusChanged;
    Vector2 scrollPos;
    string rightHandLabel;
    string selectedItem;
    int shopWidthWithoutPadding;
    int leftSideW;
    int rightSideW;
    int innerBoxH;

    // Magic Numbers
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
        state = ShopState.NONE;
        shopInventory = GetComponent<Inventory> ();
        itemDB = (ItemDatabase)GameObject.Find ("ItemDatabase").GetComponent<ItemDatabase> ();
        shopInventory.AddItem (ItemIDs.RADISH_SEEDS, INFINITE);
        shopInventory.AddItem (ItemIDs.ONION_SEEDS, INFINITE);
        shopInventory.AddItem (ItemIDs.POTATO_SEEDS, INFINITE);
        scrollPos = Vector2.zero;
        ResetItemData ();
        focusChanged = true;
    }

    /*
     * Print and process the Shopping dialog.
     */
    void OnGUI ()
    {
        if (state == ShopState.NONE) {
            return;
        }

        GUI.skin.button.wordWrap = true;
        // Set up window dimensions for multiple players
        int numPlayers = GameObject.FindGameObjectsWithTag ("Player").Length;
        if (numPlayers > 1) {
            leftStart = Screen.width / numPlayers;
            shopWidth = (int)(Screen.width * (0.75 / 2));
            widthMargins = Screen.width / 16;
        }
        shopWidthWithoutPadding = shopWidth - (4 * PADDING);
        leftSideW = (shopWidthWithoutPadding / 3) + SCROLL_W;
        rightSideW = (shopWidthWithoutPadding - leftSideW);
        innerBoxH = shopHeight - BTN_H - LABEL_H;

        // Set up our main Shop window
        GUI.BeginGroup (new Rect (leftStart + widthMargins, VERT_MARGINS, shopWidth, shopHeight));
        GUI.Box (new Rect (0, 0, shopWidth, shopHeight), "TRAVELLING MERCHANT");
        if (state == ShopState.BUYING) {
            // Lazy load our item info
            DisplayInventoryData (shopInventory);
            if (selectedItem != null) {
                DisplayItemDescription ();
                if (GUI.Button (new Rect (shopWidth - BTN_W - (2 * PADDING), innerBoxH / 2 + LABEL_H, BTN_W, BTN_H),
                        "Purchase")) {
                    if (CanBuy (selectedItem, 1)) {
                        BuyItem (selectedItem, 1);
                    } else {
                        Debug.Log ("User tried to buy something they couldn't. This is where we'd handle that.");
                    }
                }
            }
        }
        if (state == ShopState.SELLING) {
            DisplayInventoryData (playerInventory);
            if (selectedItem != null) {
                DisplayItemDescription ();
                if (GUI.Button (new Rect (shopWidth - BTN_W - (2 * PADDING), innerBoxH / 2 + LABEL_H, BTN_W, BTN_H),
                        "Sell Item")) {
                    SellItem (selectedItem, 1);
                }
                if (GUI.Button (new Rect (shopWidth - (2 * BTN_W) - (2 * PADDING), innerBoxH / 2 + LABEL_H, BTN_W, BTN_H),
                        "Sell ALL ITEMS")) {
                    SellItem (selectedItem, playerInventory.GetItemCount (itemDB.GetItemByName (selectedItem).id));
                }
            }
        }
        DisplayBuySellButton ();
        if (GUI.Button (new Rect (shopWidth - BTN_W, shopHeight - BTN_H, BTN_W, BTN_H),
          new GUIContent ("Stop Shopping"))) {
            StopShopping ();
        }
        GUI.EndGroup ();
    }

    /*
     * Depending on the state of the shop, display a Sell or Buy button.
     */
    private void DisplayBuySellButton ()
    {
        if (state == ShopState.SELLING) {
            if (GUI.Button (new Rect (shopWidth - BTN_W * 3, shopHeight - BTN_H, BTN_W, BTN_H),
              new GUIContent ("Buy (LB)"))) {
                StartBuying (playerNum);
            }
        } else {
            if (GUI.Button (new Rect (shopWidth - BTN_W * 2, shopHeight - BTN_H, BTN_W, BTN_H),
              new GUIContent ("Sell (RB)"))) {
                StartSelling (playerNum);
            }
        }
    }

    /*
     * Determine what the inventory of the shop or player is and display it.
     * This will also set some things like which of the items is selected.
     */
    private void DisplayInventoryData (Inventory inventory)
    {
        if (itemNames == null)
            itemNames = RetrieveItemNames (inventory);
        if (itemNames == null)
            return;
        if (itemDescriptions == null)
            itemDescriptions = RetrieveItemDescriptions (inventory);
        scrollPos = GUI.BeginScrollView (new Rect (PADDING * 2, LABEL_H, leftSideW, innerBoxH), scrollPos,
            new Rect (SCROLL_W, 0, SCROLL_W, itemNames.Length * LABEL_H), false, false);

        for (int i = 0; i < itemNames.Length; ++i) {
            GUI.SetNextControlName (itemNames [i]);
            if (GUI.Button (new Rect (PADDING, i * LABEL_H, leftSideW - 4, LABEL_H), itemNames [i])) {
                selectedItem = itemNames [i];
                rightHandLabel = itemDescriptions [i];
            }
        }
        //focusId = ManageFocus (focusId, itemNames.Length);
        //GUI.FocusControl (focusId.ToString());
        GUI.EndScrollView ();
    }

    /*
     * Display the box on the right hand side of the shop with info on the
     * selected item.
     */
    private void DisplayItemDescription ()
    {
        GUI.Box (new Rect (leftSideW + (3 * PADDING), LABEL_H, rightSideW, innerBoxH / 2), rightHandLabel);
    }

    private int ManageFocus (int ID, int length)
    {
        GUI.FocusControl (ID.ToString ());
        if (focusChanged && Time.timeSinceLevelLoad > 2.0f) {
            focusChanged = false;
        }
        if ((Input.GetAxis ("Horizontal") > 0 && ID < length && !focusChanged) ||
            (Input.GetAxis ("Vertical") > 0 && ID < length && !focusChanged)) {
            focusChanged = true;
            ID++;
        } else if ((Input.GetAxis ("Horizontal") > 0 && ID < 0 && !focusChanged)) {
            ID = 0;
        }
        if ((Input.GetAxis ("Horizontal") < 0 && ID > 0 && !focusChanged) ||
            (Input.GetAxis ("Vertical") < 0 && ID > 0 && !focusChanged)) {
            focusChanged = true;
            ID--;
        } else if ((Input.GetAxis ("Horizontal") < 0 && ID < 0 && !focusChanged)) {
            ID = 0;
        }
        return ID;
    }

    /*
     * Ensure that itemNames and itemDescriptions get reset so that they
     * won't be cached between screeens.
     */
    private void ResetItemData ()
    {
        selectedItem = null;
        itemNames = null;
        itemDescriptions = null;
    }

    /*
     * Sell an item by its position in the grid. Remove the item and
     * add the money to the player's money.
     */
    private void SellItem (string name, int count)
    {
        Item item = itemDB.GetItemByName (name);
        if (playerInventory.HasItem (item.id)) {
            playerInventory.RemoveItem (item.id, count);
            playerInventory.AddMoney (item.sellPrice * count);
        }
        // When the item is no longer in inventory, reset the display.
        if (!playerInventory.HasItem (item.id)) {
            ResetItemData ();
        }
    }

    /*
     * Check if a player can buy the amount of item specified.
     */
    private bool CanBuy (string itemName, int count)
    {
        Item item = itemDB.GetItemByName (itemName);
        if (playerInventory.GetItemCount (item.id) + count > item.maxCount)
            return false;
        int totalCost = item.price * count;
        return playerInventory.HasMoney (totalCost);
    }

    /*
     * Buy an item by its position in the grid. Remove the item from
     * the shopkeeper's inventory and add it to the player's. Return
     * true if purchase succeeded.
     */
    private bool BuyItem (string itemName, int count)
    {
        Item item = itemDB.GetItemByName (itemName);
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
    private string[] RetrieveItemNames (Inventory inventory)
    {
        string[] itemNames = new string[inventory.GetItems ().Count];
        int index = 0;
        Item item;
        foreach (int itemID in inventory.GetItems().Keys) {
            item = itemDB.GetItem (itemID);
            itemNames [index] = item.itemName;
            index++;
        }
        return itemNames;
    }

    /*
     * Retrieve all the owned items and get the description of each
     * including the correct price if user is buying or selling.
     */
    private string[] RetrieveItemDescriptions (Inventory inventory)
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

    public void StartBuying (int playerIndex)
    {
        ResetItemData ();
        SetActivePlayer (playerIndex);
        state = ShopState.BUYING;
    }

    public void StartSelling (int playerIndex)
    {
        ResetItemData ();
        SetActivePlayer (playerIndex);
        state = ShopState.SELLING;
    }

    public void StopShopping ()
    {
        ResetItemData ();
        selectedItem = String.Empty;
        state = ShopState.NONE;
    }

    void SetActivePlayer (int playerIndex)
    {
        playerInventory = (Inventory)GameObject.Find ("Player" + playerIndex.ToString ()).GetComponent<Inventory> ();
    }
}
