/* Author: Joe Davis
 * Project: Doodle Escape.
 * Notes:
 * Don't use Coroutine as apparently it'll make it hard to track issue in a large project. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    // GameObjects
    public GameObject objectiveTextHolder;
    public GameObject helpTextHolder;
    public Text helpText;
    public Text objectiveText;

    // Global Variables
    const float FADE_SPEED = 1.1f;
    const float HELP_TEXT_DISPLAY_TIME = 8f;
    const float OBJECTIVE_TEXT_DISPLAY_TIME = 20f;
    public enum TextFade {fadeIn, fadeOut};
    private bool helpTextIsActive, objectiveTextIsActive;
    private float textOpacity, helpTextTimer, objectiveTextTimer;

    TextFade textFade;

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

    // ----- Fade effects and display times for the objective and help text -----
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
        if(textFade == TextFade.fadeIn && textOpacity < 1){
            textOpacity += Time.deltaTime * FADE_SPEED;
        }
        if(textFade == TextFade.fadeOut && textOpacity > -1){
            textOpacity -= Time.deltaTime * FADE_SPEED;
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
}
