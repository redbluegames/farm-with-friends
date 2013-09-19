using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float movespeed;
	public GameObject reticulePrefab;
	
	public GUIText debugText;
	
	private GameObject reticule;
	private float turnSmoothing = 90f;
	
	void Start()
	{
		SpawnReticule();
	}
	
	void FixedUpdate () {
		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis ("Vertical");
	
		if(movementHorizontal != 0f || movementVertical != 0f)
		{
			Rotate(movementHorizontal, movementVertical);
		}
		
		Move(movementHorizontal, movementVertical);
		
		GameObject tile = GetActionTile();
		if ( tile != null)
		{
			debugText.text = tile.name;
		}
		else
		{
			debugText.text = "";
		}
	}
	
	// Spawn a reticule from the Player using the assigned Prefab
	private void SpawnReticule()
	{
		// Configure the initial offset for the reticule
		float xOffset = 0.0f;
		float yOffset = -0.75f;
		float zOffset = 1f;
		Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
		
		reticule = (GameObject) Instantiate (reticulePrefab, transform.position + offset, Quaternion.identity);
		reticule.transform.parent = transform;
	}
	
	void Move(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
		Vector3 movementDelta = (targetDirection * movespeed * Time.deltaTime);
		
		rigidbody.MovePosition(rigidbody.position + movementDelta);
	}
	
	void Rotate(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
		Quaternion desiredRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, desiredRotation, turnSmoothing * Time.deltaTime);
        
		rigidbody.MoveRotation(newRotation);
	}
	
	void TryAction()
	{
		
	}
	
	GameObject GetActionTile()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		GameObject closestTile = null;
		float bestD = Mathf.Infinity;
		foreach( GameObject tile in tiles)
		{
			float dToTileSquared = (tile.transform.position - reticule.transform.position).sqrMagnitude;
			if (dToTileSquared < bestD)
			{
				bestD = dToTileSquared;
				closestTile = tile;
			}
		}
		return closestTile;
	}
}
