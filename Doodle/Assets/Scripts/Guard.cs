using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Guard : Enemy{

    // Components
    Rigidbody2D rb2d;
    SpriteRenderer spriteFace;
    SpriteRenderer sprite;

    // GameObjects
    public Transform guardFace;
    public GameObject tazorLeft;
    public GameObject tazorRight;
    public LayerMask whatIsWall;
    public Transform leftCheck;
    public Transform rightCheck;

    // Global Variables
    const float COLLIDE_CHECK_RADIUS = 0.3f;
    const int ENEMY_Y_POSITION = 0;
    const int GUARD_FOLLOW_RANGE = 15;
    private int direction;
    [SerializeField]
    private float acceleration;
    private bool collidedLeft, collidedRight;
    Quaternion rotation;

    // ---------------------------------------------------------------------------------
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        spriteFace = guardFace.GetComponent<SpriteRenderer>();
        primaryTask = EnemyPrimaryTask.patrol;
        subTask = EnemySubTask.moveLeft;
        rotation = guardFace.transform.rotation;
        maxSpeed = 2f;
        health = 100f;
        base.Start();
    }

    // Update is called once per frame
    void Update(){
        guardFace.transform.rotation = rotation;
        SetTasks();
        //DetectPlayer();
        if(health > MIN_HEALTH){
            DetectPlayer();
            if(Player.IsDead){
                primaryTask = EnemyPrimaryTask.idle;
            }
        }
        else{
            primaryTask = EnemyPrimaryTask.dead;
        }
    }

    private void SetTasks(){
        switch(primaryTask){
            case EnemyPrimaryTask.patrol:
            Patrol();
            break;
            case EnemyPrimaryTask.follow:
            Follow();
            break;
            case EnemyPrimaryTask.attack:
            Attack();
            MoveGuard();
            break;
            case EnemyPrimaryTask.dead:
            Die();
            break;
            case EnemyPrimaryTask.idle:
            // Idle
            break;
        }

        switch(subTask){
            case EnemySubTask.moveLeft:
            direction = MOVE_LEFT;
            faceLeft();
            break;
            case EnemySubTask.moveRight:
            direction = MOVE_RIGHT;
            faceRight();
            break;
        }
    }

    private void MoveGuard(){
        Vector2 move = new Vector2 (direction, ENEMY_Y_POSITION);
        if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed)){
            rb2d.AddForce(move * acceleration);
        }
    }

    private float DistanceBetweenPlayerAndEnemy(){
        float distance;

        distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        return distance;
    }

    // ---------- Tasks ----------

    private void Patrol(){
        MoveGuard();
        // Check if the guard hits a wall.
        collidedLeft = Physics2D.OverlapCircle (leftCheck.position, COLLIDE_CHECK_RADIUS, whatIsWall);
        collidedRight = Physics2D.OverlapCircle (rightCheck.position, COLLIDE_CHECK_RADIUS, whatIsWall);
        // If the guard hits a wall, change their direction.
        if(collidedLeft){
            direction = MOVE_RIGHT;
            subTask = EnemySubTask.moveRight;
        }
        else if(collidedRight && !collidedLeft){
            direction = MOVE_LEFT;
            subTask = EnemySubTask.moveLeft;
        }
    }

    private void faceRight(){
        spriteFace.flipX = false;
        tazorLeft.SetActive(false);
        tazorRight.SetActive(true);
    }

    private void faceLeft(){
        spriteFace.flipX = true;
        tazorLeft.SetActive(true);
        tazorRight.SetActive(false);
    }

    // Once I see the player within range, I sprint forward to try and catch him. If I lose sight
    // of him, I attempt to follow / chase him. 
    private void DetectPlayer(){
        Vector2 lookDirection;
        var raySpawn = transform.position;
        var left = leftCheck.transform.position;
        var right = rightCheck.transform.position;

        if(subTask == EnemySubTask.moveLeft){
            lookDirection = Vector2.left;
            raySpawn = left;
        }
        else{
            lookDirection = Vector2.right;
            raySpawn = right;
        }
        RaycastHit2D guardVision = Physics2D.Raycast(raySpawn, lookDirection, ENEMY_LOOK_DISTANCE);

        if(guardVision.collider != null && guardVision.collider.name == "Player"){
            primaryTask = EnemyPrimaryTask.attack;
        }
        else if(alerted && DistanceBetweenPlayerAndEnemy() <= GUARD_FOLLOW_RANGE){
            primaryTask = EnemyPrimaryTask.follow;
        }
        else{
            primaryTask = EnemyPrimaryTask.patrol;
        }
    }

    // Follow that prisoner!!
    private void Follow(){
        if(player.transform.position.x < transform.position.x){
            direction = MOVE_LEFT;
            subTask = EnemySubTask.moveLeft;
        }
        else if(player.transform.position.x > transform.position.x){
            direction = MOVE_RIGHT;
            subTask = EnemySubTask.moveRight;
        }
        MoveGuard();
    }

    // ---------- Triggers ----------

    private void OnTriggerEnter2D (Collider2D collide){
        // If the enemy gets hit by a projectile...
        if(collide.gameObject.layer == LayerMask.NameToLayer("Projectile") && health > MIN_HEALTH){
            TakeDamage(sprite);
            //sprite.color = new Color(1f, 1f, 1f, spriteAlpha);
        }
    }

    private void OnCollisionEnter2D(Collision2D collide){
        if(collide.gameObject.layer == LayerMask.NameToLayer("Lava")){
            Die();
        }
    }
}
