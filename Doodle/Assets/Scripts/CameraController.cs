using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// GameObjects
	public GameObject player;
	private Vector3 offset;
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		pos.x = Mathf.Clamp (pos.x, -22.7f, 22.7f);
		pos.y = Mathf.Clamp (pos.y, -13.6f, 13.6f);
		transform.position = pos;
	}
}
