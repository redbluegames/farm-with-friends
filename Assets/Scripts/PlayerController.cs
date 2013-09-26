using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movespeed;
    public GameObject reticulePrefab;
    public AudioClip waterSound;
    public AudioClip digSound;
    public AudioClip digSoundFail;
    private GameObject reticule;
    private GameObject actionTile;
    private Vector3 moveDirection;
    private float gravity = -20.0f;
    private float verticalSpeed = 0.0f;
    private CollisionFlags collisionFlags;

    void Awake ()
    {
        moveDirection = transform.TransformDirection (Vector3.forward);
    }

    void Start ()
    {
        SpawnReticule ();
    }
 
    void Update ()
    {
        ApplyGravity ();

        Move ();
     
        TryHoe ();
        TryPlanting ();
        TryPicking ();
        TryWatering ();
        TryDebugs ();
    }
 
    private void LateUpdate ()
    {
        if (reticule != null)
            SnapReticuleToActionTile ();
    }

    // Snaps the Player's reticule to the current action tile
    private void SnapReticuleToActionTile ()
    {
        GameObject tile = GetActionTile ();
        actionTile = tile;
        if (tile != null) {
            if (!reticule.activeInHierarchy)
                reticule.SetActive (true);
            SnapReticuleToTarget (actionTile);
        } else {
            if (reticule.activeInHierarchy)
                reticule.SetActive (false);
        }
    }
 
    // Snaps the reticule's position and rotation to match a target
    private void SnapReticuleToTarget (GameObject target)
    {
        reticule.transform.position = target.transform.position;
        reticule.transform.rotation = target.transform.rotation;
     
        // Might need to change YOffset based on what is selected.
        const float targetYOffset = 0.25f;
        reticule.transform.position = target.transform.position + Vector3.up * targetYOffset;
    }
 
    // Spawns a reticule using the reticule Prefab
    private void SpawnReticule ()
    {
        reticule = (GameObject)Instantiate (reticulePrefab, Vector3.zero, Quaternion.identity);
        reticule.transform.parent = transform;
    }

    /*
     * Sets vertical speed to the expected value based on whether or not the Player is grounded.
     */
    private void ApplyGravity ()
    {
        if (IsGrounded ()) {
            verticalSpeed = 0.0f;
        } else {
            verticalSpeed += gravity * Time.deltaTime;
        }
    }

    /*
     * Checks to see if the Player is grounded by checking collision flags.
     */
    private bool IsGrounded ()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    /*
     * Apply movement in the Player's desired directions according to the various speed
     * and movement variables.
     */
    void Move ()
    {
        // Get input values
        float horizontal = Input.GetAxisRaw ("Horizontal");
        float vertical = Input.GetAxisRaw ("Vertical");

        // Determine move direction from target values
        float targetSpeed = 0.0f;
        Vector3 targetDirection = new Vector3 (horizontal, 0.0f, vertical);
        if (targetDirection != Vector3.zero) {
            moveDirection = Vector3.RotateTowards (moveDirection, targetDirection, Mathf.Infinity, 1000);
            moveDirection = moveDirection.normalized;
         
            targetSpeed = movespeed; 
        }

        // Get movement vector
        Vector3 movement = (moveDirection * targetSpeed) + new Vector3 (0.0f, verticalSpeed, 0.0f);
        movement *= Time.deltaTime;

        // Apply movement vector
        CharacterController biped = GetComponent<CharacterController> ();
        collisionFlags = biped.Move (movement);
     
        // Rotate to face the direction of movement immediately
        if (moveDirection != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation (moveDirection);
        }
    }    
 
    /*
     * Attempts to hoe the action tile
     */
    void TryHoe ()
    {
        bool isFire1 = Input.GetButtonDown ("Weapon1");
        if (isFire1) {
            if (actionTile != null) {
                GroundTile tile = (GroundTile)actionTile.GetComponent<GroundTile> ();
                tile.Hoe ();
             
                AudioSource.PlayClipAtPoint (digSound, transform.position);
            } else {
                AudioSource.PlayClipAtPoint (digSoundFail, transform.position);
            }
             
        }
    }
 
    /*
     * Check if the user tried planting and if so, check that location is
     * a valid place to plant. Then plant it and handle inventory changes.
     */
    void TryPlanting ()
    {
        bool isFire2 = Input.GetButtonDown ("Item");
        if (isFire2) {
            if (actionTile != null) {
                GroundTile tile = (GroundTile)actionTile.GetComponent<GroundTile> ();
                if (tile.isSoil ()) {
                    Inventory inventory = (Inventory)GetComponent<Inventory> ();
                    if (inventory.HasItem (ItemDatabase.RADISH_SEEDS) &&
                            inventory.RemoveItem (ItemDatabase.RADISH_SEEDS, 1)) {
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
    void TryPicking ()
    {
        bool isFire2 = Input.GetButtonDown ("Action");
        if (isFire2) {
            if (actionTile != null) {
                GroundTile tile = (GroundTile)actionTile.GetComponent<GroundTile> ();
                Plant plant = tile.getPlant ();
                if (plant != null && plant.isRipe ()) {
                    Inventory inventory = (Inventory)GetComponent<Inventory> ();
                    tile.Pick ();
                    inventory.AddItem (ItemDatabase.RADISH, 1);
                }
            }
        }
    }
 
    /*
     * If tile has a plant and player isn't out of water, water it.
     */
    void TryWatering ()
    {
        bool isFire3 = Input.GetButtonDown ("Weapon2");
        if (isFire3) {
            if (actionTile != null) {
                GroundTile tile = (GroundTile)actionTile.GetComponent<GroundTile> ();
                Plant plant = tile.getPlant ();
                if (plant != null) {
                    AudioSource.PlayClipAtPoint (waterSound, transform.position);
                    plant.Water ();
                }
            }
        }
    }

    void TryDebugs ()
    {
        if (Input.GetKeyDown ("z")) {
            WorldTime worldtime = (WorldTime)GameObject.FindGameObjectWithTag ("WorldTime").GetComponent<WorldTime> ();
            worldtime.GoToNextDay ();
        }
        if (Input.GetKeyDown ("i")) {
            Shop shop = (Shop)GameObject.FindGameObjectWithTag ("Shop").GetComponent<Shop> ();
            shop.StartBuying();
        }
    }
 
    GameObject GetActionTile ()
    {
        // The Player tries to act on the tile in front of him - assumes no tiles overlap in Y
        float zOffset = 1.0f;
        Vector3 actionOffset = new Vector3 (0.0f, 0.0f, zOffset);
        Vector3 actionPosition = transform.position + transform.forward * actionOffset.magnitude;
     
        // Should use a constant for a grid size...
        const float TILESIZE_HALF = 1.0f / 2;
     
        // From all tiles, find the that our action position overlaps
        GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Tile");
        GameObject actionTile = null;
        foreach (GameObject tile in tiles) {
            if (Mathf.Abs ((actionPosition.x - tile.transform.position.x)) < TILESIZE_HALF &&
             Mathf.Abs ((actionPosition.z - tile.transform.position.z)) < TILESIZE_HALF) {
                actionTile = tile;
            }
        }

        return actionTile;
    }
}