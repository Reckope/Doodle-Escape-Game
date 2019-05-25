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
	public bool movingLeft, movingRight, shootingLeft, shootingRight, shooting;
	[SerializeField]
	private float jumpForce, acceleration, maxSpeed, groundCheckRadius;
	private float nextFire, fireRate;


	// Use this for initialization
	void Start () {
		Collider2D = GetComponent<Collider2D>();
		rb2d = GetComponent<Rigidbody2D>();
		ProjectilePool = GetComponent<ProjectilePool>();
		nextFire = 0.0f;
		fireRate = 0.15f;
		Collider2D.enabled = true;
		rb2d.constraints = RigidbodyConstraints2D.None;
	}
	
	// Update is called once per frame
	void Update () {
		MovePlayer();
		Shoot();
	}

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
			NotMoving();
			movingLeft = true;
		}
		else if(moveHorizontal > 0){
			NotMoving();
			movingRight = true;
		}
		else{
			NotMoving();
		}

		// Jump up, jump up and get down.
		jump = Input.GetKeyDown (KeyCode.W);
		if(jump){
			if(grounded){
				rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
		}
		//Debug.Log("velocity: " + rb2d.velocity.x);
	}

	private void NotMoving(){
		movingLeft = false;
		movingRight = false;
	}

	// Kill those enemies!
	private void Shoot(){
		float shoot;

		shoot = Input.GetAxisRaw("Shoot");
		if(Time.time > nextFire){
			if(shoot < 0){
				NotShooting();
				shootingLeft = true;
				shooting = true;
			}
			else if(shoot > 0){
				NotShooting();
				shootingRight = true;
				shooting = true;
			}
			else{
				NotShooting();
			}

			if(shooting){
				nextFire = Time.time + fireRate;
				SpawnProjectile();
			}
		}
	}

	private void NotShooting(){
		shootingRight = false;
		shootingLeft = false;
		shooting = false;
	}

	// Spawn the bullets
	private void SpawnProjectile(){
		const int SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER = 1;
		float spawnProjectileLeft = (this.gameObject.transform.position.x - SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER);
		float spawnProjectileRight = (this.gameObject.transform.position.x + SPAWN_PROJECTILE_DISTANCE_AWAY_FROM_PLAYER);

		GameObject projectile = ProjectilePool.GetPooledProjectile();
		if (projectile != null){
			if(shootingLeft){
				projectile.transform.position = new Vector2(spawnProjectileLeft, this.gameObject.transform.position.y);
			}
			else if(shootingRight){
				projectile.transform.position = new Vector2(spawnProjectileRight, this.gameObject.transform.position.y);
			}
			projectile.SetActive(true);
		}
	}

	private void PlayerDie(string causeOfDeath){
		// anim.SetTrigger(causeOfDeath);
		// sound.Play(causeOfDeath);
		Collider2D.enabled = false;
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
		rb2d.velocity = (new Vector2 (0, 11f));
	}

	// ***** Triggers and Collisions *****

	// When the player collides with something
	private void OnCollisionEnter2D(Collision2D Col){
		string causeOfDeath = null;

		// Select cause of death once the enemy
		if(Col.gameObject.tag == ("Enemy")){
			if(Col.gameObject.layer == LayerMask.NameToLayer("Lava")){
				causeOfDeath = "DEATH_BY_LAVA";
			}
			else if(Col.gameObject.layer == LayerMask.NameToLayer ("Bullet")){
				causeOfDeath = "DEATH_BY_BULLET";
			}
			else if(Col.gameObject.layer == LayerMask.NameToLayer ("SpikeBall")){
				causeOfDeath = "DEATH_BY_SPIKEBALL";
			}
			else if(Col.gameObject.layer == LayerMask.NameToLayer ("Guard")){
				causeOfDeath = "DEATH_BY_GUARD";
			}
			else if(Col.gameObject.layer == LayerMask.NameToLayer ("ShadowDog")){
				causeOfDeath = "DEATH_BY_SHADOWDOG";
			}
			else{
				causeOfDeath = "DEATH_BY_ENVIRONMENT";
			}
			PlayerDie(causeOfDeath);
			GameController.instance.GameOver(causeOfDeath);
			Debug.Log("Cause of Death: " + causeOfDeath);
		}
	}
}
