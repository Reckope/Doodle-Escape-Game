using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the abstract class where different enemies, who
// share the same qualities, can inherit variables and methods from.
public abstract class Enemy : MonoBehaviour{

    public GameObject player;
    public EnemySubTask subTask;
    public EnemyPrimaryTask primaryTask;

    public enum EnemySubTask {moveLeft, moveRight};
    public enum EnemyPrimaryTask {patrol, follow, dead, attack, asleep, idle};
    public const int MIN_HEALTH = 0;
    public const int MOVE_RIGHT = 1;
    public const int MOVE_LEFT = -1;
    const int ALERTED_MAX_SPEED = 5;
    public const float ENEMY_LOOK_DISTANCE = 8.4f;
    public const float REDUCE_ENEMY_SPRITE_ALPHA = 0.1f;
    public int enemyLayerMask, groundLayerMask;
    public float health, damageTaken, spriteAlpha, maxSpeed;
    public bool alerted;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        groundLayerMask = LayerMask.NameToLayer("Ground");
        damageTaken = 10f;
        spriteAlpha = 1f;
        alerted = false;
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
}
