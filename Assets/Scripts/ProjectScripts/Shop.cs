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
    int activePlayerIndex;

    // GUI properties
    public GUIStyle itemDescriptionStyle;
    int shopHeight = (int)(Screen.height * 0.75);
    int shopWidth = (int)(Screen.width * 0.75);
    int leftStart = 0;
    int widthMargins = Screen.width / 8;
    readonly int LABEL_H = Screen.height / 18;
    readonly int VERT_MARGINS = Screen.height / 8;
    readonly int PADDING = 5;
    readonly int BTN_H = Screen.height / 16;
    int btnWidth = Screen.width / 4;
    readonly int SCROLL_W = 20;
    Vector2 scrollPos;
    string rightHandLabel;
    string selectedItem;
    int shopWidthWithoutPadding;
    int leftSideW;
    int rightSideW;
    int innerBoxH;

    // GUI Focus properties
    float lastVertAxis;
    bool upPressed, downPressed;
    bool actionPressed, swapPressed, weaponPressed, exitPressed;
    int focusId;

    // Sounds
    public AudioClip cashRegister;
    public AudioClip errorSound;

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
    }

    void Update ()
    {
        if (state != ShopState.NONE) {
            DetectControllerButtons ();
            DetectVertAxis ();
        }
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
            leftStart = (int)(Screen.width * ((float)activePlayerIndex / numPlayers));
            shopWidth = (int)(Screen.width * (0.75 / numPlayers));
            widthMargins = Screen.width / 16;
        }
        shopWidthWithoutPadding = shopWidth - (4 * PADDING);
        leftSideW = (shopWidthWithoutPadding / 3) + SCROLL_W;
        rightSideW = (shopWidthWithoutPadding - leftSideW);
        innerBoxH = shopHeight - BTN_H - LABEL_H;
        btnWidth = shopWidth / 3;

        // Set up our main Shop window
        GUI.BeginGroup (new Rect (leftStart + widthMargins, VERT_MARGINS, shopWidth, shopHeight));
        if (state == ShopState.BUYING) {
            GUI.Box (new Rect (0, 0, shopWidth, shopHeight), "BUYING GOODS");
            // Lazy load our item info
            DisplayInventoryData (shopInventory);
            if (selectedItem != null) {
                DisplayItemDescription ();
                if (GUI.Button (new Rect (shopWidth - btnWidth - (2 * PADDING), innerBoxH / 2 + LABEL_H, btnWidth, BTN_H),
                        "Purchase (A/space)") || actionPressed) {
                    actionPressed = false;
                    if (CanBuy (selectedItem, 1)) {
                        BuyItem (selectedItem, 1);
                    } else {
                        AudioSource.PlayClipAtPoint (errorSound, transform.position);
                    }
                }
            }
        }
        if (state == ShopState.SELLING) {
            GUI.Box (new Rect (0, 0, shopWidth, shopHeight), "SELLING GOODS");
            DisplayInventoryData (playerInventory);
            if (selectedItem != null) {
                DisplayItemDescription ();
                if (GUI.Button (new Rect (shopWidth - btnWidth - (2 * PADDING), innerBoxH / 2 + LABEL_H, btnWidth, BTN_H),
                        "Sell Item (A/space)") || actionPressed) {
                    actionPressed = false;
                    SellItem (selectedItem, 1);
                }
                if (GUI.Button (new Rect (shopWidth - (2 * btnWidth) - (2 * PADDING), innerBoxH / 2 + LABEL_H, btnWidth, BTN_H),
                        "Sell ALL ITEMS (Y/k)") || weaponPressed) {
                    weaponPressed = false;
                    SellItem (selectedItem, playerInventory.GetItemCount (itemDB.GetItemByName (selectedItem).id));
                }
            }
        }
        DisplayBuySellButton ();

        if (GUI.Button (new Rect (shopWidth - btnWidth, shopHeight - BTN_H, btnWidth, BTN_H),
          new GUIContent ("Stop Shopping (B/Ld)")) || exitPressed) {
            exitPressed = false;
            StopShopping (activePlayerIndex);
        }
        GUI.EndGroup ();
    }

    /*
     * Depending on the state of the shop, display a Sell or Buy button.
     */
    private void DisplayBuySellButton ()
    {
        if (state == ShopState.SELLING) {
            if (GUI.Button (new Rect (shopWidth - btnWidth * 3, shopHeight - BTN_H, btnWidth, BTN_H),
              new GUIContent ("Buy (LB/q)")) || swapPressed) {
                StartBuying (activePlayerIndex);
                swapPressed = false;
            }
        } else {
            if (GUI.Button (new Rect (shopWidth - btnWidth * 2, shopHeight - BTN_H, btnWidth, BTN_H),
              new GUIContent ("Sell (LB/q)")) || swapPressed) {
                StartSelling (activePlayerIndex);
                swapPressed = false;
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
        if (itemNames == null || itemNames.Length == 0)
            return;
        if (itemDescriptions == null)
            itemDescriptions = RetrieveItemDescriptions (inventory);
        scrollPos = GUI.BeginScrollView (new Rect (PADDING * 2, LABEL_H, leftSideW, innerBoxH), scrollPos,
            new Rect (SCROLL_W, 0, SCROLL_W, itemNames.Length * LABEL_H), false, false);

        bool itemClicked = false;
        for (int i = 0; i < itemNames.Length; ++i) {
            GUI.SetNextControlName (i.ToString ());
            if (GUI.Button (new Rect (PADDING, i * LABEL_H, leftSideW - 4, LABEL_H), itemNames [i])) {
                itemClicked = true;
                selectedItem = itemNames [i];
                rightHandLabel = itemDescriptions [i];
                focusId = i;
            }
        }
        // Use our control stick input if mouse wasn't used to select an item
        if (!itemClicked) {
            focusId = ManageFocus (focusId, itemNames.Length);
            selectedItem = itemNames [focusId];
            rightHandLabel = itemDescriptions [focusId];
        }
        GUI.FocusControl (focusId.ToString ());
        GUI.EndScrollView ();
    }

    /*
     * Display the box on the right hand side of the shop with info on the
     * selected item.
     */
    void DisplayItemDescription ()
    {
        GUI.Box (new Rect (leftSideW + (3 * PADDING), LABEL_H, rightSideW, innerBoxH / 2), rightHandLabel,
            itemDescriptionStyle);
    }

    /*
     * Determine which buttons are being pressed and set our global booleans
     * for later consumption by the UI.
     */
    void DetectControllerButtons ()
    {
        GameObject playerObj = FindActivePlayer ();
        InputDevice device = playerObj.GetComponent<PlayerController> ().playerDevice;
        // Bumper buttons
        if (RBInput.GetButtonDownForPlayer (InputStrings.SWAPITEM, activePlayerIndex, device)) {
            swapPressed = true;
        } else {
            swapPressed = false;
        }
        // Action button (A)
        if (RBInput.GetButtonDownForPlayer (InputStrings.ACTION, activePlayerIndex, device)) {
            actionPressed = true;
        } else {
            actionPressed = false;
        }
        // Weapon2 buttons (Y)
        if (RBInput.GetButtonDownForPlayer (InputStrings.WEAPON2, activePlayerIndex, device)) {
            weaponPressed = true;
        } else {
            weaponPressed = false;
        }
        // Exit button (B)
        if (RBInput.GetButtonDownForPlayer (InputStrings.ITEM, activePlayerIndex, device)) {
            exitPressed = true;
        } else {
            exitPressed = false;
        }
    }

    /*
     * Run through the vertical axis input to see if it has changed. If it has,
     * set the corresponding bools to let the GUI know later.
     */
    void DetectVertAxis ()
    {
        GameObject playerObj = FindActivePlayer ();
        InputDevice device = playerObj.GetComponent<PlayerController> ().playerDevice;
        float axis = RBInput.GetAxisRawForPlayer (InputStrings.VERTICAL, activePlayerIndex, device);
        bool axisChanged = (Mathf.Sign (axis) != Mathf.Sign (lastVertAxis)) || lastVertAxis == 0;
        if (axis > 0 && axisChanged) {
            upPressed = true;
            downPressed = false;
        } else if (axis < 0 && axisChanged) {
            upPressed = false;
            downPressed = true;
        } else {
            upPressed = false;
            downPressed = false;
        }
        lastVertAxis = axis;
    }

    /*
     * Return the focus ID of the inventory button that should be selected
     * depending on the player's most recently changed axis input.
     */
    int ManageFocus (int ID, int length)
    {
        if (upPressed && ID > 0) {
            ID--;
            upPressed = false;
            downPressed = false;
        } else if (downPressed && ID < length - 1) {
            ID++;
            upPressed = false;
            downPressed = false;
        }
        return ID;
    }

    /*
     * Ensure that itemNames and itemDescriptions get reset so that they
     * won't be cached between screeens.
     */
    private void ResetItemData ()
    {
        focusId = 0;
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
            AudioSource.PlayClipAtPoint (cashRegister, transform.position);
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
            AudioSource.PlayClipAtPoint (cashRegister, transform.position);
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

    /*
     * Make the shop aware of which player is currently using it.
     */
    void SetActivePlayer (int playerIndex)
    {
        activePlayerIndex = playerIndex;
        playerInventory = (Inventory)FindActivePlayer ().GetComponent<Inventory> ();
    }

    /*
     * Return the GameObject of the player that is currently shopping.
     */
    GameObject FindActivePlayer ()
    {
        return GameObject.Find ("Player" + activePlayerIndex.ToString ());
    }

    /*
     * Return the GameObject of the player that is not currently shopping.
     */
    GameObject FindInactivePlayer ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag ("Player");
        foreach (GameObject obj in objs) {
            //If we add more than 2 players, this code would need to change.
            if (obj.GetComponent<PlayerController> ().PlayerIndex != activePlayerIndex) {
                return obj;
            }
        }
        return null;
    }

    /*
     * Set the active shopper to the provided player and put the shop in BUYING state.
     * If a player is already shopping, they will be returned to a normal state and
     * the newly provided player will be given control.
     */
    public void StartBuying (int playerIndex)
    {
        // Swap player states
        SetActivePlayer (playerIndex);
        PlayerController playerController = (PlayerController)FindActivePlayer ()
            .GetComponent<PlayerController> ();
        playerController.SetShoppingState ();
        GameObject playerObj = FindInactivePlayer ();
        if (playerObj != null) {
            playerController = (PlayerController)playerObj.GetComponent<PlayerController> ();
            playerController.SetNormalState ();
        }
        ResetItemData ();
        state = ShopState.BUYING;
    }

    /*
     * Begin selling. This is designed only for when a player is already shopping.
     */
    public void StartSelling (int playerIndex)
    {
        SetActivePlayer (playerIndex);
        ResetItemData ();
        state = ShopState.SELLING;
    }

    /*
     * Stop a provided player's shopping session.
     */
    public void StopShopping (int playerIndex)
    {
        if (playerIndex == activePlayerIndex) {
            PlayerController playerController = (PlayerController)FindActivePlayer ().GetComponent<PlayerController> ();
            playerController.SetNormalState ();
            ResetItemData ();
            selectedItem = String.Empty;
            state = ShopState.NONE;
            activePlayerIndex = -1;
        }
    }

}
