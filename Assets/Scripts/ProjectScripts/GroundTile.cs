using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour
{
    // Materials and FX
    public Material dirtMaterial;
    public Material grassMaterial;
    public Material soilMaterial;
    public GameObject dirtFXPrefab;

    // The plant currently planted on this ground tile
    GameObject curPlant;

    // Size of each game tile. In a robust grid system, this would be a global constant
    static public float SIZE_XZ = 1.0f;
    static public float SIZE_Y = 0.1f;

    // All possible states for a ground tile to be
    public enum GroundState
    {
        Dirt,
        Grass,
        Soil,
        Planted
    }

    // The current state of the ground tile
    private GroundState curState = GroundState.Dirt;
 
    void Start ()
    {
        SetState (curState);
    }
 
    public void SetState (GroundState newState)
    {
        if (newState == GroundState.Dirt) {
            renderer.material = dirtMaterial;
        } else if (newState == GroundState.Grass) {
            renderer.material = grassMaterial;
        } else if (newState == GroundState.Soil) {
            renderer.material = soilMaterial;
        }
        curState = newState;
    }
 
    /*
  * Hoe the tile, changing states as appropriate
 */
    public void Hoe ()
    {
        switch (curState) {
        case GroundState.Dirt :
            SetState (GroundState.Soil);
            break;
        case GroundState.Grass :
            SetState (GroundState.Dirt);
            break;
        case GroundState.Soil :
            break;
        case GroundState.Planted :
            SetState (GroundState.Soil);
            DestroyPlant ();
            break;
        }
     
        SpawnDirtFX ();
    }
 
    /*
  * Pick the plant by setting the statuses appropriately
  * and destroying the object.
  */
    public void Pick ()
    {
        SetState (GroundState.Soil);
        DestroyPlant ();
    }
 
    /*
  * Plant a plant on the tile.
  */
    public void Plant (GameObject plantPrefab)
    {
        SetState (GroundState.Planted);
        SpawnPlant (plantPrefab);
    }
 
    /*
  * Remove the plant from the tile.
  */
    public void DestroyPlant ()
    {
        Destroy (curPlant);
    }
 
    /*
  * Return the instantiated plant.
  */
    public Plant getPlant ()
    {
        if (curPlant == null) {
            return null;
        }
        return curPlant.GetComponent<Plant> ();
    }

    /*
  * Return whether the ground state is currently soil.
 */
    public bool isSoil ()
    {
        return curState == GroundState.Soil;
    }
 
    /*
  * Spawn a plant on the tile.
 */
    private void SpawnPlant (GameObject plantPrefab)
    {
        curPlant = (GameObject)Instantiate (plantPrefab, transform.position,
            transform.rotation);

        // Offset the plant up so that it sits on top of the ground, not embedded in it.
        float halfPlantHeight = curPlant.transform.lossyScale.y / 2;
        float heightOffset = (SIZE_Y / 2 ) + halfPlantHeight;
        curPlant.transform.position = transform.position + new Vector3( 0, heightOffset, 0);
    }
 
    /*
  * Spawns dirt fx with default orientation
 */
    private void SpawnDirtFX ()
    {
        GameObject fx = (GameObject)Instantiate (dirtFXPrefab, transform.position,
            Quaternion.LookRotation (Vector3.up, Vector3.back));
        Destroy (fx, 2.0f);
    }
}
