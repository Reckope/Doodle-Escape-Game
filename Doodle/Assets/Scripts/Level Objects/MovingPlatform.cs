using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	// Scripts and components
	public EditorPath pathToFollow;
	Player PlayerScript;

	// GameObjects
	Vector2 lastPosition;
	Vector2 currentPosition;
	private GameObject player;
	private Vector3 offset;
	
	// Global Variables
	private int currentWaypointID;
	private int speed;
	private float reachDistance;
	private bool playerOnPlatform;

	// Use this for initialization
	void Start () {
		PlayerScript = GameObject.FindObjectOfType(typeof(Player)) as Player;
		lastPosition = transform.position;
		currentWaypointID = 0;
		speed = 2;
		reachDistance = 1f;
		player = null;
		playerOnPlatform = false;
	}

	// Update is called once per frame
	void Update () {
		if(gameObject != null && playerOnPlatform && currentWaypointID < pathToFollow.pathObjs.Count){
			// Move the platform to the path starting point.
			float distance = Vector2.Distance(pathToFollow.pathObjs[currentWaypointID].position, transform.position);
			transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathObjs[currentWaypointID].position, speed * Time.deltaTime);
			// Move along each waypoint. 
			if(distance <= reachDistance){
				currentWaypointID++;
			}
			 //If we reach the end of the path, loop. 
			//if(currentWaypointID >= pathToFollow.pathObjs.Count){
			//	currentWaypointID = 0;
			//}
		}

		// Whilst the player is on me, I tell the player to move themselves without directly altering their position.
		if (player != null) {
			PlayerScript.PlayerMoveWithPlatform(this.gameObject, offset);
		}
	}
	// When a player lands on me, I move!
	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == ("Player")) {
			playerOnPlatform = true;
		}
	}

	private void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == ("Player")) {
			player = col.gameObject;
			offset = player.transform.position - transform.position;
		}
	}

	private void OnTriggerExit2D(Collider2D col){
		player = null;
	}
}
