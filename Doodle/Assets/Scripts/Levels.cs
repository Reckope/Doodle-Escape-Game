// Would using an Abstract Class be more efficient here? 
// Public abstract class Levels{}. 
// Public class FetchLevel : Levels{}.
// Need to experiment in next project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    List<StoreLevels> startLevels = new List<StoreLevels>();

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];
    GameObject startPoint;

    // Global Variables
    public int currentLevel;
    private string objective;
    private string keyNeeded;

    // Start is called before the first frame update
    void Start(){
        startPoint = GameObject.Find("Level0Trigger");
        currentLevel = 0;
        StoreLevelsInList();
        DisableLevelBlockers();
    }

    delegate void StoreLevels();

    // Store in order!
    void StoreLevelsInList(){
        startLevels.Add(NotOnLevel); // 0
        startLevels.Add(LevelOne); // 1...
        startLevels.Add(LevelTwo);
        startLevels.Add(LevelThree);
        startLevels.Add(LevelFour);
    }

    public void ActivateLevel(int levelID){
        currentLevel = levelID;
        if(GameController.instance.numberOfKeysRemaining >= 1){
            startLevels[levelID]();
            Debug.Log("STARTED: " + levelID);
        }
        else{
            Escape();
        }
        GameController.instance.SetObjectiveText(objective);
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
    // could do something like this in the future for bigger projects with level specific objects. 
    public void CompletedLevel(int level, string keyAcquired){
        if(keyNeeded == keyAcquired){
            Debug.Log("Completed Level: " + level);
            objective = "Return to the start position";
            keyNeeded = null;
        }
        else{
            //Debug.Log("KEYS DID NOT MATCH");
        }
        GameController.instance.SetObjectiveText(objective);
        startPoint.SetActive(true);
    }

    // -------------------- Levels --------------------

    public void NotOnLevel(){
        objective = "Choose a path";
    }

    public void LevelOne(){
        keyNeeded = "Key_Red";
        objective = "Obtain the Red Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelTwo(){
        keyNeeded = "Key_Yellow";
        objective = "Obtain the Yellow Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelThree(){
        keyNeeded = "Key_Blue";
        objective = "Obtain the Blue Key";
    }

    public void LevelFour(){
        keyNeeded = "Key_Green";
        objective = "Obtain the Green Key";
    }

    public void Escape(){
        GameController.instance.UnlockDoor();
        objective = "Escape";
    }

}
