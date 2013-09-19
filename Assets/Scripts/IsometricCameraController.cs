using UnityEngine;
using System.Collections;

public class IsometricCameraController : MonoBehaviour {
	
	public GameObject target;
	
	private Vector3 offset;
	
	void Start () {
		offset = transform.position;
	}
	
	void LateUpdate () {
		transform.position = target.transform.position + offset;	
	}
}
