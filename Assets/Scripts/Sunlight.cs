using UnityEngine;
using System.Collections;

public class Sunlight : MonoBehaviour {
	
	void Start () {
		// Initialize the position for the sun
		transform.rotation = Quaternion.identity;
		transform.Rotate (new Vector3(60, 300, 30), Space.World);
	}
	
	/*
	 * Sets the sun position based on the supplied hour.
	*/
	public void SetSunToTimeOfDay( float hour )
	{
		float MAX_INTENSITY = 0.5f;
		float highNoon = WorldTime.GAMEHOURS_PER_GAMEDAY / 2;
		
		Light theLight = GetComponent<Light>();
		
		// Sun goes down after high noon
		if(hour > highNoon)
		{
			theLight.light.intensity = MAX_INTENSITY * (1 - ((hour - highNoon) / highNoon));
		}
		else
		{
			theLight.light.intensity = MAX_INTENSITY * (hour / highNoon);
		}
	}
}
