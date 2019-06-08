/* Author: Joe Davis
 * Project: Doodle Escape
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

    LevelManager LevelManager;
    EnemySpawnPoints SpawnPoints;

    public EnemySpawnPoints[] levelOneSpawnPoints;
    public EnemySpawnPoints[] levelTwoSpawnPoints;
    public EnemySpawnPoints[] levelThreeSpawnPoints;
    public EnemySpawnPoints[] levelFourSpawnPoints;
    public Transform guard;
    public Transform dogLeft;
    public Transform dogRight;
    GameObject fieldOfView;

    // ---------------------------------------------------------------------------------
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        fieldOfView = GameObject.Find("FieldOfViewPlayer");
        fieldOfView.SetActive(false);
    }

    // We get the level details from LevelManager.cs, and do whatever we want with the data.
    // Including launching the levels function via the ID.
    public void LaunchLevel(string id, string name, string objective, int buildIndex, bool isActive){
        LevelManager.currentLevel = buildIndex;
        LevelManager.currentObjective = objective;
        GameController.instance.SetHelpText("Level launched: " + name);
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
        SpawnEnemies(levelOneSpawnPoints);
    }

    private void LevelTwo(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
        fieldOfView.SetActive(true);
        SpawnEnemies(levelTwoSpawnPoints);
    }

    private void LevelThree(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
        Debug.Log("LEVEL THREEEE");
        SpawnEnemies(levelThreeSpawnPoints);
    }

    private void LevelFour(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
        SpawnEnemies(levelFourSpawnPoints);
    }

    private void LevelFive(){
        GameController.instance.UnlockDoor();
    }
}
