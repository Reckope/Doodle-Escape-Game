/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * Attach this to the SpikeBall object to set it's speed / tasks. .  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

	// Global Variables
	[SerializeField]
	private float rotationSpeed;
	private float stationary = 0;

	// ---------------------------------------------------------------------------------
	void Start () {
		if (rotationSpeed == stationary){
			rotationSpeed = 2f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(stationary, stationary, rotationSpeed);
	}
}
