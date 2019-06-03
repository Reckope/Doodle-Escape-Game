/* Author: Joe Davis
 * Project: Doodle Escape.
 * Code QA Sweep: DONE - 31/05/19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Scripts
	ProjectilePool ProjectilePool;
	
	// Components
	private Rigidbody2D rb2d;
	Collider2D Collider2D;

	// GameObjects
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Global Variables
	const int SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER = 1;
	public enum PlayerShootDir {left, notShooting, right};
	public enum PlayerMoveDir {left, idle, right};
	[SerializeField]
	private float jumpForce, acceleration, maxSpeed, groundCheckRadius;
	private float nextFire, fireRate;

	public PlayerShootDir shooting;
	public PlayerMoveDir moving;


	// ---------------------------------------------------------------------------------
	void Start () {
		Collider2D = GetComponent<Collider2D>();
		rb2d = GetComponent<Rigidbody2D>();
		ProjectilePool = GetComponent<ProjectilePool>();
		nextFire = 0.0f;
		fireRate = 0.15f;
		Collider2D.enabled = true;
		rb2d.constraints = RigidbodyConstraints2D.None;
		shooting = PlayerShootDir.notShooting;
		moving = PlayerMoveDir.idle;
	}
	
	// Update is called once per frame
	void Update () {
		MovePlayer();
		Shoot();
	}

	// --------- Player Input ---------
	// Key controls to move the player
	private void MovePlayer(){
		bool jump;
		bool grounded;
		float moveHorizontal;

		// Move the main character.
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		Vector2 movement = new Vector2 (moveHorizontal, 0);
		if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed && grounded)){
			rb2d.AddForce(movement * acceleration);
		}
		else if(!grounded){
			rb2d.AddForce(movement * (acceleration / 3f));
			if(rb2d.velocity.x < -maxSpeed || rb2d.velocity.x > maxSpeed){
				rb2d.AddForce(-movement * acceleration);
			}
		}

		if(moveHorizontal < 0){
			moving = PlayerMoveDir.left;
		}
		else if(moveHorizontal > 0){
			moving = PlayerMoveDir.right;
		}
		else{
			moving = PlayerMoveDir.idle;
		}

		// Jump around.
		jump = Input.GetKeyDown (KeyCode.W);
		if(jump){
			if(grounded){
				rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
		}
		//Debug.Log("velocity: " + rb2d.velocity.x);
	}

	// Kill those enemies!
	private void Shoot(){
		float shoot;

		shoot = Input.GetAxisRaw("Shoot");
		if(Time.time > nextFire){
			if(shoot < 0){
				shooting = PlayerShootDir.left;
			}
			else if(shoot > 0){
				shooting = PlayerShootDir.right;
			}
			else{
				shooting = PlayerShootDir.notShooting;
			}

			if(shooting == PlayerShootDir.left || shooting == PlayerShootDir.right){
				nextFire = Time.time + fireRate;
				SpawnProjectile();
			}
		}
	}

	// Spawn the bullets
	private void SpawnProjectile(){
		float spawnProjectileLeft = (this.gameObject.transform.position.x - SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER);
		float spawnProjectileRight = (this.gameObject.transform.position.x + SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER);

		GameObject projectile = ProjectilePool.GetPooledProjectile();
		if (projectile != null){
			if(shooting == PlayerShootDir.left){
				projectile.transform.position = new Vector2(spawnProjectileLeft, this.gameObject.transform.position.y);
			}
			else if(shooting == PlayerShootDir.right){
				projectile.transform.position = new Vector2(spawnProjectileRight, this.gameObject.transform.position.y);
			}
			projectile.SetActive(true);
		}
	}
	// ---------- End of Input ----------

	// If i'm on a moving platform, adjust my position so I stay on it. 
	public void PlayerMoveWithPlatform(GameObject platformName, Vector3 offset){
		transform.position = platformName.transform.position + offset;
	}

	private void PlayerDie(int causeOfDeath){
		// anim.SetTrigger(causeOfDeath);
		// sound.Play(causeOfDeath);
		Collider2D.enabled = false;
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
		rb2d.velocity = (new Vector2 (0, 11f));
	}

	// ***** Triggers and Collisions *****

	// When the player collides with something
	private void OnCollisionEnter2D(Collision2D Col){
		int causeOfDeath;

		// Select cause of death once the enemy hits me.
		if(Col.gameObject.tag == ("Enemy")){
			causeOfDeath = GameController.instance.FindDeathReason(Col);
			PlayerDie(causeOfDeath);
			//Debug.Log("CAUSE OF DEATH: " + causeOfDeath);
			GameController.instance.GameOver(causeOfDeath);
		}
	}
}
