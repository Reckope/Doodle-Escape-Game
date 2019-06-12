using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	Collider2D Collider2D;
	private string keyColour;

	// Use this for initialization
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
