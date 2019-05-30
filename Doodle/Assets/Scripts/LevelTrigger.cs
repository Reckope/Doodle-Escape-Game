using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour{

    LevelManager LevelManager;

    private int levelActivated;

    // Start is called before the first frame update
    void Start(){
        LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        this.gameObject.SetActive(true);
    }

    // Need to detect which level the players enters.
    private void OnTriggerEnter2D(Collider2D Col){
        string _levelActivated;

        if (Col.gameObject.tag == ("Player")) {
            _levelActivated = this.gameObject.tag;
            int.TryParse(_levelActivated, out levelActivated);
            this.gameObject.SetActive(false);
            LevelManager.ActivateLevel(levelActivated);
        }
    }
}
