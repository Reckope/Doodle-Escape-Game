/* Author: Joe Davis
 * Project: Doodle Escape
 * Code QA Sweep: DONE - 10/06/19
 * Notes:
 * This is an intermediary script. This controls the main flow of the game, and
 * contains the key methods that other classes can call / pass params through. 
 */

using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Static instance of GameController which allows it to be accessed by any other script.
	public static GameController instance;

	// Scripts
	LevelManager LevelManager;
	UIController UIController;
	CameraController CameraController;
	CinematicBars CinematicBars;
	Player Player;

	// GameObjects & components
	public VideoPlayer endCutscene;
	public GameObject[] keys;
	public Transform[] spawnPoints;
	private GameObject key;
	GameObject Door;

	// Global Variables
	const int FINISHED_LEVEL_VALUE = -1;
	const int END_TRANSITION_POINT = -15;
	public int numberOfKeysRemaining;
	public bool escaping;
	public bool inTransition;

	// ---------------------------------------------------------------------------------
	void Awake () {
		Door = GameObject.Find("Door");
		if(Door == null){
			Door.SetActive(true);
		}
		LevelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		UIController = GameObject.FindObjectOfType(typeof(UIController)) as UIController;
		CameraController = GameObject.FindObjectOfType(typeof(CameraController)) as CameraController;
		CinematicBars = GameObject.FindObjectOfType(typeof(CinematicBars)) as CinematicBars;
		Player = GameObject.FindObjectOfType(typeof(Player)) as Player;
		GameSettings();
		SpawnKeys();
		escaping = false;
		inTransition = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(inTransition){
			DuringTransition();
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
		numberOfKeysRemaining = 0;
        for(int x = 0; x < keys.Length; x++){
            key = (GameObject)Instantiate (keys[x], spawnPoints[x].position, Quaternion.identity);
            GameController.instance.numberOfKeysRemaining++;
        }
    }

	// When the player collects a key, warp the key to it's allocated lock
	// and decrement the total remaining by 1. 
	public void CollectedKey(string keyColour){
		numberOfKeysRemaining--;
		WarpKeyToLock(keyColour);
		LevelManager.CompletedLevel(LevelManager.currentLevel);
	}

	private void WarpKeyToLock(string keyColour){
		GameObject key = GameObject.FindGameObjectWithTag(keyColour);
		GameObject keyLock = GameObject.Find(keyColour + "_Lock");
		key.transform.position = new Vector2(keyLock.transform.position.x, keyLock.transform.position.y);
	}

	public void CompleteLevelHelpText(){
		string helptext;
		string keyPlural;
		string keyVerb;

		if(numberOfKeysRemaining == 1){
			keyPlural = "Key";
			keyVerb = "is";
		}
		else{
			keyPlural = "Keys";
			keyVerb = "are";
		}
		helptext = "You've obtained a key! There " + keyVerb + " now " + numberOfKeysRemaining + " remaining. Return to the start position and track down the remaining " + keyPlural + ".";
		if(numberOfKeysRemaining == 0){
			helptext = "All the keys have been collected! The door is now unlocked.";
		}
		SetHelpText(helptext);
	}

	public void UnlockDoor(){
		if(Door != null){
			Door.SetActive(false);
		}
	}

	public bool GameOver(int causeOfDeath){
		UIController.DisplayGameOverUI(causeOfDeath);
		// sad music
		return true;
	}

	// We set the UI strings by passing paramaters from various methods,
	// then tell the UIController to display that string. 
	public void SetObjectiveText(string objective){
		if(objective != null){
			UIController.DisplayObjectiveText(objective);
		}
		else{
			UIController.HideObjectiveText();
		}
	}

	public void SetHelpText(string helpText){
		if(helpText != null){
			UIController.DisplayHelpText(helpText);
		}
	}

	// Need to detect what killed the player by getting the Layermask value
	// and setting the causeOfDeath to that value. 
	public int FindDeathReason(Collision2D Col){
		int causeOfDeath;

		if(Col.gameObject.layer == LayerMask.NameToLayer("Lava")){
			causeOfDeath = LayerMask.NameToLayer("Lava");
		}
		else if(Col.gameObject.layer == LayerMask.NameToLayer ("Bullet")){
			causeOfDeath = LayerMask.NameToLayer ("Bullet");
		}
		else if(Col.gameObject.layer == LayerMask.NameToLayer ("SpikeBall")){
			causeOfDeath = LayerMask.NameToLayer ("SpikeBall");
		}
		else if(Col.gameObject.layer == LayerMask.NameToLayer ("Guard")){
			causeOfDeath = LayerMask.NameToLayer ("Guard");
		}
		else if(Col.gameObject.layer == LayerMask.NameToLayer ("Dog")){
			causeOfDeath = LayerMask.NameToLayer ("Dog");
		}
		else{
			causeOfDeath = UIController.DEFAULT_DEATH_REASON;
		}
		return causeOfDeath;
	}

	// When escaping, perform various actions before, during and after the
	// transition down to the lower gound. 
	public void StartTransition(){
		escaping = true;
		inTransition = true;
	}

	public void DuringTransition(){
		var playerPosition = GameObject.Find("Player").transform.position.y;

		Player.PlayerEscapeTransition();
		CinematicBars.ShowCinematicBars();
		if(playerPosition < END_TRANSITION_POINT){
			EndTransition();
		}
	}

	public void EndTransition(){
		Debug.Log("END");
		inTransition = false;
		Player.PlayerEndTransition();
		CinematicBars.HideCinematicBars();
	}

	// Once the player reaches the escape trigger, play the final cutscene
	// and end the game. 
	public void PlayFinalCutscene(){
		UIController.HideAllUI();
		Player.PlayerCompleteGame();
		LevelManager.StopLevelMusic(LevelManager.levelMusic[LevelManager.currentLevel]);
		endCutscene.Play();
		endCutscene.loopPointReached += CompleteGame;
	}

	public void CompleteGame(VideoPlayer vp){
		CinematicBars.HideCinematicBars();
		UIController.DisplayGameCompleteUI();
	}
}