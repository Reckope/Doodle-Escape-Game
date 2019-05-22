using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour {

	// Scripts and components
	public EditorPath pathToFollow;

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

		if (player != null) {
			player.transform.position = transform.position+offset;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == ("Player")) {
			playerOnPlatform = true;
		}
	}

	void OnTriggerStay2D(Collider2D col){
		player = col.gameObject;
		offset = player.transform.position - transform.position;
	}

	void OnTriggerExit2D(Collider2D col){
		player = null;
	}
}
