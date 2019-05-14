﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// Scripts
	public Player Player;

	// GameObjects
	public GameObject player;

	// Global Variables
	private float direction;
	private float speed;
	private float moveXPosition;

	// Use this for initialization
	void Start () {
		//player = GameObject.FindWithTag ("Player");
		speed = 17f;
	}
	
	// Update is called once per frame
	private void Update () {
		// Move the projectile left or right.
		if(Player.shootingLeft && gameObject.transform.position.x <= player.transform.position.x){
			direction = -1;
		}
		if (Player.shootingRight && gameObject.transform.position.x >= player.transform.position.x){
			direction = 1;
		}
		moveXPosition = direction * speed * Time.deltaTime * 1;
		transform.Translate(moveXPosition, 0, 0);
		// If the projectile goes out of bounds, disable it.
		if(transform.position.x < -25 || transform.position.x > 25){
			gameObject.SetActive(false);
		}
	}

	// If the projectile hits either an enemy, wall or the ground...
	private void OnTriggerEnter2D(Collider2D Collider2D){
		if (Collider2D.gameObject.layer == LayerMask.NameToLayer ("enemy") || Collider2D.gameObject.layer == LayerMask.NameToLayer ("ground")){
			gameObject.SetActive(false);
		}
	}
}
