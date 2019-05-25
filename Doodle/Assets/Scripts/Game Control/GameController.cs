using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Static instance of GameController which allows it to be accessed by any other script.
	public static GameController instance;

	// Scripts
	Levels LevelsScript;

	// GameObjects & components
    public GameObject[] keys = new GameObject[4];
    public Transform[] spawnPoints = new Transform[4];
    private GameObject key;
	GameObject Door;

	// Global Variables
	const int FINISHED_LEVEL_VALUE = -1;
	public int numberOfKeysRemaining;

	void Awake () {
		Door = GameObject.Find("Door");
		if(Door == null){
			Door.SetActive(true);
		}
		LevelsScript = GameObject.FindObjectOfType(typeof(Levels)) as Levels;
		GameSettings();
		numberOfKeysRemaining = 0;
		SpawnKeys();
	}
	
	// Update is called once per frame
	void Update () {
		if(numberOfKeysRemaining == 0){
			UnlockDoor();
		}
	}

	// Instance / target framerate.
	private void GameSettings(){
		// Target frame rate is 60 fps
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 600;
		// Singleton Pattern: There can only ever be one instance of a GameController.
		if (instance == null){
			instance = this;
		}
		else if (instance != this){
			Destroy(gameObject);
		}
	}

	private void SpawnKeys(){
        for(int x = 0; x < keys.Length; x++){
            key = (GameObject)Instantiate (keys[x], spawnPoints[x].position, Quaternion.identity);
            GameController.instance.numberOfKeysRemaining++;
        }
    }

	// When the player collects a key
	public void CollectedKey(string keyColour){
		numberOfKeysRemaining--;
		GameObject key = GameObject.FindGameObjectWithTag(keyColour);
		GameObject keyLock = GameObject.Find(keyColour + "_Lock");
		key.transform.position = new Vector2(keyLock.transform.position.x, keyLock.transform.position.y);
		LevelsScript.DisableLevelBlockers();
		LevelsScript.ActivateLevel(FINISHED_LEVEL_VALUE);
	}

	public void UnlockDoor(){
		if(Door != null){
			Door.SetActive(false);
		}
	}

	// Game Over
	public bool GameOver(string causeOfDeath){
		// UI
		// Freeze everything
		// sad music
		return true;
	}

	public bool CompletedLevel(int level){
		return true;
	}
}
