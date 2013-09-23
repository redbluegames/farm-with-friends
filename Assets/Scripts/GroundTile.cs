using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour {
	
	public Material dirtMaterial;
	public Material grassMaterial;
	public Material soilMaterial;
	public GameObject dirtFXPrefab;
	
	public enum GroundState
	{
		Dirt,
		Grass,
		Soil
	}
	
	private GroundState state;
	
	// Use this for initialization
	void Start () {
		float rand = Random.Range(0.0f, 1.0f);
		
		if(rand < 0.75f)
		{
			SetState (GroundState.Grass);
		}
		else
		{
			SetState (GroundState.Dirt);
		}
	}
	
	public void SetState(GroundState newState)
	{
		if(newState == GroundState.Dirt)
		{
			renderer.material = dirtMaterial;
		}
		else if(newState == GroundState.Grass)
		{
			renderer.material = grassMaterial;
		}
		else if(newState == GroundState.Soil)
		{
			renderer.material = soilMaterial;
		}
		state = newState;
	}
	
	/*
	 * Hoe the tile, changing states as appropriate
	*/
	public void Hoe()
	{
		switch (state)
		{
		case GroundState.Dirt :
			SetState( GroundState.Soil);
			break;
		case GroundState.Grass :
			SetState( GroundState.Dirt);
			break;
		case GroundState.Soil :
			break;
		}
		
		SpawnDirtFX();
	}
	
	/*
	 * Spawns dirt fx with default orientation
	*/
	private void SpawnDirtFX()
	{
		GameObject fx = (GameObject) Instantiate (dirtFXPrefab, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.back));
		Destroy (fx, 2.0f);
	}
}
