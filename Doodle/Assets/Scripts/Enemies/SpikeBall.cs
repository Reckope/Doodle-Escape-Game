using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

	[SerializeField]
	private float rotationSpeed;

	// Use this for initialization
	void Start () {
		if (rotationSpeed == 0f){
			rotationSpeed = 2f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, rotationSpeed);
	}
}
