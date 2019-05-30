/* Author: Joe Davis
 * Project: Doodle Escape
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    LevelManager LevelManager;
    Level level = new Level();

    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
    }

    public void LaunchLevel(string levelID){
        Invoke(levelID, 0f);
        //level.DebugValues();
    }

    // -------------------- Levels --------------------

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
        level.SetLevel(LevelManager.levels[levelID].id, LevelManager.levels[levelID].name, LevelManager.levels[levelID].objective, LevelManager.levels[levelID].buildIndex, true);
        LevelManager.objective = LevelManager.levels[levelID].objective;
    }
}
