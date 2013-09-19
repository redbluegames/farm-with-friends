using UnityEngine;
using System.Collections;
using System;

/**
 * Handles generic plant logic for growing, withering, watering,
 * and killing plants.
 */
public class Plant : MonoBehaviour {
	
	private int daysOld;
	private int daysSinceGrowth;
	private int daysSinceWatered;
	private bool isWatered;
	private PlantStates plantState;
	
	// These are 0 indexed basically, i.e. 0 days to grow still requires 1 night
	public int daysToGrow;
	public int dryNightsAllowed;
	
	public Material seedMat;
	public Material sproutMat;
	public Material adultMat;
	public Material dessicatedMat;
	
	public enum PlantStates
	{
		Seed = 0,
		Sprout = 1,
		Adult = 2,
		Dessicated = 3
	}
	
	// Use this for initialization
	void Start ()
	{
		daysOld = 0;
		daysSinceGrowth = 0;
		daysSinceWatered = 0;
		RemoveWater();
	}
	
	// Stub method for testing 
	void Update ()
	{
		if(Input.GetKeyDown("x"))
		{
			Water();
		}
		if(Input.GetKeyDown("z"))
		{
			NightlyUpdate();
		}
	}
	
	/**
	 * If the plant is already watered, set it as watered and
	 * make it visually obvious.
	 */
	public void Water()
	{
		if(!isWatered)
		{
			daysSinceWatered = 0;
			isWatered = true;
			light.enabled = true;
		}		
	}
	
	/**
	 * Remove the water effect and mark the plant as unwatered.
	 */
	public void RemoveWater() 
	{
		if(isWatered)
		{
			isWatered = false;
			light.enabled = false;
		}
	}
	
	/**
	 * Perform our nightly aging of the plant. All logic for withering,
	 * growing, and aging the plant should go here.
	 */
	public void NightlyUpdate()
	{
		if(!isWatered)
		{
			daysSinceWatered++;
		}		
		// If equal to tolerance, that's okay
		if(daysSinceWatered > dryNightsAllowed)
		{
			Wither();
		}		
		// If it was watered recently enough and it's old enough for the stage, grow.
		if(daysSinceGrowth >= daysToGrow && daysSinceWatered <= dryNightsAllowed)
		{
			Grow();
		}
		else
		{		
			daysSinceGrowth++;
		}
		
		daysOld++;
		RemoveWater();
		RenderPlantState();
	}
	
	/**
	 * Put the plant in it's next stage and reset the days since last growth.
	 */
	private void Grow()
	{		
		Debug.Log(String.Format("GROWING ({0})): DaysSinceGrowth ({1}) GrowthSpeed ({2}) new PlantState ({3})", 
			name, daysSinceGrowth, daysToGrow, (int) plantState+1));
		if((int) plantState < Enum.GetValues(typeof(PlantStates)).Length-1)
		{
			daysSinceGrowth = 0;
			plantState++;
		}
	}
	
	/**
	 * Set the plant to withered state.
	 */
	private void Wither()
	{
		Debug.Log(String.Format("WITHERING ({0}): Not watered for ({1}) days.", name, daysSinceWatered));
		plantState = PlantStates.Dessicated;
	}
	
	/**
	 * Update the plant state to the appropriate material.
	 */
	private void RenderPlantState()
	{
		if(plantState == PlantStates.Seed)
		{
			renderer.material = seedMat;
		}
		else if(plantState == PlantStates.Sprout)
		{
			renderer.material = sproutMat;
		}
		else if(plantState == PlantStates.Adult)
		{
			renderer.material = adultMat;
		}
		else if(plantState == PlantStates.Dessicated)
		{
			renderer.material = dessicatedMat;
		}
	}
}