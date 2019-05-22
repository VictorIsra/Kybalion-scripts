using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBoss : MonoBehaviour {
	[SerializeField] GameObject bossPrefab;
	private Vector3 spawnPoint;
	void OnBecameVisible(){
		if(bossPrefab){
			spawnPoint = transform.GetChild(0).position;
			GameObject boss = Instantiate(bossPrefab,spawnPoint,Quaternion.identity);
		}	
	}
}
