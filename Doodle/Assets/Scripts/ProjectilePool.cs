﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {

	public List<GameObject> pooledProjectiles;
	public GameObject projectileToPool;
	private int amountToPool;

	// Use this for initialization
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
			if(!pooledProjectiles[x].activeInHierarchy){	// Move onto the next projectile if the previous one is in use. 
				return pooledProjectiles[x];
			}
		}
		return null;
	}
}