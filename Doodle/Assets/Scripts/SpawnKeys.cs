using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKeys : MonoBehaviour {

	// GameObjects
	public GameObject[] keys = new GameObject[4];
	private Vector2[] keyLocation = new Vector2[4];
	private GameObject key;

	void Start () {
		InstantiateKeys();
	}

	// Spawn those keys in! 
	private void InstantiateKeys(){
		keyLocation [0] = new Vector2(-11.19f, 10.2f);
		keyLocation [1] = new Vector2(27.61f, 7.15f);
		keyLocation [2] = new Vector2(-16.73f, -13.20f);
		keyLocation [3] = new Vector2(28.56f, -15.3f);

		for(int x = 0; x < keys.Length; x++){
			key = (GameObject)Instantiate (keys[x], keyLocation[x], Quaternion.identity);
			GameController.instance.numberOfKeys++;
		}
	}
}