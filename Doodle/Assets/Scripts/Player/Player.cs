/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * Attach this to the player gameobject. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Classes
	ProjectilePool ProjectilePool;
	
	// Components
	private Rigidbody2D rb2d;
	Collider2D Collider2D;

	// GameObjects
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Global Variables
	const int SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER = 1;
	const int IDLE_VALUE = 0;
	public static bool isDead;
	public enum PlayerShootDir {left, notShooting, right};
	public enum PlayerMoveDir {left, idle, right};
	[SerializeField]
	private float jumpForce, acceleration, maxSpeed, groundCheckRadius;
	private float nextFire, fireRate;

	// Objects
	public PlayerShootDir shooting;
	public PlayerMoveDir moving;

	// ---------------------------------------------------------------------------------
	void Start () {
		Collider2D = GetComponent<Collider2D>();
		rb2d = GetComponent<Rigidbody2D>();
		ProjectilePool = GetComponent<ProjectilePool>();
		nextFire = 0.0f;
		fireRate = 0.14f;
		isDead = false;
		Collider2D.enabled = true;
		rb2d.constraints = RigidbodyConstraints2D.None;
		shooting = PlayerShootDir.notShooting;
		moving = PlayerMoveDir.idle;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isDead){
			MovePlayer();
			Shoot();
		}
	}

	// --------- Player Input ---------
	// Key controls to move the player
	private void MovePlayer(){
		bool jump;
		bool grounded;
		float moveHorizontal;
		float airDrag = 3f;

		// Move the main character.
		rb2d.bodyType = RigidbodyType2D.Dynamic;
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		Vector2 movement = new Vector2 (moveHorizontal, IDLE_VALUE);
		if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed && grounded)){
			rb2d.AddForce(movement * acceleration);
		}
		else if(!grounded){
			if((rb2d.velocity.x < -maxSpeed || rb2d.velocity.x > maxSpeed)){
				airDrag = 15f;
			}
			else{
				airDrag = 3f;
			}
			rb2d.AddForce(movement * (acceleration / airDrag));
		}

		if(moveHorizontal < IDLE_VALUE){
			moving = PlayerMoveDir.left;
		}
		else if(moveHorizontal > IDLE_VALUE){
			moving = PlayerMoveDir.right;
		}
		else{
			moving = PlayerMoveDir.idle;
		}

		// Jump around.
		jump = Input.GetKeyDown(KeyCode.W);
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
			if(shoot < IDLE_VALUE){
				shooting = PlayerShootDir.left;
			}
			else if(shoot > IDLE_VALUE){
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
		float spawnProjectile = 0;

		GameObject projectile = ProjectilePool.GetPooledProjectile();
		if (projectile != null){
			if(shooting == PlayerShootDir.left){
				spawnProjectile = spawnProjectileLeft;
			}
			else if(shooting == PlayerShootDir.right){
				spawnProjectile = spawnProjectileRight;
			}
			projectile.transform.position = new Vector2(spawnProjectile, this.gameObject.transform.position.y);
			projectile.SetActive(true);
		}
	}
	// ---------- End of Input ----------

	// If i'm on a moving platform, adjust my position so I stay on it. 
	public void PlayerMoveWithPlatform(GameObject platformName, Vector3 offset){
		transform.position = platformName.transform.position + offset;
	}

	private void PlayerDie(){
		Collider2D.enabled = false;
		rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
		isDead = true;
	}

	// Float down whilst in transition. 
	public void PlayerEscapeTransition(){
		float transitionDirection = -1f;
		float transitionSpeed = 4.5f;
		int noChangeInPosition = 0;
		var rotation = transform.rotation.eulerAngles;
		rotation.z = 0;

		rb2d.bodyType = RigidbodyType2D.Static;
		transform.rotation = Quaternion.Euler(rotation);
		transform.Translate(noChangeInPosition, transitionDirection * transitionSpeed * Time.deltaTime, noChangeInPosition);
	}

	public void PlayerEndTransition(){
		rb2d.bodyType = RigidbodyType2D.Dynamic;
	}

	public void PlayerCompleteGame(){
		Destroy(gameObject);
	}

	// ---------- Triggers and Collisions ----------

	// When the player collides with something
	private void OnCollisionEnter2D(Collision2D Col){
		int causeOfDeath;

		// Select cause of death once the enemy hits me.
		if(Col.gameObject.tag == ("Enemy")){
			causeOfDeath = GameController.instance.FindDeathReason(Col);
			PlayerDie();
			//Debug.Log("CAUSE OF DEATH: " + causeOfDeath);
			GameController.instance.GameOver(causeOfDeath);
		}
	}
}
