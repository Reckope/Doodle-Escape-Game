/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * This is used to see what level is triggered by the player. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour{

    // Classes
    LevelManager LevelManager;

    // Global variables.
    private int levelActivated;
    const int ESCAPED_LEVEL = 5;

    // ---------------------------------------------------------------------------------
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        this.gameObject.SetActive(true);
    }

    // Need to detect which level the players triggers.
    private void OnTriggerEnter2D(Collider2D Col){
        string _levelActivated;

        if (Col.gameObject.tag == ("Player")) {
            if(this.gameObject.layer == LayerMask.NameToLayer("Level")){
                _levelActivated = this.gameObject.tag;
                int.TryParse(_levelActivated, out levelActivated);
                LevelManager.ActivateLevel(levelActivated);
                this.gameObject.SetActive(false);
            }
            else if(this.gameObject.tag == "EscapeTransition"){
                GameController.instance.StartTransition();
            }
            else if(this.gameObject.tag == "Escaped"){
                GameController.instance.PlayFinalCutscene();
                LevelManager.UpdateLevelData(ESCAPED_LEVEL);
            }
        }
    }
}
