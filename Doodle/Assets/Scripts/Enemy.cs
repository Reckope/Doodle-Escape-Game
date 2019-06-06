using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the abstract class where different enemies, who
// share the same qualities, can inherit variables and methods from.
public abstract class Enemy : MonoBehaviour{

    // Components
    public Rigidbody2D rb2d;
    public SpriteRenderer spriteFace;
    public SpriteRenderer sprite;

    public GameObject player;
    public LayerMask whatIsWall;
    public Transform leftCheck;
    public Transform rightCheck;
    public EnemySubTask subTask;
    public EnemyPrimaryTask primaryTask;

    public enum EnemySubTask {moveLeft, moveRight};
    public enum EnemyPrimaryTask {patrol, follow, dead, attack, asleep, idle};
    public const int MIN_HEALTH = 0;
    public const int MOVE_RIGHT = 1;
    public const int MOVE_LEFT = -1;
    const int ALERTED_MAX_SPEED = 5;
    public const int ENEMY_Y_POSITION = 0;
    public const float ENEMY_LOOK_DISTANCE = 7.8f;
    public const float REDUCE_ENEMY_SPRITE_ALPHA = 0.1f;
    public int enemyLayerMask, groundLayerMask;
    public float health, damageTaken, spriteAlpha, maxSpeed;
    public bool alerted;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        groundLayerMask = LayerMask.NameToLayer("Ground");
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        damageTaken = 10f;
        spriteAlpha = 1f;
        alerted = false;
    }

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
        RaycastHit2D enemyVision = Physics2D.Raycast(raySpawn, lookDirection, ENEMY_LOOK_DISTANCE);

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

    public void TakeDamage(SpriteRenderer sprite){
        alerted = true;
        health -= damageTaken;
        primaryTask = EnemyPrimaryTask.attack;
        spriteAlpha -= REDUCE_ENEMY_SPRITE_ALPHA;
        sprite.color = new Color(1f, 1f, 1f, spriteAlpha);
    }

    public void Attack(){
        maxSpeed = ALERTED_MAX_SPEED;
        alerted = true;
    }

    public void MoveTowardsPlayer(){
        if(player.transform.position.x < transform.position.x){
            subTask = EnemySubTask.moveLeft;
        }
        else if(player.transform.position.x > transform.position.x){
            subTask = EnemySubTask.moveRight;
        }
    }
}
