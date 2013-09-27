using UnityEngine;
using System.Collections;

public class Trigger_Sleep : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        Vector3 directionToOther = Vector3.forward;
        if(other.tag == "Player")
        {
        //    Vector3.RotateTowards(directionToOther, other.transform.position, Mathf.Infinity, Mathf.Infinity);
         //   if(directionToOther.y > 0)
         //   {
         //       Debug.Log ("entering in front");
          //  }
            WorldTime worldtime = (WorldTime)GameObject.FindGameObjectWithTag ("WorldTime").GetComponent<WorldTime> ();
            worldtime.GoToNextDay ();
        }
    }
}
