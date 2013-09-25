using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour
{
    public Material dirtMaterial;
    public Material grassMaterial;
    public Material soilMaterial;
    public GameObject dirtFXPrefab;
    public GameObject plantPrefab;
    private GameObject curPlant;
    static public float SIZE = 1.0f;
 
    public enum GroundState
    {
        Dirt,
        Grass,
        Soil,
        Planted
    }
 
    private GroundState state = GroundState.Dirt;
 
    void Start ()
    {
        SetState (state);
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
        state = newState;
    }
 
    /*
  * Hoe the tile, changing states as appropriate
 */
    public void Hoe ()
    {
        switch (state) {
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
    public void Plant ()
    {
        SetState (GroundState.Planted);
        SpawnPlant ();
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
        return state == GroundState.Soil;
    }
 
    /*
  * Spawn a plant on the tile.
 */
    private void SpawnPlant ()
    {
        curPlant = (GameObject)Instantiate (plantPrefab, transform.position, Quaternion.LookRotation (Vector3.up, Vector3.back));
    }
 
    /*
  * Spawns dirt fx with default orientation
 */
    private void SpawnDirtFX ()
    {
        GameObject fx = (GameObject)Instantiate (dirtFXPrefab, transform.position, Quaternion.LookRotation (Vector3.up, Vector3.back));
        Destroy (fx, 2.0f);
    }
}
