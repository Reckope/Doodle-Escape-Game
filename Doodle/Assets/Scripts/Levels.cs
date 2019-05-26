// Would using an Abstract Class be more efficient here? 
// Public abstract class Levels{}. 
// Public class FetchLevel : Levels{}.
// Need to experiment in next project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];
    GameObject startPoint;

    // Global Variables
    public int currentLevel;

    // Start is called before the first frame update
    void Start(){
        startPoint = GameObject.Find("Level0Trigger");
        currentLevel = 0;
        DisableLevelBlockers();
    }

    // Update is called once per frame
    void Update(){
        //Debug.Log("Current: " + currentLevel);
    }

    public void ActivateLevel(int level){
        currentLevel = level;
        //Debug.Log("ACTIVATED: " + currentLevel);
        if(GameController.instance.numberOfKeysRemaining >= 1){
            switch(currentLevel){
                case 4:
                LevelFour();
                break;
                case 3:
                LevelThree();
                break;
                case 2:
                LevelTwo();
                break;
                case 1:
                LevelOne();
                break;
                case 0:
                NotOnLevel();
                break;
                default:
                Debug.Log("ERROR: Couldn't find level number.");
                break;
            }
        }
        else{
            Escape();
        }
        GameController.instance.SetObjectiveText();
    }

    public void DisableLevelBlockers(){
        for(int i = 0; i < levelBlockers.Length; i++){
            levelBlockers[i].SetActive(false);
        }
    }

    public void ActivateLevelBlocker(int level){
        if(levelBlockers[level] != null){
            levelBlockers[level].SetActive(true);
        }
        else{
            return;
        }
    }

    // Don't really need to check if the right key collected matches the level, but
    // could do something like this in the future for bigger projects. 
    public void CompletedLevel(int level, string keyAcquired){
        if(GameController.instance.keyNeeded == keyAcquired){
            Debug.Log("Completed Level: " + level);
            GameController.instance.objective = "Return to the start position";
            GameController.instance.keyNeeded = null;
        }
        else{
            Debug.Log("KEYS DID NOT MATCH");
            // reset key
        }
        GameController.instance.SetObjectiveText();
        startPoint.SetActive(true);
    }

    public void NotOnLevel(){
        GameController.instance.objective = "Choose a path";
    }

    public void LevelOne(){
        GameController.instance.keyNeeded = "Key_Red";
        GameController.instance.objective = "Obtain the Red Key";
    }

    public void LevelTwo(){
        GameController.instance.keyNeeded = "Key_Yellow";
        GameController.instance.objective = "Obtain the Yellow Key";
    }

    public void LevelThree(){
        GameController.instance.keyNeeded = "Key_Blue";
        GameController.instance.objective = "Obtain the Blue Key";
    }

    public void LevelFour(){
        GameController.instance.keyNeeded = "Key_Green";
        GameController.instance.objective = "Obtain the Green Key";
    }

    public void Escape(){
        GameController.instance.UnlockDoor();
        GameController.instance.objective = "Escape";
    }

}
