/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
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
    public AudioSource dieSound;
    public AudioSource guardAlert;
    public AudioSource dogAlert;

    // GameObjects
    public GameObject player;
    public GameObject enemyDeathSound;
    public GameObject guardAlertSound;
    public GameObject dogAlertSound;
    public LayerMask whatIsWall;
    public Transform leftCheck;
    public Transform rightCheck;

    // Objects
    public EnemySubTask subTask;
    public EnemyPrimaryTask primaryTask;

    // Core lobal Variables for enemies
    public enum EnemySubTask {moveLeft, moveRight};
    public enum EnemyPrimaryTask {patrol, dead, attack, asleep, idle};
    public const float COLLIDE_CHECK_RADIUS = 0.3f;
    public const int MIN_HEALTH = 0;
    public const int MOVE_RIGHT = 1;
    public const int MOVE_LEFT = -1;
    public const float ALERTED_MAX_SPEED = 4.2f;
    public const int ENEMY_Y_POSITION = 0;
    public const float ENEMY_VISION_DISTANCE = 7.8f;
    public const float REDUCE_ENEMY_SPRITE_ALPHA = 0.1f;
    public int enemyLayerMask, groundLayerMask;
    public float health, damageTaken, spriteAlpha, maxSpeed;
    private bool preventLoop;

    // ---------------------------------------------------------------------------------
    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        groundLayerMask = LayerMask.NameToLayer("Ground");
        enemyDeathSound = GameObject.Find("pop");
        guardAlertSound = GameObject.Find("alert");
        dogAlertSound = GameObject.Find("growl");
        dieSound = enemyDeathSound.GetComponent<AudioSource>();
        guardAlert = guardAlertSound.GetComponent<AudioSource>();
        dogAlert = dogAlertSound.GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        damageTaken = 10f;
        spriteAlpha = 1f;
    }

    // ---------- Core Tasks and Methods ----------

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
            //Debug.Log("Detected!");
            return true;
        }
        else{
            return false;
        }
    }

    public void Die(){
        dogAlert.Stop();
        guardAlert.Stop();
        if(dieSound != null){
            dieSound.Play();
        }
        Destroy(gameObject);
    }

    // When the enemy gets hit with a rubber, it gradually starts to disappear. 
    public void TakeDamage(SpriteRenderer sprite){
        float maxRGBAValue = 1f;

        health -= damageTaken;
        primaryTask = EnemyPrimaryTask.attack;
        spriteAlpha -= REDUCE_ENEMY_SPRITE_ALPHA;
        sprite.color = new Color(maxRGBAValue, maxRGBAValue, maxRGBAValue, spriteAlpha);
    }

    public void Attack(){
        maxSpeed = ALERTED_MAX_SPEED;
    }

    // Once detected, the enemy follows the player. 
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

    // This is constantly called to move the enemy. Different tasks pass over
    // the direction and speed it should go.
    public void MoveEnemy(int direction, float acceleration){
        Vector2 move = new Vector2 (direction, ENEMY_Y_POSITION);
        if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed)){
            rb2d.AddForce(move * acceleration);
        }
    }

    // When an enemy gets alerts, play the sound that is passed over. 
    public void PlayEnemySound(AudioSource enemySound){
        if(preventLoop){
                return;
            }
        preventLoop = true;
        if(enemySound != null){
            enemySound.Play();
        }
    }
}