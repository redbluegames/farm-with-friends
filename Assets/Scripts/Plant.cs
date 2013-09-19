using UnityEngine;
using System.Collections;
using System;

/**
 * Handles generic plant logic for growing, withering, watering,
 * and killing plants.
 */
public class Plant : MonoBehaviour {
	
	private int nightsOld;
	private int nightsSinceGrowth;
	private bool neverWatered;
	private PlantStates plantState;
	
	private const int MIN_LIFE = 0;
	
	public int nightsPerGrowth;
	public int maxLife;	
	private int curLife;
	
	public Material seedMat;
	public Material sproutMat;
	public Material adultMat;
	public Material witheredMat;
	
	public enum PlantStates
	{
		Seed = 0,
		Sprout = 1,
		Adult = 2,
		Withered = 3
	}
	
	// Use this for initialization, catch unset public vars.
	void Start ()
	{
		nightsOld = 0;
		nightsSinceGrowth = 0;
		curLife = maxLife;
		if (nightsPerGrowth == 0)
		{
			Debug.LogError("nightsPerGrowth is 0 which is invalid. Setting to 1.");
			nightsPerGrowth = 1;
		}
		if (maxLife == 0)
		{
			Debug.LogError("maxLife is 0 which is invalid. Setting to 1.");
			maxLife = 1;
		}
		neverWatered = true;
		FlagAsDry();
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
		neverWatered = false;
		curLife = maxLife;
		light.enabled = true;
	}
	
	/**
	 * Remove the water effect and mark the plant as unwatered.
	 */
	public void FlagAsDry() 
	{
		light.enabled = false;
	}
	
	/**
	 * Perform our nightly aging of the plant. All logic for withering,
	 * growing, and aging the plant should go here.
	 */
	public void NightlyUpdate()
	{		
		// Tick down plant health and warn user if health low
		curLife--;			
		if(curLife < MIN_LIFE) {
			Wither();
		} else if (curLife == MIN_LIFE) {
			FlagAsDry();
		}
		
		nightsSinceGrowth++;		
		// If it was watered recently enough and it's old enough for the stage, grow.
		if(nightsSinceGrowth >= nightsPerGrowth && !isWithered())
		{
			Grow();
		}
		
		nightsOld++;
		RenderPlantState();
	}
	
	/**
	 * Put the plant in it's next stage and reset the days since last growth.
	 */
	private void Grow()
	{	
		// Plants that have never been watered can't grow
		if (neverWatered)
		{
			Debug.Log(String.Format("COULD NOT GROW ({0}): Never watered.", name));
			return;
		}
		if((int) plantState < Enum.GetValues(typeof(PlantStates)).Length-1)
		{
			Debug.Log(String.Format("GROWING ({0})): DaysSinceGrowth ({1}) GrowthSpeed ({2}) new PlantState ({3})", 
				name, nightsSinceGrowth, nightsPerGrowth, (int) plantState+1));
			nightsSinceGrowth = 0;
			plantState++;
		}
	}
	
	/**
	 * Set the plant to withered state if not already.
	 */
	private void Wither()
	{
		if(!isWithered())
		{
			Debug.Log(String.Format("WITHERING ({0}): CurLife reached {1}.", name, curLife));
			plantState = PlantStates.Withered;
		}
	}

	/**
	 * Return if the plant is withered.
	 */
	private bool isWithered() {
		return plantState == PlantStates.Withered;
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
		else if(plantState == PlantStates.Withered)
		{
			renderer.material = witheredMat;
		}
	}
}