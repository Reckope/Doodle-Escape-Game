﻿/* Author: Joe Davis
 * Project: Doodle Escape.
 * References: [2]
 * 2019
 * Notes:
 * This is used to control what happens within each level, as
 * well as invoking them. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable class for the enemies that spawn within each level.
// I can simply drag spawn points into these arrays within the Unity editor. 
[Serializable]
public class EnemySpawnPoints{
    public GameObject[] guardSpawnPoints;
    public GameObject[] dogLeftSpawnPoints;
    public GameObject[] dogRightSpawnPoints;
}

public class Levels : MonoBehaviour{

    // Other Classes
    LevelManager LevelManager;
    EnemySpawnPoints SpawnPoints;

    // GameObjects
    public Transform guard;
    public Transform dogLeft;
    public Transform dogRight;
    GameObject fieldOfView;

    // Global Variables
    public const int LEVEL_ZERO_MUSIC = 0;
    public const int LEVEL_ONE_MUSIC = 1;
    public const int LEVEL_TWO_MUSIC = 2;
    public const int LEVEL_THREE_MUSIC = 3;
    public const int LEVEL_FOUR_MUSIC = 4;
    public const int LEVEL_FIVE_MUSIC = 5;
    public EnemySpawnPoints[] levelOneSpawnPoints;
    public EnemySpawnPoints[] levelTwoSpawnPoints;
    public EnemySpawnPoints[] levelThreeSpawnPoints;
    public EnemySpawnPoints[] levelFourSpawnPoints;

    // ---------------------------------------------------------------------------------
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        fieldOfView = GameObject.Find("FieldOfViewPlayer");
        fieldOfView.SetActive(false);
    }

    void Update(){
        // Debug to unlock door:
        //if(Input.GetKeyDown("space")){
        //    LaunchLevel("LevelFive", "f", "escape", 5, true);
        //}
    }

    // We get the level details from LevelManager.cs, and do whatever we want with the data.
    // Including launching the levels' method via the ID.
    public void LaunchLevel(string id, string name, string objective, int buildIndex, bool isActive){
        LevelManager.currentLevel = buildIndex;
        LevelManager.currentObjective = objective;
        if(buildIndex != LEVEL_ZERO_MUSIC){
            LevelManager.StopLevelMusic(LevelManager.levelMusic[LEVEL_ZERO_MUSIC]);
            LevelManager.PlayLevelMusic(LevelManager.levelMusic[buildIndex]);
        }
        Invoke(id, 0f);
    }

    // Using a jagged array, I am able to spawn the three different types of enemies into
    // the level that was triggered.
    // I tried to pass SpawnPoints.[enemy]SpawnPoints as a param to reduce code duplication,
    // but it wasn't working. 
    void SpawnEnemies(EnemySpawnPoints[] level){
        //Debug.Log("Activated: " + level);
        for(int i = 0; i < level.Length; i++){
            for(int j = 0; j < level[i].guardSpawnPoints.Length; j++){
                Instantiate(guard, new Vector3(level[i].guardSpawnPoints[j].transform.position.x, level[i].guardSpawnPoints[j].transform.position.y), Quaternion.identity);
            }
            for(int k = 0; k < level[i].dogLeftSpawnPoints.Length; k++){
                Instantiate(dogLeft, new Vector3(level[i].dogLeftSpawnPoints[k].transform.position.x, level[i].dogLeftSpawnPoints[k].transform.position.y), Quaternion.identity);
            }
            for(int l = 0; l < level[i].dogRightSpawnPoints.Length; l++){
                Instantiate(dogRight, new Vector3(level[i].dogRightSpawnPoints[l].transform.position.x, level[i].dogRightSpawnPoints[l].transform.position.y), Quaternion.identity);
            }
        }
    }

    // -------------------- Levels, launched via the levels ID. --------------------

    private void LevelOne(){
        GameController.instance.SetHelpText("If the guards spot you on your mission to escape, things will get very messy.");
        SpawnEnemies(levelOneSpawnPoints);
    }

    private void LevelTwo(){
        GameController.instance.SetHelpText("The dogs can still sense your presence in the dark...");
        fieldOfView.SetActive(true);
        SpawnEnemies(levelTwoSpawnPoints);
    }

    private void LevelThree(){
        GameController.instance.SetHelpText("Be careful not to fall off the platform, lava has been known to instantly burn prisoners who attempt to steal from the prison.");
        SpawnEnemies(levelThreeSpawnPoints);
    }

    private void LevelFour(){
        GameController.instance.SetHelpText("Prisoners have been known to mistime their jump, resulting in being savagely tased by the guard patrolling the area.");
        SpawnEnemies(levelFourSpawnPoints);
    }

    private void LevelFive(){
        GameController.instance.UnlockDoor();
    }
}
