/* Author: Joe Davis
 * Project: Doodle Escape
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure for the level (Can be accessed anywhere!! :D)
public struct Level{
    public string id;
    public string name;
    public string objective;
    public int buildIndex;
    public bool isActive;

    // Constructor
    public Level(string _id, string _name, string _objective, int _buildIndex, bool _isActive){
        id = _id;
        name = _name;
        objective = _objective;
        isActive = _isActive;
        buildIndex = _buildIndex;
        isActive = _isActive;
    }

    // Member Variables
    public void SetLevel(string _id, string _name, string _objective, int _buildIndex, bool _isActive){
        id = _id;
        name = _name;
        objective = _objective;
        isActive = _isActive;
        buildIndex = _buildIndex;
        isActive = _isActive;
    }

    public void DebugValues(){
        Debug.Log("ID is: " + id);
        Debug.Log("Name is: " + name);
        Debug.Log("Objective is: " + objective);
        Debug.Log("buildIndex is: " + buildIndex);
        Debug.Log("Active? is: " + isActive);
    }
}

public class Levels : MonoBehaviour{

    Level level = new Level();

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];
    GameObject startPoint;

    // Global Variables
    const int COMPLETED_LEVEL_INDEX = -1;
    public int currentLevel;
    private string objective;

    // First, I need to store the levels while the script instance is loading.
    Dictionary<int, Level> levels = new Dictionary<int, Level>();
    void StoreLevelsInDictionary(){
        levels.Add(-1, new Level("CompletedLevel", "Completed", "Return to the Start Point", -1, false));
        levels.Add(0, new Level("NotOnLevel", "I Thought this was the Bar?", "Choose a path" , 0, false));
        levels.Add(1, new Level("LevelOne", "Prison Walls are Never Built to Scale.", "Obtain the Red key", 1, false));
        levels.Add(2, new Level("LevelTwo", "As Light as a Dark Cell.", "Obtain the Yellow key", 2, false));
        levels.Add(3, new Level("LevelThree", "I Would Lava to Break Free.", "Obtain the Blue key", 3, false));
        levels.Add(4, new Level("LevelFour", "Becoming a Cellebrity.", "Obtain the Green key", 4, false));
        levels.Add(5, new Level("LevelFive", "Freedom", "Escape", 5, false));
    }

    void Awake(){
        StoreLevelsInDictionary();
    }

    void Start(){
        startPoint = GameObject.Find("NotOnLevel");
        currentLevel = 0;
        for(int i = 0; i < levelBlockers.Length; i++){
            DisableLevelBlocker(i);
        }
    }

    // Start each level depending on what trigger was entered, and block the player from exiting. 
    public void ActivateLevel(int levelID){
        currentLevel = levelID;
        if(GameController.instance.numberOfKeysRemaining >= 1){
            objective = levels[levelID].objective;
            ActivateLevelBlocker(levelID);
            CompleteLevel(false);
            SetLevel(levelID, true);
            Invoke(levels[levelID].id, 0f); // Use a Reflection here to check if the method exists??
        }
        else{
            Escape();
        }
        GameController.instance.SetObjectiveText(objective);
        levels[levelID].DebugValues();
    }

    public void ActivateLevelBlocker(int level){
        if(levelBlockers[level] != null){
            levelBlockers[level].SetActive(true);
        }
        else{
            return;
        }
    }

    // Once the player has collected a key, it tells the game the level is complete. 
    // This can be any key collected, but any method can simply call this to complete the level it tells it to. (S-Skip) ;) 
    public void CompletedLevel(int levelID){
        SetLevel(levelID, false);
        DisableLevelBlocker(levelID);
        CompleteLevel(true);
        objective = levels[COMPLETED_LEVEL_INDEX].objective;
        GameController.instance.SetObjectiveText(objective);
        GameController.instance.CompleteLevelHelpText();
        startPoint.SetActive(true);
        levels[levelID].DebugValues();
    }

    public void DisableLevelBlocker(int level){
        levelBlockers[level].SetActive(false);
    }

    // Tells the structure what level is currently active and not active. This can be useful for debug / data.  
    public void SetLevel(int levelID, bool isActive){
        level.SetLevel(levels[levelID].id, levels[levelID].name, levels[levelID].objective, levels[levelID].buildIndex, isActive);
    }

    // -------------------- Levels --------------------

    public void CompleteLevel(bool isActive){
        level.SetLevel(levels[COMPLETED_LEVEL_INDEX].id, levels[COMPLETED_LEVEL_INDEX].name, levels[COMPLETED_LEVEL_INDEX].objective, levels[COMPLETED_LEVEL_INDEX].buildIndex, isActive);
    }

    public void LevelOne(){
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelTwo(){
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelThree(){
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void LevelFour(){
        GameController.instance.SetHelpText("This is a test This is a test This is a test");
    }

    public void Escape(){
        int levelID = 5;

        GameController.instance.UnlockDoor();
        level.SetLevel(levels[levelID].id, levels[levelID].name, levels[levelID].objective, levels[levelID].buildIndex, true);
        objective = levels[levelID].objective;
    }
}
