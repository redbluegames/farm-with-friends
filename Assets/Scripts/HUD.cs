using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	//public Inventory inventory;
	public GameObject player;
	
	private Inventory inventory;
	private string moneyMsg;

	private string radishMsg;
	private string radishSeedMsg;
	
	void Start()
	{
		inventory = (Inventory) player.GetComponent<Inventory>();
	}
	
	void OnGUI()
	{
		DrawInventory();
		DrawPlayerHUD();
	}

	private void DrawPlayerHUD()
	{
	}

	private void DrawInventory()
	{
		// TODO this is silly to perform a get every single frame.
		moneyMsg = "Shellings: " + inventory.money;
		radishMsg = "Radishes: " + inventory.getItemCount(ItemDatabase.RADISH);
		radishSeedMsg = "Radish Seeds: " + inventory.getItemCount(ItemDatabase.RADISH_SEEDS);
		GUI.Label(new Rect(10, Screen.height - 115, 120, 100), moneyMsg);
		GUI.Label(new Rect(10, Screen.height - 100, 120, 100), radishMsg);
		GUI.Label(new Rect(10, Screen.height - 85, 120, 100), radishSeedMsg);
	}
}