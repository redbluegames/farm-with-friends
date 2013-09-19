using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float movespeed;
	
	private float turnSmoothing = 90f;
	
	void FixedUpdate () {
		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis ("Vertical");
	
		if(movementHorizontal != 0f || movementVertical != 0f)
		{
			Rotate(movementHorizontal, movementVertical);
		}
		
		Move(movementHorizontal, movementVertical);
	}
	
	void Move(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
		Vector3 movementDelta = (targetDirection * movespeed * Time.deltaTime);
		
		rigidbody.MovePosition(rigidbody.position + movementDelta);
	}
	
	void Rotate(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
		Quaternion desiredRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, desiredRotation, turnSmoothing * Time.deltaTime);
        
		rigidbody.MoveRotation(newRotation);
	}
}
