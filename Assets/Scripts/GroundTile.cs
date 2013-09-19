using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour {
	
	public Material dirtMaterial;
	public Material grassMaterial;
	public Material soilMaterial;
	
	public enum GroundState
	{
		Dirt,
		Grass,
		Soil
	}
	
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
	}
}
