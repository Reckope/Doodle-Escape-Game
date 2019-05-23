using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	private string keyColour;

	// Use this for initialization
	void Start () {
		keyColour = this.gameObject.tag;
	}

	// When this key is collected by the player
	private void OnTriggerEnter2D (Collider2D collide){
		if (collide.gameObject.tag == ("Player")) {
			GameController.instance.CollectedKey(keyColour);
			GameController.instance.numberOfKeys--;
			GetComponent<Collider2D>().enabled = false;
		}
	}
}
