using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    //public Inventory inventory;
    public GameObject player;

    Inventory inventory;
    string moneyMsg;
    string radishMsg;
    string radishSeedMsg;
    WorldTime gametime;

    void Start ()
    {
        GameObject obj = GameObject.FindGameObjectWithTag ("WorldTime");
        gametime = (WorldTime)obj.GetComponent<WorldTime> ();

        inventory = (Inventory)player.GetComponent<Inventory> ();
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
        moneyMsg = "Shellings: " + inventory.money + "\n";
        radishMsg = "Radishes: " + inventory.GetItemCount (ItemDatabase.RADISH);
        radishSeedMsg = "Radish Seeds: " + inventory.GetItemCount (ItemDatabase.RADISH_SEEDS);
        GUI.Label (new Rect (10, Screen.height - 115, 120, 100), moneyMsg);
        GUI.Label (new Rect (10, Screen.height - 100, 120, 100), radishMsg);
        GUI.Label (new Rect (10, Screen.height - 85, 120, 100), radishSeedMsg);
    }
}