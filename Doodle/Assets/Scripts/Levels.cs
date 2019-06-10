/* Author: Joe Davis
 * Project: Doodle Escape.
 * References: [2]
 * Code QA Sweep: DONE - 10/06/19
 * Notes:
 * This is used to control what happens within each level, as
 * well as invoking them. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnPoints{
    public GameObject[] guardSpawnPoints;
    public GameObject[] dogLeftSpawnPoints;
    public GameObject[] dogRightSpawnPoints;
}

public class Levels : MonoBehaviour{

    // Other Classes.
    LevelManager LevelManager;
    EnemySpawnPoints SpawnPoints;

    // GameObjects
    public Transform guard;
    public Transform dogLeft;
    public Transform dogRight;
    GameObject fieldOfView;

    // Global Variables
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

    // We get the level details from LevelManager.cs, and do whatever we want with the data.
    // Including launching the levels' method via the ID.
    public void LaunchLevel(string id, string name, string objective, int buildIndex, bool isActive){
        LevelManager.currentLevel = buildIndex;
        LevelManager.currentObjective = objective;
        Invoke(id, 0f);
    }

    // Using a jagged array, I am able to spawn different types of enemies into
    // the level that was triggered.
    void SpawnEnemies(EnemySpawnPoints[] level){
        Debug.Log("Activated: " + level);
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
