/* Author: Joe Davis
 * Project: Doodle Escape.
 * References: [1]
 * Notes:
 * If I really needed to get level data from a .txt file, I could use 'TextAsset levelText'.
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
}

// Data collection for the level. 
[Serializable]
public class LevelDataCollection {
    public Level[] levels;
}

public class LevelManager : MonoBehaviour{
    Levels Levels;
    UIController UIController;

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];
    GameObject startPoint;

    // Global Variables
    const int FINAL_LEVEL_BUILD_INDEX = 5;
    const string LEVEL_DATA_JSON_PATH = "/Scripts/Data/LevelData.json";
    public static int currentLevel;
    public static string currentObjective;
    private string applicationDataPath;

    // ---------------------------------------------------------------------------------
    void Start(){
        Levels = GameObject.FindObjectOfType(typeof(Levels)) as Levels;
        UIController = GameObject.FindObjectOfType(typeof(UIController)) as UIController;
        startPoint = GameObject.Find("NotOnLevel");
        applicationDataPath = Application.dataPath;
        DisableLevelBlockers();
    }

    // Start each level depending on what trigger was activated, and block the player from exiting. 
    public void ActivateLevel(int levelID){
        currentLevel = levelID;
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
    void LoadLevelDataFromJsonFile(int levelActivated){
        string id, name, objective;
        int buildIndex;
        bool isActive;

        string displayData = File.ReadAllText(applicationDataPath + LEVEL_DATA_JSON_PATH);
        LevelDataCollection allLevels = JsonUtility.FromJson<LevelDataCollection>(displayData);

        id = allLevels.levels[levelActivated].id;
        name = allLevels.levels[levelActivated].name;
        objective = allLevels.levels[levelActivated].objective;
        buildIndex = allLevels.levels[levelActivated].buildIndex;
        isActive = allLevels.levels[levelActivated].isActive;
        
        Levels.LaunchLevel(id, name, objective, buildIndex, isActive);
    }

    public void ActivateLevelBlocker(int level){
        if(levelBlockers[level] != null){
            levelBlockers[level].SetActive(true);
        }
        else{
            Debug.Log("Level Blocker is NULL");
            return;
        }
    }

    // Once the player has collected a key, the game controller tells the level manager the level is complete. 
    // This can be any key collected, and any method can simply call this to complete the level it tells it to. (S-Skip) ;) 
    public void CompletedLevel(int levelID){
        DisableLevelBlockers();
        UIController.HideObjectiveText();
        GameController.instance.CompleteLevelHelpText();
        startPoint.SetActive(true);
    }

    public void DisableLevelBlockers(){
        for(int i = 0; i < levelBlockers.Length; i++){
            levelBlockers[i].SetActive(false);
        }
    }
}
