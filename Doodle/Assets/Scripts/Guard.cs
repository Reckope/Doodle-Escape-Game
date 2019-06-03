using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour{

    GuardSubTask subTask;
    GuardPrimaryTask primaryTask;

    // Components
    Rigidbody2D rb2d;
    SpriteRenderer mySpriteRenderer;

    // GameObjects
    public Transform guardFace;
    public GameObject tazorLeft;
    public GameObject tazorRight;
    public LayerMask whatIsWall;
    public Transform leftCheck;
    public Transform rightCheck;
    GameObject Player;

    // Global Variables
    const float GUARD_LOOK_DISTANCE = 8.4f;
    const int ALERTED_MAX_SPEED = 5;
    const int GUARD_Y_POSITION = 0;
    private int enemyLayerMask, groundLayerMask;
    private int direction;
    [SerializeField]
    private float acceleration, maxSpeed;
    private float checkRadius;
    private bool collidedLeft, collidedRight, alerted;
    Quaternion rotation;
    enum GuardPrimaryTask {patrol, follow, dead, attack};
    enum GuardSubTask {moveLeft, moveRight};

    // ---------------------------------------------------------------------------------
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        mySpriteRenderer = guardFace.GetComponent<SpriteRenderer>();
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        groundLayerMask = LayerMask.NameToLayer("Ground");
        Player = GameObject.FindGameObjectWithTag("Player");
        primaryTask = GuardPrimaryTask.patrol;
        subTask = GuardSubTask.moveLeft;
        alerted = false;
        rotation = guardFace.transform.rotation;
        checkRadius = 0.3f;
    }

    // Update is called once per frame
    void Update(){
        guardFace.transform.rotation = rotation;
        SetTasks();
        DetectPlayer();
        Debug.Log(primaryTask);
    }

    private void SetTasks(){
        switch(primaryTask){
            case GuardPrimaryTask.patrol:
            Patrol();
            break;
            case GuardPrimaryTask.follow:
            Follow();
            break;
            case GuardPrimaryTask.attack:
            Attack();
            break;
            case GuardPrimaryTask.dead:
            break;
        }

        switch(subTask){
            case GuardSubTask.moveLeft:
            direction = -1;
            faceLeft();
            break;
            case GuardSubTask.moveRight:
            direction = 1;
            faceRight();
            break;
        }
    }
    // ----- Tasks -----
    private void Patrol(){
        MoveGuard();
        // Check if the guard hits a wall.
        collidedLeft = Physics2D.OverlapCircle (leftCheck.position, checkRadius, whatIsWall);
        collidedRight = Physics2D.OverlapCircle (rightCheck.position, checkRadius, whatIsWall);
        // If the guard hits a wall, change their direction.
        if(collidedLeft){
            direction = 1;
            subTask = GuardSubTask.moveRight;
        }
        else if(collidedRight){
            direction = -1;
            subTask = GuardSubTask.moveLeft;
        }
    }

    private void faceRight(){
        mySpriteRenderer.flipX = false;
        tazorLeft.SetActive(false);
        tazorRight.SetActive(true);
    }

    private void faceLeft(){
        mySpriteRenderer.flipX = true;
        tazorLeft.SetActive(true);
        tazorRight.SetActive(false);
    }

    // Once I see the player within range, I sprint forward to try and catch him. If I lose sight
    // of him, I attempt to follow / chase him. 
    private void DetectPlayer(){
        Vector2 lookDirection;

        if(subTask == GuardSubTask.moveLeft){
            lookDirection = Vector2.left;
        }
        else{
            lookDirection = Vector2.right;
        }
        RaycastHit2D guardVision = Physics2D.Raycast(leftCheck.transform.position, lookDirection, GUARD_LOOK_DISTANCE);

        if(guardVision.collider != null && guardVision.collider.name == "Player"){
            primaryTask = GuardPrimaryTask.attack;
        }
        else if(alerted){
            primaryTask = GuardPrimaryTask.follow;
        }
    }

    // Follow that prisoner!!
    private void Follow(){
        if(Player.transform.position.x < transform.position.x){
            direction = -1;
            subTask = GuardSubTask.moveLeft;
        }
        else if(Player.transform.position.x > transform.position.x){
            direction = 1;
            subTask = GuardSubTask.moveRight;
        }
        MoveGuard();
    }

    private void Attack(){
        maxSpeed = ALERTED_MAX_SPEED;
        alerted = true;
        MoveGuard();
    }

    private void MoveGuard(){
        Vector2 move = new Vector2 (direction, GUARD_Y_POSITION);
        if((rb2d.velocity.x >= -maxSpeed && rb2d.velocity.x <= maxSpeed)){
            rb2d.AddForce(move * acceleration);
        }
    }
}
