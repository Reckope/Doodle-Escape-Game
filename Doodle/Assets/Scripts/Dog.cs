using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy {

    Quaternion rotation;
    public Transform face;
    private Animator anim;

    private int direction;
    [SerializeField]
    private float acceleration;
    private bool collidedLeft, collidedRight;

    // Start is called before the first frame update
    void Start(){
        spriteFace = face.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        primaryTask = EnemyPrimaryTask.asleep;
        //subTask = EnemySubTask.moveRight;
        rotation = face.transform.rotation;
        base.Start();
    }

    // Update is called once per frame
    void Update(){
        face.transform.rotation = rotation;
        SetTasks();
        if(health > MIN_HEALTH){
            DetectPlayer();
            if(DetectPlayer()){
                primaryTask = EnemyPrimaryTask.attack;
            }
            if(Player.isDead){
                primaryTask = EnemyPrimaryTask.idle;
            }
        }
        else{
            primaryTask = EnemyPrimaryTask.dead;
        }
    }

    private void SetTasks(){
        switch(primaryTask){
            case EnemyPrimaryTask.asleep:
            // Play snoring sound
            break;
            case EnemyPrimaryTask.attack:
            anim.SetTrigger("DogAwake");
            MoveTowardsPlayer();
            Attack();
            MoveDog();
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

    // ----- Tasks -----

    private void MoveDog(){
        Vector2 move = new Vector2 (direction, ENEMY_Y_POSITION);
        if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed)){
            rb2d.AddForce(move * acceleration);
        }
    }

    private void OnTriggerEnter2D (Collider2D collide){
        // If the enemy gets hit by a projectile...
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
