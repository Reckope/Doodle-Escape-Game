﻿/* Author: Joe Davis
 * Project: Doodle Escape
 * 2019
 * Notes:
 * Black bars are created via this script instead of using game objects. This
 * can be useful for future projects :) All you need to do is call ShowCinematicBars or HideCinematicBars.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : MonoBehaviour {

	public RectTransform topBar, bottomBar;
    const float BLACK_BAR_SIZE = 250f;
	const int ANCHOR_MIN = 0;
	const int ANCHOR_MAX = 1;
	private float changeSizeAmount;
	private float targetSize;
	private float barSpeed;
	public bool isActive;

	// ---------------------------------------------------------------------------------
	void Start () {
		CreateBars();
		barSpeed = 0.3f;
	}
	
	// Update is called once per frame
	private void Update () {
		if(isActive && !Player.isDead){
            Debug.Log("CINEMA");
			Vector2 sizeDelta = topBar.sizeDelta;
			sizeDelta.y += changeSizeAmount * Time.deltaTime;
			if(changeSizeAmount > ANCHOR_MIN){
				if(sizeDelta.y >= targetSize){
					sizeDelta.y = targetSize;
					isActive = false;
				}
			}
			else{
				if(sizeDelta.y <= targetSize){
					sizeDelta.y = targetSize;
					isActive = false;
				}
			}
			topBar.sizeDelta = sizeDelta;
			bottomBar.sizeDelta = sizeDelta;
		}
	}

	// Display the cinematic bars
	public void ShowCinematicBars(){
		float targetSize = BLACK_BAR_SIZE;
		this.targetSize = targetSize;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y) / barSpeed;
		isActive = true;
	}

	// Hide the cinematic bars
	public void HideCinematicBars(){
		targetSize = ANCHOR_MIN;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y) / barSpeed;
		isActive = true;
	}


	// Create the bars of the cinematic cam
	private void CreateBars(){
		GameObject barsObject = new GameObject("topBar", typeof(Image));
		barsObject.transform.SetParent(transform, false); // Scales the parent size in order to maintain this objects size.
		barsObject.GetComponent<Image>().color = Color.black;
		topBar = barsObject.GetComponent<RectTransform>();
		topBar.anchorMin = new Vector2(ANCHOR_MIN, ANCHOR_MAX);
		topBar.anchorMax = new Vector2(ANCHOR_MAX, ANCHOR_MAX);
		topBar.sizeDelta = new Vector2(ANCHOR_MIN, ANCHOR_MIN);

		barsObject = new GameObject("bottomBar", typeof(Image));
		barsObject.transform.SetParent(transform, false);
		barsObject.GetComponent<Image>().color = Color.black;
		bottomBar = barsObject.GetComponent<RectTransform>();
		bottomBar.anchorMin = new Vector2(ANCHOR_MIN, ANCHOR_MIN);
		bottomBar.anchorMax = new Vector2(ANCHOR_MAX, ANCHOR_MIN);
		bottomBar.sizeDelta = new Vector2(ANCHOR_MIN , ANCHOR_MIN);
	}
}
