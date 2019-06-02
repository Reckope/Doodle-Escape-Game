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

    // Global Variables
    private int direction;
    [SerializeField]
    private float speed;
    private float checkRadius;
    private bool collidedLeft, collidedRight;
    Quaternion rotation;
    enum GuardPrimaryTask {patrol, alerted, dead, attack};
    enum GuardSubTask {dontMove, moveLeft, moveRight, faceLeft, faceRight};

    // ---------------------------------------------------------------------------------
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        mySpriteRenderer = guardFace.GetComponent<SpriteRenderer>();
        primaryTask = GuardPrimaryTask.patrol;
        rotation = guardFace.transform.rotation;
        checkRadius = 0.1f;
        direction = 1;
    }

    // Update is called once per frame
    void Update(){
        guardFace.transform.rotation = rotation;
        SetTasks();
    }

    private void SetTasks(){
        switch(primaryTask){
            case GuardPrimaryTask.patrol:
            Patrol();
            break;
            case GuardPrimaryTask.alerted:
            break;
            case GuardPrimaryTask.attack:
            break;
            case GuardPrimaryTask.dead:
            break;
        }

        switch(subTask){
            case GuardSubTask.dontMove:
            break;
            case GuardSubTask.faceLeft:
            faceLeft();
            break;
            case GuardSubTask.faceRight:
            faceRight();
            break;
            case GuardSubTask.moveLeft:
            direction = 1;
            break;
            case GuardSubTask.moveRight:
            direction = -1;
            break;
        }
    }
    // ----- Tasks -----
    private void Patrol(){
        Vector2 movement = new Vector2 (direction, 0);
        rb2d.AddForce(movement * speed);
        // Check if the guard hits a wall.
        collidedLeft = Physics2D.OverlapCircle (leftCheck.position, checkRadius, whatIsWall);
        collidedRight = Physics2D.OverlapCircle (rightCheck.position, checkRadius, whatIsWall);
        // If the guard hits a wall, change their direction.
        if(collidedLeft){
            subTask = GuardSubTask.moveRight;
            subTask = GuardSubTask.faceRight;
        }
        else if(collidedRight){
            direction = -1;
            subTask = GuardSubTask.moveLeft;
            subTask = GuardSubTask.faceLeft;
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
}
