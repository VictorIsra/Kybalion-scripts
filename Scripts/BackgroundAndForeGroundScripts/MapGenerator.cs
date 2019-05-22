using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/* dinamicamente criará um tilemap nos pontos desejados
 */

public class MapGenerator : MonoBehaviour {

	[SerializeField] Tilemap tilemap;
	
	void OnBecameVisible(){
		foreach(Transform tileSpawnPoint in transform)
			InstantiateTileMap(tileSpawnPoint);
	}
	private void InstantiateTileMap(Transform tileSpawnPoint){
		Tilemap tileMap = Instantiate(tilemap, tileSpawnPoint.position,Quaternion.identity);
		if(gameObject.transform.parent.gameObject){
			tileMap.transform.SetParent(tileSpawnPoint.transform);
		}
	}

}
