using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour {
	/*pra instanciar uma unica onda ou uma porrada delas rs */
	[SerializeField] List<EnemyWave> waves;
	/* pegar a posicao no momento da criacao, pq se eu fizer isso dinamico, o mapa
	anda e consequentemente o ponto de isntnacia tb hehe */
	private Vector2 spawnPostion;

	void OnBecameVisible(){
		SpawnWaves();
	}
	private void SpawnWaves(){
		spawnPostion = transform.position;
		foreach(EnemyWave wave in waves){
			StartCoroutine(WaitTimeBetweenEnemies(wave));
		}
	}
	IEnumerator WaitTimeBetweenEnemies(EnemyWave wave){
		for(int i = 0; i < wave.GetEnemiesNumber(); i++){
			ChangeEnemySettings(wave.GetEnemyPrefab(), wave);
			Instantiate(wave.GetEnemyPrefab(),spawnPostion,Quaternion.identity);
		yield return new WaitForSeconds(wave.GetTimeBetweenEnemiesSpawns());
		}
	}
	/* mudara a velocidade e o trajeto do inimigo! */
	private void ChangeEnemySettings(GameObject enemyPrefab, EnemyWave wave){
		enemyPrefab.GetComponent<EnemyMoveManager>().SetWaveBehaviour(wave);
	}

}
