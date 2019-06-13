/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * Attach this to the Key gameobject. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	// Components
	Collider2D Collider2D;

	// Global Variables
	private string keyColour;

	// ---------------------------------------------------------------------------------
	void Start () {
		keyColour = this.gameObject.tag;
		Collider2D = GetComponent<Collider2D>();
		Collider2D.enabled = true;
	}

	// When this key is collected by the player
	private void OnTriggerEnter2D (Collider2D collide){
		if (collide.gameObject.tag == ("Player")) {
			GameController.instance.CollectedKey(keyColour);
			Collider2D.enabled = false;
		}
	}
}
