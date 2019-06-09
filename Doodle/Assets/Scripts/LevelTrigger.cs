using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour{

    LevelManager LevelManager;

    private int levelActivated;
    const int ESCAPED_LEVEL = 5;

    // Start is called before the first frame update
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        this.gameObject.SetActive(true);
    }

    // Need to detect which level the players enters.
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
                GameController.instance.CompleteGame();
                LevelManager.UpdateLevelData(ESCAPED_LEVEL);
            }
        }
    }
}
