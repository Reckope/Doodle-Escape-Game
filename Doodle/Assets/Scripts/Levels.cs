using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour{

    // Gameobjects
    public GameObject[] levelBlockers = new GameObject[5];

    // Global Variables
    public int currentLevel;

    // Start is called before the first frame update
    void Start(){
        currentLevel = 0;
        DisableLevelBlockers();
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void ActivateLevel(int level){
        currentLevel = level;
        Debug.Log("ACTIVATED: " + currentLevel);
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
            case -1:
            FinishedLevel();
            break;
            default:
            Debug.Log("ERROR: Couldn't find level number.");
            break;
        }
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

    public void FinishedLevel(){

    }

    public void NotOnLevel(){

    }

    public void LevelOne(){

    }

    public void LevelTwo(){

    }

    public void LevelThree(){

    }

    public void LevelFour(){

    }

}
