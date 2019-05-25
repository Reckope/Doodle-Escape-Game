using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersFace : MonoBehaviour {

	// Scripts
	public Player player;

	// GameObjects
	public GameObject leftArm;
	public GameObject rightArm;

	// Components
	private Animator anim;

	Quaternion rotation;
	private string AnimTask;

	// Use this for initialization
	void Start () {
		rotation = transform.rotation;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = rotation;
		if(player.movingLeft && !player.shooting){
			anim.SetTrigger("LookLeft");
			AnimTask = "Looking Left";
		}
		else if(player.movingRight && !player.shooting){
			anim.SetTrigger("Idle");
			AnimTask = "Idle";
		}
		else if(player.shootingLeft){
			anim.SetTrigger("ShootLeft");
			AnimTask = "Shooting Left";
		}
		else if(player.shootingRight){
			anim.SetTrigger("ShootRight");
			AnimTask = "Shooting Right";
		}
		else{
			anim.SetTrigger("Idle");
			AnimTask = "Not playing any anims";
		}
		//Debug.Log(AnimTask);
	}
}
