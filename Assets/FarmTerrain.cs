using UnityEngine;
using System.Collections;

public class FarmTerrain : MonoBehaviour {
	
	public GameObject groundTilePrefab;
	
	void Start () {
		SpawnGroundTiles();
	}
	
	/*
	 * Spawn ground tiles all over this terrain
	*/
	private void SpawnGroundTiles()
	{
		// Get size of the terrain
		float TERRAIN_X = collider.bounds.extents.x * 2;
		float TERRAIN_Z = collider.bounds.extents.z * 2;
		
		// Start spawning from the bottom left corner, moving up and out until we reach the top right
		Vector3 bottomLeftCorner = transform.position - new Vector3(TERRAIN_X / 2, 0, TERRAIN_Z / 2);
		float NUM_ROWS = (TERRAIN_X) / GroundTile.SIZE;
		float NUM_COLS = (TERRAIN_Z) / GroundTile.SIZE;
		// Tile edges should align with the terrain, not their centers, so we must shift them by their "radius"
		Vector3 tileOffset = new Vector3(GroundTile.SIZE / 2.0f, 0, GroundTile.SIZE / 2.0f);
		for(int row = 0; row < NUM_ROWS; row++)
		{
			for(int col = 0; col < NUM_COLS; col++)
			{
				// Spawn the tile and parent it to the FarmTerrain for grouping purposes
				GameObject tile = (GameObject) Instantiate(groundTilePrefab, 
					bottomLeftCorner + tileOffset + new Vector3(row * GroundTile.SIZE, 0, col * GroundTile.SIZE), 
					Quaternion.identity);
				tile.transform.parent = transform;
			}
		}
	}
}
