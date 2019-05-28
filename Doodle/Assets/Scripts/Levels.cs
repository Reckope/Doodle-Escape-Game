/* Author: Joe Davis
 * Project: Doodle Escape
 * Notes:
 * Would using an Abstract Class be more efficient here? 
 * Public abstract class Levels{}. 
 * Public class FetchLevel : Levels{}.
 * Need to experiment in next project.
 */

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

    // Start is called before the first frame update
    void Start(){
        startPoint = GameObject.Find("NotOnLevel");
        currentLevel = 0;
        StoreLevelsInList();
        DisableLevelBlockers();
    }

    delegate void StoreLevels();
    // First I need to have a list of levels. 
    // Store in order!
    // Tried a for loop, but could not convert string to void function type. 
    void StoreLevelsInList(){
        startLevels.Add(NotOnLevel); // 0
        startLevels.Add(LevelOne); // 1...
        startLevels.Add(LevelTwo);
        startLevels.Add(LevelThree);
        startLevels.Add(LevelFour);
    }

    // Start each level depending on what trigger was entered. 
    public void ActivateLevel(int levelID, string levelName){
        currentLevel = levelID;
        if(GameController.instance.numberOfKeysRemaining >= 1){
            startLevels[levelID]();
            Debug.Log("STARTED: " + levelName);
        }
        else{
            Escape();
        }
        GameController.instance.SetObjectiveText(objective);
    }

    // Activate and deactive level blockers once the player enters, and finishes the level. 
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
        objective = "Return to the start position";
        GameController.instance.SetObjectiveText(objective);
        startPoint.SetActive(true);
    }

    // -------------------- Levels --------------------

    public void NotOnLevel(){
        objective = "Choose a path";
    }

    public void LevelOne(){
        objective = "Obtain the Red Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelTwo(){
        objective = "Obtain the Yellow Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelThree(){
        objective = "Obtain the Blue Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelFour(){
        objective = "Obtain the Green Key";
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void Escape(){
        GameController.instance.UnlockDoor();
        objective = "Escape";
    }
}
