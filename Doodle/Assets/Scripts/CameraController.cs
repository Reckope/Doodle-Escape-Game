using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// GameObjects
	public Transform player;
	Camera Camera;

	const float FOLLOW_PLAYER_DAMP_TIME = 0.15f;
	const float CAMERA_MIN_X_BOUNDS = -22.7f;
	const float CAMERA_MAX_X_BOUNDS = 22.7f;
	const float CAMERA_MIN_Y_BOUNDS = -13.6f;
	const float CAMERA_MAX_Y_BOUNDS = 13.6f;
	const float CAMERA_DELTA_X_POSITION = 0.5f;
	const float CAMERA_DELTA_Y_POSITION = 0.5f;
	const float CAMERA_DISTANCE_ABOVE_PLAYER = 1f;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		Camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (player != null){
			FollowPlayer();
		}
	}

	private void FollowPlayer(){
			Vector3 point = Camera.WorldToViewportPoint(player.position);
			Vector3 delta = player.position - Camera.ViewportToWorldPoint(new Vector3(CAMERA_DELTA_X_POSITION, CAMERA_DELTA_Y_POSITION, point.z));
			Vector3 destination = transform.position + delta;
			destination.x = Mathf.Clamp (destination.x, CAMERA_MIN_X_BOUNDS, CAMERA_MAX_X_BOUNDS);
			destination.y = Mathf.Clamp (destination.y + CAMERA_DISTANCE_ABOVE_PLAYER, CAMERA_MIN_Y_BOUNDS, CAMERA_MAX_Y_BOUNDS);
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, FOLLOW_PLAYER_DAMP_TIME);
	}
}
