/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * Attach this to the Guard object to set / manage its relevant tasks.  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy{

    // GameObjects
    public Transform face;
    public GameObject tazorLeft;
    public GameObject tazorRight;

    // Global Variables
    const int GUARD_FOLLOW_RANGE = 15;
    private int direction;
    [SerializeField]
    private float acceleration;
    private bool collidedLeft, collidedRight;
    private Quaternion rotation;

    // ---------------------------------------------------------------------------------
    void Start(){
        base.Start();
        spriteFace = face.GetComponent<SpriteRenderer>();
        rotation = face.transform.rotation;
        primaryTask = EnemyPrimaryTask.patrol;
        subTask = EnemySubTask.moveLeft;
        maxSpeed = 2f;
        health = 100f;
    }

    void Update(){
        face.transform.rotation = rotation;
        SetGuardTasks();
        if(health > MIN_HEALTH){
            DetectPlayer();
            if(DetectPlayer()){
                primaryTask = EnemyPrimaryTask.attack;
            }
            else if(DistanceBetweenPlayerAndEnemy() >= GUARD_FOLLOW_RANGE){
                primaryTask = EnemyPrimaryTask.patrol;
            }
            else if(Player.isDead){
                primaryTask = EnemyPrimaryTask.idle;
            }
        }
        else{
            primaryTask = EnemyPrimaryTask.dead;
        }
    }

    // These are the main tasks for the guard. Once a task is set, the
    // relevant methods are called. 
    private void SetGuardTasks(){
        switch(primaryTask){
            case EnemyPrimaryTask.patrol:
            Patrol();
            break;
            case EnemyPrimaryTask.attack:
            PlayEnemySound(guardAlert);
            MoveTowardsPlayer();
            Attack();
            MoveEnemy(direction, acceleration);
            break;
            case EnemyPrimaryTask.dead:
            Die();
            break;
            case EnemyPrimaryTask.idle:
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            break;
        }

        switch(subTask){
            case EnemySubTask.moveLeft:
            direction = MOVE_LEFT;
            GuardFaceLeft();
            break;
            case EnemySubTask.moveRight:
            direction = MOVE_RIGHT;
            GuardFaceRight();
            break;
        }
    }

    private float DistanceBetweenPlayerAndEnemy(){
        float distance;
        if(player != null){
            distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        }
        else{
            distance = 0;
        }
        return distance;
    }

    // ---------- Tasks ----------

    private void Patrol(){
        MoveEnemy(direction, acceleration);
        // Check if the guard hits a wall.
        collidedLeft = Physics2D.OverlapCircle (leftCheck.position, COLLIDE_CHECK_RADIUS, whatIsWall);
        collidedRight = Physics2D.OverlapCircle (rightCheck.position, COLLIDE_CHECK_RADIUS, whatIsWall);
        // If the guard hits a wall, change their direction.
        if(collidedLeft){
            subTask = EnemySubTask.moveRight;
        }
        else if(collidedRight && !collidedLeft){
            subTask = EnemySubTask.moveLeft;
        }
    }

    private void GuardFaceRight(){
        spriteFace.flipX = false;
        tazorLeft.SetActive(false);
        tazorRight.SetActive(true);
    }

    private void GuardFaceLeft(){
        spriteFace.flipX = true;
        tazorLeft.SetActive(true);
        tazorRight.SetActive(false);
    }

    // ---------- Triggers ----------

    private void OnTriggerEnter2D (Collider2D collide){
        if(collide.gameObject.layer == LayerMask.NameToLayer("Projectile") && health > MIN_HEALTH){
            TakeDamage(sprite);
        }
    }

    private void OnCollisionEnter2D(Collision2D collide){
        if(collide.gameObject.layer == LayerMask.NameToLayer("Lava")){
            Die();
        }
    }
}