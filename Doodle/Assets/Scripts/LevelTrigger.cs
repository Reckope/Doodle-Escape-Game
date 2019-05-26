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

    // Need to detect which level the players enters. Make sure level is 0 when exiting also. 
    private void OnTriggerEnter2D(Collider2D Col){
        string _levelActivated;

        if (Col.gameObject.tag == ("Player")) {
            _levelActivated = this.gameObject.tag;
            int.TryParse(_levelActivated, out levelActivated);
            this.gameObject.SetActive(false);
            LevelsScript.ActivateLevel(levelActivated);
            LevelsScript.ActivateLevelBlocker(levelActivated);
        }
    }
}
