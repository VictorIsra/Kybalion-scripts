using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEditor;

public class TileAutomata : MonoBehaviour {
	[Range(0,100)]
	public int initChance;

	[Range(1,8)]
	public int birthLimit;

	[Range(1,8)]
	public int deathLimit;
	
	[Range(1,10)]
	public int numR; //numero de repeticoes de rodar o algoritmo
	private int count = 0;

	private int[,] terrainMap;//1 vivo, 0 n preenchido
	public Vector3Int tmapSize;

	public Tilemap topMap;//fogo por ex
	public Tilemap bottonMap;//agua
	public Tile topTile;
	public Tile bottomTile;

	int width;
	int height;
	// Update is called once per frame

	public void DoSim(int numR){
		ClearMaps(false);
		width = tmapSize.x;
		height = tmapSize.y;

		if(terrainMap == null){
			terrainMap = new int[width,height];
			InitPos();
		}
		for(int i = 0; i < numR; i++){
			terrainMap = GenTilePos(terrainMap);
		}
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				if(terrainMap[x,y] == 1)
					topMap.SetTile(new Vector3Int(-x + width/2, -y + height/2,0),topTile);
					bottonMap.SetTile(new Vector3Int(-x + width/2, -y + height/2,0),bottomTile);
			}
		}
	}
	public int[,] GenTilePos(int[,] oldMap){
		int[,] newMap = new int[width,height];
		int neighb;
		BoundsInt myB = new BoundsInt(-1,-1,0,3,3,1);

		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				neighb = 0;
				foreach(var b in myB.allPositionsWithin){
					if(b.x == 0 && b.y == 0) continue;
					if(x+b.x > 0 && x+b.x < width && y+b.y >= 0 && y+b.y < height){
						neighb += oldMap[x + b.x, y + b.y];
					}
					else{
						neighb++;
					}
				}
				if(oldMap[x,y] == 1){
					if(neighb < deathLimit) newMap[x,y] = 0;
					else{
						newMap[x,y] = 1;
					}
				}	
				if(oldMap[x,y] == 0){
					if(neighb > birthLimit) newMap[x,y] = 1;
					else{
						newMap[x,y] = 0;
					}
				}	
			}
			
		}
		
		return newMap;
	}
	public void InitPos(){
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				terrainMap[x,y] = Random.Range(1,101) < initChance ? 1:0;
			}	
		}
		
		
	}
	public void ClearMaps(bool complete){
		topMap.ClearAllTiles();
		bottonMap.ClearAllTiles();
		if(complete)
			terrainMap = null;
	}
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			DoSim(numR);
		}
		if(Input.GetMouseButtonDown(1)){
			ClearMaps(true);
		}
		if(Input.GetMouseButtonDown(2)){
		//	SaveAssetMap();
			count++;
		}
	}
	/* public void SaveAssetMap(){
		string saveName = "tmapXY_" + count;
		var mf = GameObject.Find("Grid");
		if(mf){
			var savePath = "Assets/Resources/Prefabs/" + saveName + ".prefab";
			if(PrefabUtility.CreatePrefab(savePath,mf)){
				EditorUtility.DisplayDialog("SALVEI", "SALVO EM " + savePath,"Continue");

			}
			else
				EditorUtility.DisplayDialog("ZIKOOU", " N SALVO EM " + savePath,"Continue");

		}
	}*/
}
