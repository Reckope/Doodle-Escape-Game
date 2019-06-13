/* Author: Joe Davis
 * Project: Doodle Escape
 * 2019
 * Notes:
 * This is used to switch between each scene.
 */

using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScene : MonoBehaviour {

	// Components
	public VideoPlayer IntroCutscene;
	public AudioSource menuMusic;
	public GameObject Menu;

	// Global Variables
	[SerializeField]
	private string gameScene = "Game";
	[SerializeField]
	private string rulesScene = "How To Play";
	[SerializeField]
	private string creditsScene = "Credits";
	[SerializeField]
	private string mainMenuScene = "Main Menu";

	// ---------------------------------------------------------------------------------
	// Before starting the game, play the intro cutscene. 
	public void PlayCutscene(){
		menuMusic.Stop();
		Menu.SetActive(false);
		IntroCutscene.Play();
		IntroCutscene.loopPointReached += StartGameScene;
	}

	// Start the Game
	public void StartGameScene(VideoPlayer vp){
		SceneManager.LoadScene(this.gameScene);
	}

	// Show rules
	public void StartHowToPlayScene(){
		SceneManager.LoadScene(this.rulesScene);
	}

	// Show credits
	public void StartCreditsScene(){
		SceneManager.LoadScene(this.creditsScene);
	}

	// Show main menu
	public void StartMainMenuScene(){
		SceneManager.LoadScene(this.mainMenuScene);
	}
}