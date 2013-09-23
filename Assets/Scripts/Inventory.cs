using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    private int numRadishes;
    
	void Start () {
        numRadishes = 0;
	}
	
	// Stub method for testing 
	void Update ()
	{
		if(Input.GetKeyDown("p"))
		{
			addRadish();
		}
	}
	
    public void addRadish()
    {
        numRadishes++;
    }

    public void removeRadish()
    {
        numRadishes--;
    }

    public int getRadishCount()
    {
        return numRadishes;
    }
}
