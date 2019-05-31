/* Author: Joe Davis
 * Project: Doodle Escape.
 * Code QA Sweep: DONE - 31/05/19
 */

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

	// Global Variables
	Quaternion rotation;
	private string AnimTask;

	// ---------------------------------------------------------------------------------
	void Start () {
		anim = GetComponent<Animator>();
		rotation = transform.rotation;
	}
	
	void Update () {
		transform.rotation = rotation;
		MovingShootingAnims();
	}

	// Use a two-dimentional switch combined with enum values to determine
	// which animations to play. 
	private void MovingShootingAnims(){
		switch(player.shooting){
			case Player.PlayerShootDir.notShooting:
				switch(player.moving){
					case Player.PlayerMoveDir.left:
					anim.SetTrigger("LookLeft");
					AnimTask = "Looking Left";
					break;
					case Player.PlayerMoveDir.right:
					anim.SetTrigger("Idle");
					AnimTask = "Idle";
					break;
					case Player.PlayerMoveDir.idle:
					anim.SetTrigger("Idle");
					AnimTask = "Idle";
					break;
				}
			break;
			case Player.PlayerShootDir.left:
			anim.SetTrigger("ShootLeft");
			AnimTask = "Shooting Left";
			break;
			case Player.PlayerShootDir.right:
			anim.SetTrigger("ShootRight");
			AnimTask = "Shooting Right";
			break;
			default:
			anim.SetTrigger("Idle");
			AnimTask = "Not playing any anims";
			break;
		}
		//Debug.Log(AnimTask);
	}
}
