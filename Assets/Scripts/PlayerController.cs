using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float movespeed;
	public GameObject reticulePrefab;
	public AudioClip digSound;
	public AudioClip digSoundFail;
	
	private GameObject reticule;
	private GameObject actionTile;
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
	}
	
	void Update() {
		TryHoe();
		TryPlanting();
		TryPicking();
	}
	
	private void LateUpdate()
	{
		if( reticule != null)			
			SnapReticuleToActionTile();
	}
	
	// Snaps the Player's reticule to the current action tile
	private void SnapReticuleToActionTile()
	{	
		GameObject tile = GetActionTile();
		actionTile = tile;
		if ( tile != null)
		{
			if( !reticule.activeInHierarchy)
				reticule.SetActive(true);
			SnapReticuleToTarget(actionTile);
		}
		else
		{
			if( reticule.activeInHierarchy)
				reticule.SetActive(false);
		}
	}
	
	// Snaps the reticule's position and rotation to match a target
	private void SnapReticuleToTarget(GameObject target)
	{
		reticule.transform.position = target.transform.position;
		reticule.transform.rotation = target.transform.rotation;
		
		// Might need to change YOffset based on what is selected.
		const float targetYOffset = 0.25f;
		reticule.transform.position = target.transform.position + Vector3.up * targetYOffset;
	}
	
	// Spawns a reticule using the reticule Prefab
	private void SpawnReticule()
	{
		reticule = (GameObject) Instantiate (reticulePrefab, Vector3.zero, Quaternion.identity);
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
	
	/*
	 * Attempts to hoe the action tile
	*/
	void TryHoe()
	{
		bool isFire1 = Input.GetButtonDown("Fire1");
		if(isFire1)
		{
			if(actionTile != null)
			{
				GroundTile tile = (GroundTile) actionTile.GetComponent<GroundTile>();
				tile.Hoe ();
				
				AudioSource.PlayClipAtPoint(digSound, transform.position);
			}
			else {	
				AudioSource.PlayClipAtPoint(digSoundFail, transform.position);
			}
				
		}
	}
	
	/*
	 * Check if the user tried planting and if so, check that location is
	 * a valid place to plant. Then plant it and handle inventory changes.
	 */
	void TryPlanting()
	{
		bool isFire2 = Input.GetButtonDown("Fire2");
		if(isFire2)
		{
			if(actionTile != null)
			{
				GroundTile tile = (GroundTile) actionTile.GetComponent<GroundTile>();
				if (tile.isSoil()) {
					Inventory inventory = (Inventory) GetComponent<Inventory>();
					if (inventory.removeItem(Inventory.RADISH_SEED))
					{
						tile.Plant ();
					}
				}
			}
		}
	}
	
	/*
	 * Check if the tile is a valid tile to be picked and if so, pick it
	 * and handle inventory changes.
	 */
	void TryPicking()
	{
		bool isFire2 = Input.GetButtonDown("Fire2");
		if(isFire2)
		{
			if(actionTile != null)
			{
				GroundTile tile = (GroundTile) actionTile.GetComponent<GroundTile>();
				Plant plant = tile.getPlant();
				if (plant != null && plant.isRipe()) {
					Inventory inventory = (Inventory) GetComponent<Inventory>();
					tile.Pick();
					inventory.addItem(Inventory.RADISH);
				}
			}
		}
	}
	
	GameObject GetActionTile()
	{
		// The Player tries to act on the tile in front of him - assumes no tiles overlap in Y
		float zOffset = 1.0f;
		Vector3 actionOffset = new Vector3(0.0f, 0.0f, zOffset);
		Vector3 actionPosition = transform.position + transform.forward * actionOffset.magnitude;
		
		// Should use a constant for a grid size...
		const float TILESIZE_HALF = 1.0f / 2;
		
		// From all tiles, find the that our action position overlaps
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		GameObject actionTile = null;
		foreach( GameObject tile in tiles)
		{
			if( Mathf.Abs ((actionPosition.x - tile.transform.position.x) ) < TILESIZE_HALF && 
				Mathf.Abs( ( actionPosition.z - tile.transform.position.z)) < TILESIZE_HALF)
			{
					actionTile = tile;
			}
		}
		
		return actionTile;
	}
}
