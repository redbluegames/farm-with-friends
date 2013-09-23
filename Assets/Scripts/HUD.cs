using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    //public Inventory inventory;
	public GameObject player;
	
	private Inventory inventory;
	private string radishMsg;
	private string radishSeedMsg;
	
	void Start()
	{
		inventory = (Inventory) player.GetComponent<Inventory>();
	}
	
    void OnGUI()
    {	
		// TODO this is silly to perform a get every single frame. 
		radishMsg = "Radishes: " + inventory.getItemCount(Inventory.RADISH);
		radishSeedMsg = "Radish Seeds: " + inventory.getItemCount(Inventory.RADISH_SEED);
        GUI.Label(new Rect(10, Screen.height - 100, 120, 100), radishMsg);
        GUI.Label(new Rect(10, Screen.height - 85, 120, 100), radishSeedMsg);
    }
}
