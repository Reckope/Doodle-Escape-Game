using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour{

    Levels LevelsScript;

    private int levelActivated;

    // Start is called before the first frame update
    void Start(){
        LevelsScript = GameObject.FindObjectOfType(typeof(Levels)) as Levels;
        //levelActivated = 0;
    }

    // Need to detect which level the players enters.
    private void OnTriggerEnter2D(Collider2D Col){
        string _levelActivated;
        string levelName;

        if (Col.gameObject.tag == ("Player")) {
            _levelActivated = this.gameObject.tag;
            levelName = this.gameObject.name;
            int.TryParse(_levelActivated, out levelActivated);
            this.gameObject.SetActive(false);
            LevelsScript.ActivateLevel(levelActivated, levelName);
            LevelsScript.ActivateLevelBlocker(levelActivated);
        }
    }
}
