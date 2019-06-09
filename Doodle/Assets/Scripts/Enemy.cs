/* Author: Joe Davis
 * Project: Doodle Escape.
 * Code QA Sweep: DONE - 08/06/19
 * Notes:
 * This is the abstract class where different enemies, who
 * share the same variables & methods, can inherit from.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour{

    // Components
    public Rigidbody2D rb2d;
    public SpriteRenderer spriteFace;
    public SpriteRenderer sprite;

    // GameObjects
    public GameObject player;
    public LayerMask whatIsWall;
    public Transform leftCheck;
    public Transform rightCheck;
    public EnemySubTask subTask;
    public EnemyPrimaryTask primaryTask;

    // Core lobal Variables for enemies
    public enum EnemySubTask {moveLeft, moveRight};
    public enum EnemyPrimaryTask {patrol, dead, attack, asleep, idle};
    public const float COLLIDE_CHECK_RADIUS = 0.3f;
    public const int MIN_HEALTH = 0;
    public const int MOVE_RIGHT = 1;
    public const int MOVE_LEFT = -1;
    public const int ALERTED_MAX_SPEED = 5;
    public const int ENEMY_Y_POSITION = 0;
    public const float ENEMY_VISION_DISTANCE = 7.8f;
    public const float REDUCE_ENEMY_SPRITE_ALPHA = 0.1f;
    public int enemyLayerMask, groundLayerMask;
    public float health, damageTaken, spriteAlpha, maxSpeed;

    // ---------------------------------------------------------------------------------
    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        groundLayerMask = LayerMask.NameToLayer("Ground");
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        damageTaken = 10f;
        spriteAlpha = 1f;
    }

    // ---------- Core Tasks ----------

    // Use Raycasts to detect the player.
    public bool DetectPlayer(){
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
        RaycastHit2D enemyVision = Physics2D.Raycast(raySpawn, lookDirection, ENEMY_VISION_DISTANCE);

        if(enemyVision.collider != null && enemyVision.collider.name == "Player"){
            Debug.Log("Detected!");
            return true;
        }
        else{
            return false;
        }
    }

    public void Die(){
        //enemyDieAudio.Stop();
        //enemyDieAudio.Play();
        Destroy(gameObject);
    }

    // When the enemy gets hit with a rubber, it gradually starts to disappear. 
    public void TakeDamage(SpriteRenderer sprite){
        health -= damageTaken;
        primaryTask = EnemyPrimaryTask.attack;
        spriteAlpha -= REDUCE_ENEMY_SPRITE_ALPHA;
        sprite.color = new Color(1f, 1f, 1f, spriteAlpha);
    }

    public void Attack(){
        maxSpeed = ALERTED_MAX_SPEED;
    }

    public void MoveTowardsPlayer(){
        if(player != null){
            if(player.transform.position.x < transform.position.x){
                subTask = EnemySubTask.moveLeft;
            }
            else if(player.transform.position.x > transform.position.x){
                subTask = EnemySubTask.moveRight;
            }
        }
    }

    public void MoveEnemy(int direction, float acceleration){
        Vector2 move = new Vector2 (direction, ENEMY_Y_POSITION);
        if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed)){
            rb2d.AddForce(move * acceleration);
        }
    }
}
