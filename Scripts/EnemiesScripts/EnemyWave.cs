using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "EnemyWave")]
public class EnemyWave : ScriptableObject {
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject wavePathPrefab;
	[SerializeField] float timeBetweenSpawns;
	[SerializeField] float enemySpeed;
	[SerializeField] int enemiesNumber;
	
	public float GetTimeBetweenEnemiesSpawns(){
		return timeBetweenSpawns;
	}
	public float GetEnemySpeed(){
		return enemySpeed;
	}
	public int GetEnemiesNumber(){
		return enemiesNumber;
	}
	public GameObject GetWavePathPrefab(){
		return wavePathPrefab;
	}
	public GameObject GetEnemyPrefab(){
		return enemyPrefab;
	}
}
