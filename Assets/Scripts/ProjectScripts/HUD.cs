using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    GameObject player0;
    GameObject player1;
    Inventory inventory0;
    Inventory inventory1;
    string moneyMsg;
    string radishMsg;
    string radishSeedMsg;
    GameManager gameManager;
    WorldTime gametime;
    public GUIStyle HUDStyle;
    int finalScoreP0;
    int finalScoreP1;

    void Awake ()
    {
        GameObject obj = GameObject.FindGameObjectWithTag ("GameManager");
        gametime = (WorldTime)obj.GetComponent<WorldTime> ();
        gameManager = (GameManager)obj.GetComponent<GameManager> ();
    }
 
    void OnGUI ()
    {
        InstantiatePlayerHUDS ();
        DrawInventory ();
        DrawPlayerHUD ();
    }

    private void DrawPlayerHUD ()
    {
        GUI.BeginGroup (new Rect (10, 10, 340, 150));
        GUIStyle countdownStyle = new GUIStyle (HUDStyle);
        countdownStyle.fontSize = 30;
        countdownStyle.fontStyle = FontStyle.Bold;
        countdownStyle.normal.textColor = Color.white;
        // TODO: MAXDAYS should be on GameManager or somewhere else. But it's ok here for proto.
        int MAXDAYS = 30;
        int day = (int) (MAXDAYS - (gametime.GetDay () - 1));
        string label = "Days Left: ";
        if (day > 1) {
            label += day.ToString();
        } else if (day == 1){
            label = "Last Day!";
        }
        else if (day == 0)
        {
            if(player0 != null)
            {
                finalScoreP0 = inventory0.money;
            }
            if(player1 != null)
            {
                finalScoreP1 = inventory1.money;
            }
            label = GetFinalScoreLabel();
        }
        else
        {
            label = GetFinalScoreLabel();
        }

        GUI.Label (new Rect (0, 0, 100, 100), label, countdownStyle);

        GUI.Label (new Rect (0, 50, 120, 100), "Day: " + gametime.GetDay ());
        GUI.Label (new Rect (0, 65, 120, 100), "Hour: " + gametime.GetHour ());
        GUI.Label (new Rect (0, 80, 120, 100), "Minute: " + gametime.GetMinute ());
        GUI.EndGroup ();

    }

    private string GetFinalScoreLabel()
    {
        string label = "P1 Score: " + finalScoreP0;
        if(player1 != null)
        {
            label += "\nP2 Score: " + finalScoreP1;
        }
        return label;
    }

    private void DrawInventory ()
    {
        DisplayInventoryItems (inventory0, 0);
        // Draw player 1's equipped item
        // TODO Can GetComponent be avoided? Isn't this expensive?
        Item equippedItem = player0.GetComponent<PlayerController> ().GetEquippedItem ();
        string itemName = "";
        if (equippedItem == null) {
            itemName = "No Item Equipped";
        } else {
            itemName = equippedItem.itemName;
        }
        float itemLabelWidth = 100;
        float itemLabelHeight = 100;
        float itemLabelYOffset = 80;

        float itemXPos;
        if (player1 == null) {
            itemXPos = Screen.width;
        } else {
            itemXPos = Screen.width / 2;
        }
        GUI.Label (new Rect (itemXPos - itemLabelWidth, itemLabelYOffset, itemLabelWidth, itemLabelHeight),
            itemName, HUDStyle);

        if (player1 != null) {
            // Now draw player 2's
            if (inventory1 != null) {
                DisplayInventoryItems (inventory1, 1);
            }
            equippedItem = player1.GetComponent<PlayerController> ().GetEquippedItem ();
            if (equippedItem == null) {
                itemName = "No Item Equipped";
            } else {
                itemName = equippedItem.itemName;
            }
            GUI.Label (new Rect (Screen.width - itemLabelWidth, itemLabelYOffset, itemLabelWidth, itemLabelHeight),
                itemName, HUDStyle);
        } else {
            DisplayHowToEnterText ();
        }
    }

    void InstantiatePlayerHUDS ()
    {
        if (player0 == null) {
            player0 = GameObject.Find ("Player0");
            inventory0 = (Inventory)player0.GetComponent<Inventory> ();
        }
        if (gameManager.NumPlayers > 1) {
            if (player1 == null) {
                player1 = GameObject.Find ("Player1");
                inventory1 = (Inventory)player1.GetComponent<Inventory> ();
            }
        }
    }

    /*
     * Show the inventory of the provided player and at the desired screen
     * position, depending on the number of players.
     */
    void DisplayInventoryItems (Inventory inventory, int viewPort)
    {
        string inventoryMsg = "Gold: " + inventory.money + "\n\n";
        inventoryMsg += "Bean Seeds: " + inventory.GetItemCount (ItemIDs.BEAN_SEEDS) + "\n";
        inventoryMsg += "Radish Seeds: " + inventory.GetItemCount (ItemIDs.RADISH_SEEDS) + "\n";
        inventoryMsg += "Onion Seeds: " + inventory.GetItemCount (ItemIDs.ONION_SEEDS) + "\n";
        inventoryMsg += "Potato Seeds: " + inventory.GetItemCount (ItemIDs.POTATO_SEEDS) + "\n";
        inventoryMsg += "Tomato Seeds: " + inventory.GetItemCount (ItemIDs.TOMATO_SEEDS) + "\n\n";
        inventoryMsg += "Beans: " + inventory.GetItemCount (ItemIDs.BEAN) + "\n";
        inventoryMsg += "Radishes: " + inventory.GetItemCount (ItemIDs.RADISH) + "\n";
        inventoryMsg += "Onions: " + inventory.GetItemCount (ItemIDs.ONION) + "\n";
        inventoryMsg += "Potatoes: " + inventory.GetItemCount (ItemIDs.POTATO) + "\n";
        inventoryMsg += "Tomatoes: " + inventory.GetItemCount (ItemIDs.TOMATO) + "\n";
        inventoryMsg += "Wildflowers: " + inventory.GetItemCount (ItemIDs.WILDFLOWER);
        GUI.Label (new Rect (10 + ((Screen.width / gameManager.NumPlayers) * viewPort),
            Screen.height - 260, 200, 240), inventoryMsg, HUDStyle);
    }

    /*
     * Displays the instructions for second player entering the game. Hide this
     * once the player joins.
     */
    public void DisplayHowToEnterText ()
    {
        int width = Screen.width / 3;
        GUI.Label (new Rect (Screen.width - width, Screen.height - 20, width, 20), 
            "Player 2 Press ENTER to join.", HUDStyle);
    }
}