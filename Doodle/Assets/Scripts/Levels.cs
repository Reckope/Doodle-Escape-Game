/* Author: Joe Davis
 * Project: Doodle Escape
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    LevelManager LevelManager;

    public GameObject[] guardSpawnPointsLevelOne;
    public GameObject[] dogSpawnPointsLevelOne;
    public Transform guard;
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

    // -------------------- Levels, launched via the levels ID. --------------------

    public void LevelOne(){
        // CREATE ONE FUNCTION FOR THIS, THEN PASS A VAR FOR THE LEVEL
        for(int i = 0; i < guardSpawnPointsLevelOne.Length; i++){
            if(guardSpawnPointsLevelOne[i] != null){
                Instantiate(guard, new Vector3(guardSpawnPointsLevelOne[i].transform.position.x, guardSpawnPointsLevelOne[i].transform.position.y), Quaternion.identity);
            }
            else{
                Debug.Log("Object " + i + " is null");
            }
        }
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
