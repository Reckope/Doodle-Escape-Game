/* Author: Joe Davis
 * Project: Doodle Escape.
 * 2019
 * Notes:
 * This is used to store the projectile at the start of a game. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {

	// Game Objects
	public List<GameObject> pooledProjectiles;
	public GameObject projectileToPool;

	// Global Variables
	private int amountToPool;

	// ---------------------------------------------------------------------------------
	void Start () {
		amountToPool = 25;
		// Store the inactive projectiles in a pool, ready to be fired.
		pooledProjectiles = new List<GameObject>();
		for (int x = 0; x < amountToPool; x++){
			GameObject projectile = (GameObject)Instantiate(projectileToPool);
			projectile.SetActive(false);
			pooledProjectiles.Add(projectile);
		}
	}

	// Get the pooled objects
	public GameObject GetPooledProjectile(){
		for(int x = 0; x < pooledProjectiles.Count; x++){
			// Move onto the next projectile if the previous one is in use.
			if(!pooledProjectiles[x].activeInHierarchy){ 
				return pooledProjectiles[x];
			}
		}
		return null;
	}
}
