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
            player2 = GameObject.Find ("Player2");
            inventory2 = (Inventory)player2.GetComponent<Inventory> ();
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
        // TODO this is silly to perform a get every single frame.
        // Draw player 1's
        inventory = (Inventory)GameObject.Find ("Player1").GetComponent<Inventory> ();
        string inventoryMsg = "Shellings: " + inventory.money + "\n";
        inventoryMsg += "Radishes: " + inventory.GetItemCount (ItemDatabase.RADISH) + "\n";
        inventoryMsg += "Radish Seeds: " + inventory.GetItemCount (ItemDatabase.RADISH_SEEDS);
        GUI.Label (new Rect (10, Screen.height - 115, 120, 100), inventoryMsg);

        // Now draw player 2's
        if (inventory2 != null) {
            inventory2 = (Inventory)GameObject.Find ("Player2").GetComponent<Inventory> ();
            inventoryMsg = "Shellings: " + inventory2.money + "\n";
            inventoryMsg += "Radishes: " + inventory2.GetItemCount (ItemDatabase.RADISH) + "\n";
            inventoryMsg += "Radish Seeds: " + inventory2.GetItemCount (ItemDatabase.RADISH_SEEDS);
            GUI.Label (new Rect (10 + (Screen.width / 2), Screen.height - 115, 120, 100), inventoryMsg);
        }

    }
}