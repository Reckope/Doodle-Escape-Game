using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Scripts
	public ProjectilePool ProjectilePool;
	// Components
	private Rigidbody2D rb2d;
	private Animator anim;

	// GameObjects
	[Header("Game Objects")]
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Global Variables
	public bool movingLeft, movingRight, shootingLeft, shootingRight, shooting;
	[SerializeField]
	private float jumpForce, acceleration, maxSpeed, groundCheckRadius;
	private float nextFire, fireRate;


	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		nextFire = 0.0f;
		fireRate = 0.15f;
	}
	
	// Update is called once per frame
	void Update () {
		MovePlayer();
		ShootGuns();
	}

	// Key controls to move the player
	private void MovePlayer(){
		bool jump;
		bool grounded;
		float moveHorizontal;

		// I like to move it move it.
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
			movingLeft = true;
			movingRight = false;
		}
		else if(moveHorizontal > 0){
			movingLeft = false;
			movingRight = true;
		}
		else{
			movingLeft = false;
			movingRight = false;
		}

		// Jump up, jump up and get down. Jump around.
		jump = Input.GetKeyDown (KeyCode.W);
		if(jump){
			if(grounded){
				rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
		}
		//Debug.Log("velocity: " + rb2d.velocity.x);
	}

	// Kill those enemies!
	private void ShootGuns(){
		float shoot;

		shoot = Input.GetAxisRaw("Shoot");
		if(shoot < 0){
			shootingRight = false;
			shootingLeft = true;
			shooting = true;
			if(Time.time > nextFire){
				nextFire = Time.time + fireRate;
				SpawnProjectile();
			}
		}
		else if(shoot > 0){
			shootingLeft = false;
			shootingRight = true;
			shooting = true;
			if(Time.time > nextFire){
				nextFire = Time.time + fireRate;
				SpawnProjectile();
			}
		}
		else{
			shootingRight = false;
			shootingLeft = false;
			shooting = false;
		}
	}
	// Spawn the bullets
	private void SpawnProjectile(){
		float spawnProjectileLeft = (this.gameObject.transform.position.x - 1f);
		float spawnProjectileRight = (this.gameObject.transform.position.x + 1f);

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
}
