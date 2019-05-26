// Still need to store UI objects in arrays.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public GameObject[] UI;

    const float FADE_SPEED = 1.2f;
    public GameObject objectiveTextHolder;
    public GameObject helpTextHolder;
    public Text helpText;
    public Text objectiveText;
    private bool fadeIn, fadeOut;
    private float textOpacity;

    // Start is called before the first frame update
    void Start(){
        objectiveTextHolder.SetActive(true);
        textOpacity = 0f;
        objectiveText.text = null;
    }

    // Update is called once per frame
    void Update(){
        FadeObjectiveText();
    }

    public void DisplayObjectiveText(string objective){
        objectiveText.text = objective;
        textOpacity = 0f;
        fadeOut = false;
        fadeIn = true;
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
}
