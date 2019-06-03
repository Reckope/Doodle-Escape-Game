﻿/* Author: Joe Davis
 * Project: Doodle Escape
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    LevelManager LevelManager;

    public Transform guard;

    // ---------------------------------------------------------------------------------
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
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
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
        Instantiate(guard, new Vector3(-24.08f, 2.87f, 0), Quaternion.identity);
        Instantiate(guard, new Vector3(-12.65f, 5.54f, 0), Quaternion.identity);
    }

    public void LevelTwo(){
        //GameController.instance.SetHelpText("This is a test This is a test This is a test");
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
