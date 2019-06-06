/* Author: Joe Davis
 * Project: Doodle Escape
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    LevelManager LevelManager;

    public GameObject[] guardSpawnPointsLevelOne;
    public GameObject[] dogLeftSpawnPointsLevelOne;
    public GameObject[] dogRightSpawnPointsLevelOne;
    public Transform guard;
    public Transform dogLeft;
    public Transform dogRight;
    GameObject fieldOfView;

    // ---------------------------------------------------------------------------------
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        fieldOfView = GameObject.Find("FieldOfView");
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

    // This is called from each level, to determine what enemies to spawn.
    private void SpawnEnemiesInLevel(int level){
        GameObject[] guardsArray = guardSpawnPointsLevelOne;
        GameObject[] dogsLeftArray = dogLeftSpawnPointsLevelOne;
        GameObject[] dogsRightArray = dogRightSpawnPointsLevelOne;

        switch(level){
            case 5:
            break;
            case 4:
            break;
            case 3:
            break;
            case 2:
            break;
            case 1:
            guardsArray = guardSpawnPointsLevelOne;
            dogsLeftArray = dogLeftSpawnPointsLevelOne;
            dogsRightArray = dogRightSpawnPointsLevelOne;
            break;
            default:
            guardsArray = null;
            break;
        }
        SpawnEnemies(guardsArray, dogsLeftArray, dogsRightArray);
    }

    private void SpawnEnemies(GameObject[] guardsArray, GameObject[] dogsLeftArray, GameObject[] dogsRightArray){
        for(int i = 0; i < guardsArray.Length; i++){
            if(guardsArray[i] != null){
                Instantiate(guard, new Vector3(guardsArray[i].transform.position.x, guardsArray[i].transform.position.y), Quaternion.identity);
            }
        }
        for(int i = 0; i < dogsLeftArray.Length; i++){
            if(dogsLeftArray[i] != null){
                Instantiate(dogLeft, new Vector3(dogsLeftArray[i].transform.position.x, dogsLeftArray[i].transform.position.y), Quaternion.identity);
            }
        }
        for(int i = 0; i < dogsRightArray.Length; i++){
            if(dogsRightArray[i] != null){
                Instantiate(dogRight, new Vector3(dogsRightArray[i].transform.position.x, dogsRightArray[i].transform.position.y), Quaternion.identity);
            }
        }
    }

    // -------------------- Levels, launched via the levels ID. --------------------

    public void LevelOne(){
        SpawnEnemiesInLevel(1);
    }

    public void LevelTwo(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
        fieldOfView.SetActive(true);
    }

    public void LevelThree(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelFour(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelFive(){
        GameController.instance.UnlockDoor();
    }
}
