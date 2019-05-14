/* Author: Joe Davis
 * Project: Doodle Escape
 * Date modified: 14/04/19
 * This is used to switch between each scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScene : MonoBehaviour {

	// Global Variables
	[SerializeField]
	private string gameScene = "Game";
	[SerializeField]
	private string rulesScene = "How To Play";
	[SerializeField]
	private string creditsScene = "Credits";
	[SerializeField]
	private string mainMenuScene = "Main Menu";

	// Use this for initialization
	void Start(){
	}

	// Start the Game
	public void StartGameScene(){
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