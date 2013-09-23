using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {

    //public Inventory inventory;
	public GameObject player;
	
	private Inventory inventory;
	private string radishMsg;
	
	void Start()
	{
		inventory = (Inventory) player.GetComponent<Inventory>();
	}
	
    void OnGUI()
    {	
		radishMsg = "Radishes: " + inventory.getRadishCount();
        GUI.Label(new Rect(10, Screen.height - 100, 100, 20), radishMsg);
    }
}
