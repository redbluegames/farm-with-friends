using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {

    public Inventory inventory;
	
	private string radishMsg;

    void OnGUI()
    {	
		radishMsg = "Radishes: " + inventory.getRadishCount();
        GUI.Label(new Rect(10, Screen.height - 100, 100, 20), radishMsg);
    }
}
