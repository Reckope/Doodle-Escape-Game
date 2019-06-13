﻿/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * This is used to control the UI within the game. 
 * Don't use Coroutine as apparently it'll make it hard to track issues in a large project.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Structure for the death message. 
[Serializable]
public struct DeathMessageString{
    public int id;
    public string name;
    public string deathMessage;
}

// The array of death message strings. 
[Serializable]
public class DeathStringDataCollection {
    public DeathMessageString[] causeOfDeathStrings;
}

public class UIController : MonoBehaviour{

    DeathStringDataCollection allDeathStrings;

    // GameObjects
    public GameObject gameOverUI;
    public GameObject gameCompleteUI;
    public GameObject objectiveTextHolder;
    public GameObject helpTextHolder;
    public Text causeOfDeathDisplayText;
    public Text helpText;
    public Text objectiveText;

    // Global Variables
    public const int DEFAULT_DEATH_REASON = 14;
    const int MAX_ALPHA_VALUE = 1;
    const float FADE_TIME = 1.1f;
    const float HELP_TEXT_DISPLAY_TIME = 8f;
    const float OBJECTIVE_TEXT_DISPLAY_TIME = 20f;
    private enum TextFade {fadeIn, fadeOut};
    private bool helpTextIsActive, objectiveTextIsActive;
    private float textOpacity, helpTextTimer, objectiveTextTimer;

    TextFade textFade;

    // ---------------------------------------------------------------------------------
    void Start(){
        objectiveTextHolder.SetActive(true);
        helpTextHolder.SetActive(false);
        gameOverUI.SetActive(false);
        gameCompleteUI.SetActive(false);
        textOpacity = 0f;
        objectiveText.text = null;
        DisplayHelpText("There are 4 keys located within this prison. Find them all to escape.");
        // Load the data for the causes of death strings. 
        TextAsset txtAssetUI = (TextAsset)Resources.Load("CauseOfDeathData", typeof(TextAsset));
        String causeData = txtAssetUI.text;
        allDeathStrings = JsonUtility.FromJson<DeathStringDataCollection>(causeData);
    }

    // Update is called once per frame
    void Update(){
        FadeObjectiveText();
        HelpTextDisplayTime();
        //ObjectiveTextDisplayTime();
    }

    // Once the objective / help text is set by the GameController, that then tells
    // this to display them with the fade affect. 
    public void DisplayObjectiveText(string objective){
        objectiveText.text = objective;
        textOpacity = 0f;
        textFade = TextFade.fadeIn;
        objectiveTextTimer = 0;
        objectiveTextIsActive = true;
    }

    public void HideObjectiveText(){
        textFade = TextFade.fadeOut;
        objectiveTextIsActive = false;
    }

    public void DisplayHelpText(string help){
        helpText.text = help;
        helpTextHolder.SetActive(true);
        helpTextIsActive = true;
        helpTextTimer = 0;
    }

    // ---------- Fade effects for the objective and help text ----------

    private void ObjectiveTextDisplayTime(){
        if(objectiveTextIsActive && objectiveTextTimer <= OBJECTIVE_TEXT_DISPLAY_TIME){
            objectiveTextTimer += Time.deltaTime;
        }
        else{
            HideObjectiveText();
        }
    }

    private void FadeObjectiveText(){
        objectiveTextHolder.GetComponent<CanvasGroup>().alpha = textOpacity;
        if(textFade == TextFade.fadeIn && textOpacity < MAX_ALPHA_VALUE){
            textOpacity += Time.deltaTime * FADE_TIME;
        }
        if(textFade == TextFade.fadeOut && textOpacity > -MAX_ALPHA_VALUE){
            textOpacity -= Time.deltaTime * FADE_TIME;
        }
    }

    private void HelpTextDisplayTime(){
        if(helpTextIsActive && helpTextTimer <= HELP_TEXT_DISPLAY_TIME){
            helpTextTimer += Time.deltaTime;
        }
        else{
            helpTextHolder.SetActive(false);
        }
    }
    // --------------------------------------------------------------------

    public void HideAllUI(){
        objectiveTextHolder.SetActive(false);
        helpTextHolder.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void DisplayGameOverUI(int causeOfDeath){
        if(gameOverUI != null){
            HideAllUI();
            causeOfDeathDisplayText.text = LoadDeathStrings(causeOfDeath);
            gameOverUI.SetActive(true);
        }
        else{
            Debug.Log("Error: Can't find Game Over UI Object.");
        }
    }

    public void DisplayGameCompleteUI(){
        if(gameCompleteUI != null){
            HideAllUI();
            gameCompleteUI.SetActive(true);
        }
        else{
            Debug.Log("Error: Can't find Game Complete UI Object.");
        }
    }

    // Load the strings and return what message is needed. 
    private string LoadDeathStrings(int causeOfDeath){
        int id, stringArrayValue = 9;
        string name, deathMessage;

        id = causeOfDeath - stringArrayValue;
        //Debug.Log("ID: " + id);
        name = allDeathStrings.causeOfDeathStrings[id].name;
        deathMessage = allDeathStrings.causeOfDeathStrings[id].deathMessage;

        return deathMessage;
    }
}
