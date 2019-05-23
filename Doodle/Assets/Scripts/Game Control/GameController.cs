using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Static instance of GameController which allows it to be accessed by any other script.
	public static GameController instance;

	// GameObjects & components
	GameObject Door;

	// Global Variables
	public int numberOfKeys;

	void Awake () {
		Door = GameObject.Find("Door");
		GameSettings();
		numberOfKeys = 0;
		if(Door == null){
			Door.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(numberOfKeys == 0){
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

	// When the player collects a key
	public void CollectedKey(string keyColour){
		GameObject key = GameObject.FindGameObjectWithTag(keyColour);
		GameObject keyEmpty;
		switch(keyColour){
			case "Key_Red":
				keyEmpty = GameObject.Find("Key_Red_Empty");
				key.transform.position = new Vector2(keyEmpty.transform.position.x, keyEmpty.transform.position.y);
			break;
			case "Key_Yellow":
				keyEmpty = GameObject.Find("Key_Yellow_Empty");
				key.transform.position = new Vector2(keyEmpty.transform.position.x, keyEmpty.transform.position.y);
			break;
			case "Key_Blue":
				keyEmpty = GameObject.Find("Key_Blue_Empty");
				key.transform.position = new Vector2(keyEmpty.transform.position.x, keyEmpty.transform.position.y);
			break;
			case "Key_Green":
				keyEmpty = GameObject.Find("Key_Green_Empty");
				key.transform.position = new Vector2(keyEmpty.transform.position.x, keyEmpty.transform.position.y);
			break;
			default:
				Debug.Log("no colour!");
			break;
		}
	}

	// Unlock the Door and escape!
	private void UnlockDoor(){
		if(Door != null){
			Door.SetActive(false);
		}
	}
}
