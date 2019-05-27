// Still need to store UI objects in arrays.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public GameObject[] UI;

    const float FADE_SPEED = 1.2f;
    const float HELP_TEXT_DISPLAY_TIME = 5f;
    const float OBJECTIVE_TEXT_DISPLAY_TIME = 15f;
    public GameObject objectiveTextHolder;
    public GameObject helpTextHolder;
    public Text helpText;
    public Text objectiveText;
    private bool fadeIn, fadeOut, helpTextIsActive, objectiveTextIsActive;
    private float textOpacity, helpTextTimer, objectiveTextTimer;

    // Start is called before the first frame update
    void Start(){
        objectiveTextHolder.SetActive(true);
        helpTextHolder.SetActive(false);
        textOpacity = 0f;
        objectiveText.text = null;
    }

    // Update is called once per frame
    void Update(){
        FadeObjectiveText();
        HelpTextDisplayTime();
        ObjectiveTextDisplayTime();
    }

    public void DisplayObjectiveText(string objective){
        objectiveText.text = objective;
        textOpacity = 0f;
        fadeOut = false;
        fadeIn = true;
        objectiveTextTimer = 0;
        objectiveTextIsActive = true;
    }

    private void ObjectiveTextDisplayTime(){
        if(objectiveTextIsActive && objectiveTextTimer <= OBJECTIVE_TEXT_DISPLAY_TIME){
            objectiveTextTimer += Time.deltaTime;
        }
        else{
            HideObjectiveText();
            return;
        }
    }

    public void HideObjectiveText(){
        fadeIn = false;
        fadeOut = true;
    }

    public void FadeObjectiveText(){
        objectiveTextHolder.GetComponent<CanvasGroup>().alpha = textOpacity;
        if(fadeIn && textOpacity < 1){
            textOpacity += Time.deltaTime * FADE_SPEED;
        }
        if(fadeOut && textOpacity > -1){
            textOpacity -= Time.deltaTime * FADE_SPEED;
        }
    }

    public void DisplayHelpText(string help){
        helpText.text = help;
        helpTextHolder.SetActive(true);
        helpTextIsActive = true;
        helpTextTimer = 0;
    }

    private void HelpTextDisplayTime(){
        if(helpTextIsActive && helpTextTimer <= HELP_TEXT_DISPLAY_TIME){
            helpTextTimer += Time.deltaTime;
        }
        else{
            helpTextHolder.SetActive(false);
            return;
        }
    }
}
