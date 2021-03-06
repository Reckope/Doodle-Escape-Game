﻿/* Author: Joe Davis
 * Project: Doodle Escape
 * 2019
 * Notes:
 * This is used to control all of the camera movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// GameObjects
	public Transform player;
	public Transform targetPoint;
	Camera Camera;

	// Global Variables
	const float FOLLOW_PLAYER_DAMP_TIME = 0.25f;
	const float CAMERA_ESCAPE_Y_POSITION = -25f;
	const float CAMERA_DELTA_X_POSITION = 0.5f;
	const float CAMERA_DELTA_Y_POSITION = 0.5f;
	const float CAMERA_DISTANCE_ABOVE_PLAYER = 1f;
	const float LOWER_GROUND_Y_POSITION = -26.5f;
	const float LOWER_GROUND_FINISH_POINT = 22f;
	private float cameraDistanceAheadOfPlayer;
	private float cameraMaxXBounds;
	private float cameraMinXBounds;
	private float cameraMinYBounds;
	private float cameraMaxYBounds;
	private Vector3 velocity = Vector3.zero;

	// ---------------------------------------------------------------------------------
	void Start () {
		Camera = GetComponent<Camera>();
		cameraMaxYBounds = 13.6f;
		cameraMinYBounds = -13.6f;
		cameraMinXBounds = -23.1f;
		cameraMaxXBounds = 23.1f;
		cameraDistanceAheadOfPlayer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null && !GameController.instance.inTransition){
			FollowPlayer();
		}
		else if(GameController.instance.inTransition){
			CameraEscapeTransition();
		}
	}

	// Constantly follow the player throughout the game, then stop when reaching
	// the edge of the game world. 
	private void FollowPlayer(){
		// Set position
		Vector3 point = Camera.WorldToViewportPoint(player.position);
		Vector3 delta = player.position - Camera.ViewportToWorldPoint(new Vector3(CAMERA_DELTA_X_POSITION, CAMERA_DELTA_Y_POSITION, point.z));
		Vector3 destination = transform.position + delta;
		// Set Bounds
		destination.x = Mathf.Clamp (destination.x + cameraDistanceAheadOfPlayer, cameraMinXBounds, cameraMaxXBounds);
		destination.y = Mathf.Clamp (destination.y + CAMERA_DISTANCE_ABOVE_PLAYER, cameraMinYBounds, cameraMaxYBounds);
		// Follow
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, FOLLOW_PLAYER_DAMP_TIME);
	}

	// When the player is escaping, move the camera downwards and change it's 
	// min / max bounds. 
	public void CameraEscapeTransition(){
		if(targetPoint != null){
			transform.position = Vector3.Lerp(transform.position, targetPoint.position, Time.deltaTime);
		}
		else{
			Debug.Log("ERROR: CANT FIND TARGET POINT");
		}
		cameraMinYBounds = -25f;
		cameraMinXBounds = 0f;
		cameraMaxYBounds = -25f;
		cameraMaxXBounds = 55f;
		cameraDistanceAheadOfPlayer = 2f;
	}
}
