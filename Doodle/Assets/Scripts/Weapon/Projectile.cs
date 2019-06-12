using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// GameObjects
	GameObject player;
	GameObject ground;

	// Global Variables
	private float direction;
	private float speed;
	private float moveXPosition;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		ground = GameObject.FindWithTag ("Ground");
		speed = 15f;
	}
	
	// Update is called once per frame
	private void Update () {
		if(player != null){
			// Move the projectile left or right.
			if(gameObject.transform.position.x <= player.transform.position.x){
				direction = -1;
			}
			if (gameObject.transform.position.x >= player.transform.position.x){
				direction = 1;
			}
			moveXPosition = direction * speed * Time.deltaTime;
			transform.Translate(moveXPosition, 0, 0);
		}
	}

	// If the projectile hits either an enemy, wall or the ground...
	private void OnTriggerEnter2D(Collider2D Collider2D){
		if (Collider2D.gameObject.tag == "Enemy" || Collider2D.gameObject.layer == LayerMask.NameToLayer ("Ground")){
			gameObject.SetActive(false);
		}
	}
}
