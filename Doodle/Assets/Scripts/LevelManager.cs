/* Author: Joe Davis
 * Project: Doodle Escape.
 * References: [1]
 * Code QA Sweep: DONE - 31/05/19
 * Notes:
 * This is a intermediary script, used to read and manage the level data. 
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure for the level.
[Serializable]
public struct Level{
    public string id;
    public string name;
    public string objective;
    public int buildIndex;
    public bool isActive;
    public bool completed;
}

// Data collection for the level. 
[Serializable]
public class LevelDataCollection {
    public Level[] levels;
}

public class LevelManager : MonoBehaviour{
    // Other classes
    Levels Levels;
    UIController UIController;
    LevelDataCollection allLevels;

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];
    GameObject startPoint;

    // Global Variables
    const int NOT_ON_LEVEL_BUILD_INDEX = 0;
    const int FINAL_LEVEL_BUILD_INDEX = 5;
    const string LEVEL_DATA_JSON_PATH = "/Scripts/Data/LevelData.json";
    public static int currentLevel;
    public static string currentObjective;
    private string applicationDataPath, jsonData;

    // ---------------------------------------------------------------------------------
    void Start(){
        Levels = GameObject.FindObjectOfType(typeof(Levels)) as Levels;
        UIController = GameObject.FindObjectOfType(typeof(UIController)) as UIController;
        startPoint = GameObject.Find("NotOnLevel");
        applicationDataPath = Application.dataPath;
        DisableLevelBlockers();
        jsonData = File.ReadAllText(Application.dataPath + LEVEL_DATA_JSON_PATH);
        allLevels = JsonUtility.FromJson<LevelDataCollection>(jsonData);
    }

    // Start each level depending on what trigger was activated, and block the player from exiting. 
    public void ActivateLevel(int levelID){
        // Disable NotOnLevel once another level has been actived. 
        if(levelID != NOT_ON_LEVEL_BUILD_INDEX){
            allLevels.levels[NOT_ON_LEVEL_BUILD_INDEX].isActive = false;
            Debug.Log("Disbaled Level 0");
        }
        if(GameController.instance.numberOfKeysRemaining >= 1){
            ActivateLevelBlocker(levelID);
        }
        else{
            levelID = FINAL_LEVEL_BUILD_INDEX;
        }
        LoadLevelDataFromJsonFile(levelID);
        GameController.instance.SetObjectiveText(currentObjective);
    }

    // Once we know what level has been triggered, we load the data for that level from 
    // the Json file and pass it over to Levels.cs.
    private void LoadLevelDataFromJsonFile(int levelActivated){
        string id, name, objective;
        int buildIndex;
        bool isActive = true;

        id = allLevels.levels[levelActivated].id;
        name = allLevels.levels[levelActivated].name;
        objective = allLevels.levels[levelActivated].objective;
        buildIndex = allLevels.levels[levelActivated].buildIndex;
        allLevels.levels[levelActivated].isActive = isActive;
        Debug.Log("Level: " + levelActivated + " is active: " + isActive);
        
        Levels.LaunchLevel(id, name, objective, buildIndex, isActive);
    }

    private void ActivateLevelBlocker(int level){
        Debug.Log(level);
        if(levelBlockers[level] != null){
            levelBlockers[level].SetActive(true);
        }
        else{
            Debug.Log("Level Blocker is NULL");
            return;
        }
    }

    // Once the player has collected a key, the game controller tells the level manager the level is complete. 
    // This can be any key collected, and any method can simply call this to complete the level it tells it to. 
    public void CompletedLevel(int levelID){
        DisableLevelBlockers();
        UIController.HideObjectiveText();
        GameController.instance.CompleteLevelHelpText();
        startPoint.SetActive(true);
        UpdateLevelData(levelID);
    }

    public void UpdateLevelData(int levelID){
        allLevels.levels[levelID].isActive = false;
        allLevels.levels[levelID].completed = true;
        Debug.Log("Level: " + levelID + " is active: " + allLevels.levels[levelID].isActive);
        Debug.Log("Level " + levelID + " is completed: " + allLevels.levels[levelID].completed);
    }

    private void DisableLevelBlockers(){
        for(int i = 0; i < levelBlockers.Length; i++){
            levelBlockers[i].SetActive(false);
        }
    }
}