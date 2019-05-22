using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// GameObjects
	public Transform player;
	private Vector3 pos;

	public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		//var pos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (player)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(player.position);
			Vector3 delta = player.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			destination.x = Mathf.Clamp (destination.x, -22.7f, 22.7f);
			destination.y = Mathf.Clamp (destination.y + 1f, -13.6f, 13.6f);
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
		//var pos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, transform.position.z);
		//pos.x = Mathf.Clamp (pos.x, -22.7f, 22.7f);
		//pos.y = Mathf.Clamp (pos.y, -13.6f, 13.6f);
		//transform.position = pos;
	}
}
