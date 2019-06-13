/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * Attach this to the Guard object to set / manage its relevant tasks.  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy {

    // Components
    private Quaternion rotation;
    public Transform face;
    private Animator anim;

    // Global Variables
    private int direction;
    [SerializeField]
    private float acceleration;
    private bool collidedLeft, collidedRight;

    // ---------------------------------------------------------------------------------
    void Start(){
        base.Start();
        rotation = face.transform.rotation;
        spriteFace = face.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        primaryTask = EnemyPrimaryTask.asleep;
    }

    void Update(){
        face.transform.rotation = rotation;
        SetTasks();
        if(health > MIN_HEALTH){
            DetectPlayer();
            if(DetectPlayer()){
                primaryTask = EnemyPrimaryTask.attack;
            }
            else if(Player.isDead){
                primaryTask = EnemyPrimaryTask.idle;
            }
        }
        else{
            primaryTask = EnemyPrimaryTask.dead;
        }
    }

    // These are the main tasks for the dog. Once a task is set, the
    // relevant methods are called. 
    private void SetTasks(){
        switch(primaryTask){
            case EnemyPrimaryTask.asleep:
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            break;
            case EnemyPrimaryTask.attack:
            rb2d.constraints = RigidbodyConstraints2D.None;
            PlayEnemySound(dogAlert);
            anim.SetTrigger("DogAwake");
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
            spriteFace.flipX = true;
            direction = MOVE_LEFT;
            break;
            case EnemySubTask.moveRight:
            spriteFace.flipX = false;
            direction = MOVE_RIGHT;
            break;
        }
    }

    // ---------- Triggers ----------

    private void OnTriggerEnter2D (Collider2D col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Projectile") && health > MIN_HEALTH){
            TakeDamage(sprite);
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Lava")){
            Die();
        }
    }
}
