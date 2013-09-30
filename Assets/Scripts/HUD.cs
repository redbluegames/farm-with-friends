using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    GameObject player;
    GameObject player2;
    Inventory inventory;
    Inventory inventory2;
    string moneyMsg;
    string radishMsg;
    string radishSeedMsg;
    WorldTime gametime;
    int numPlayers;

    void Start ()
    {
        GameObject obj = GameObject.FindGameObjectWithTag ("WorldTime");
        gametime = (WorldTime)obj.GetComponent<WorldTime> ();

        player = GameObject.Find ("Player1");
        inventory = (Inventory)player.GetComponent<Inventory> ();
        if (GameObject.FindGameObjectsWithTag ("Player").Length == 2) {
            AddSecondPlayerHUD ();
        }
    }
 
    void OnGUI ()
    {
        DrawInventory ();
        DrawPlayerHUD ();
    }

    private void DrawPlayerHUD ()
    {
        GUI.BeginGroup (new Rect (10, 10, 120, 100));
        GUI.Label (new Rect (0, 0, 120, 100), "Day: " + gametime.GetDay ());
        GUI.Label (new Rect (0, 15, 120, 100), "Hour: " + gametime.GetHour ());
        GUI.Label (new Rect (0, 30, 120, 100), "Minute: " + gametime.GetMinute ());
        GUI.EndGroup ();
    }

    private void DrawInventory ()
    {
        // Draw player 1's inventory
        // TODO Let's not have a Find and a GetComponent onGUI
        inventory = (Inventory)GameObject.Find ("Player1").GetComponent<Inventory> ();
        string inventoryMsg = "Shellings: " + inventory.money + "\n";
        inventoryMsg += "Radishes: " + inventory.GetItemCount (ItemIDs.RADISH) + "\n";
        inventoryMsg += "Radish Seeds: " + inventory.GetItemCount (ItemIDs.RADISH_SEEDS);
        GUI.Label (new Rect (10, Screen.height - 115, 120, 100), inventoryMsg);

        // Draw player 1's equipped item
        // TODO Can GetComponent be avoided? Isn't this expensive?
        Item equippedItem = player.GetComponent<PlayerController> ().GetEquippedItem ();
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
        if (player2 == null) {
            itemXPos = Screen.width;
        } else {
            itemXPos = Screen.width / 2;
        }
        GUI.Label (new Rect (itemXPos - itemLabelWidth, itemLabelYOffset, itemLabelWidth, itemLabelHeight), itemName);

        if (player2 != null) {
            // Now draw player 2's
            if (inventory2 != null) {
                inventoryMsg = "Shellings: " + inventory2.money + "\n";
                inventoryMsg += "Radishes: " + inventory2.GetItemCount (ItemIDs.RADISH) + "\n";
                inventoryMsg += "Radish Seeds: " + inventory2.GetItemCount (ItemIDs.RADISH_SEEDS);
                GUI.Label (new Rect (10 + (Screen.width / 2), Screen.height - 115, 120, 100), inventoryMsg);
            }
            equippedItem = player2.GetComponent<PlayerController> ().GetEquippedItem ();
            if (equippedItem == null) {
                itemName = "No Item Equipped";
            } else {
                itemName = equippedItem.itemName;
            }
            GUI.Label (new Rect (Screen.width - itemLabelWidth, itemLabelYOffset, itemLabelWidth, itemLabelHeight), itemName);
        } else {
            DisplayHowToEnterText ();
        }
    }

    /*
     * Displays the instructions for second player entering the game. Hide this
     * once the player joins.
     */
    public void DisplayHowToEnterText ()
    {
        int width = Screen.width / 3;
        GUI.Label (new Rect (Screen.width - width, Screen.height - 20, width, 20), 
            "Player 2 Press ENTER to join.");
    }

    /*
     * Set the necessary variables to trigger drawing of a second HUD.
     */
    public void AddSecondPlayerHUD ()
    {
        player2 = GameObject.Find ("Player2");
        inventory2 = (Inventory)player2.GetComponent<Inventory> ();
        numPlayers = 2;
    }
}