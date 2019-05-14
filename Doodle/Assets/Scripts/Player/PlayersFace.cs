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
		}
		else if(player.movingRight && !player.shooting){
			anim.SetTrigger("Idle");
		}
		else if(player.shootingLeft){
			anim.SetTrigger("ShootLeft");
		}
		else if(player.shootingRight){
			anim.SetTrigger("ShootRight");
		}
		else{
			anim.SetTrigger("Idle");
		}
	}
}
