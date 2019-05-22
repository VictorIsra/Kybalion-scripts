using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGrid : MonoBehaviour {
	private float height;
	private float width;
	private Camera cam;
	private GameObject[] tileMaps;
	private GameObject tileTeste;
	private GameObject mapRef;
	private Vector3 spawnPoint = new Vector3();
	private GameObject preRenderPoint;
	private GameObject grid; /*é o objeto parent */

	void Awake(){
		if(!LoadTileMaps())
			return;
		SetScreenAtributes();
		CreateGrid();
		for(int i = 0; i < tileMaps.Length; i++)
			CreateTileMapPreRenderPoint(i);
	}
	private void CreateGrid(){
		/*objeto pai que guardará os tilemaps e seus prerenderspoints */
		GameObject grid = new GameObject("Grid");
		grid.AddComponent<Grid>();
		grid.AddComponent<MoveGrid>();
		this.grid = grid;
	}
	/*carrega os tilemaps da faze e guardam num vetor
	,lembre que isso só carrega, nao os instancia, por isso é level ;D*/
	private bool LoadTileMaps(){
		tileMaps = Resources.LoadAll<GameObject>("lvls/lvl1/groundLayer");
		Debug.LogWarning(("QTS " + tileMaps.Length));

 		if(tileMaps == null){
		 	return false;
		 }		 
		else 
			return true;	
	}
	private void CreateTileMapPreRenderPoint(int index){
		/*pre renders que irao eventualmente colidir com o raycast da camera e isso fará um tilemap ser instanciado */	
		spawnPoint = Camera.main.ScreenToWorldPoint(new Vector2(cam.pixelWidth/2, index * cam.pixelHeight + cam.pixelHeight/2));
			
		preRenderPoint = new GameObject();	
		preRenderPoint.transform.SetParent(grid.transform);
		preRenderPoint.layer = 17;
		preRenderPoint.transform.position = spawnPoint;
		//preRenderPoint.AddComponent<DestroyOnBecomeInvisible>();
		preRenderPoint.AddComponent<BoxCollider2D>();
		preRenderPoint.GetComponent<BoxCollider2D>().size = new Vector2(5f,0.01f);
		preRenderPoint.name = "tileMapPreRenderPoint" + index.ToString();

	}
	/*raycast da camera ( CameraRayCast.cs ) que chamará essa funcao ao colidir com um prerender de um tilemap: */
	public void GenerateMap(string index){
			int indice = System.Convert.ToInt32(index);
			Vector3 spawnPoint = new Vector3();
			Debug.LogWarning("OK TO AKI E SOU " + indice);
			/*indice 0 instancia o tilemap no meio da tela, nos outros casos,
			instancia na segunda metade da tela (mais longe),dando ideia de mapa gigante/continuo */
			if(indice == 0)
				spawnPoint = Camera.main.ScreenToWorldPoint(new Vector2(cam.pixelWidth/2,cam.pixelHeight/2 +2));
			else
				spawnPoint = Camera.main.ScreenToWorldPoint(new Vector2(cam.pixelWidth/2, (cam.pixelHeight/2 ) + cam.pixelHeight));

			if(indice == tileMaps.Length -1)
				LastMapManager(indice);

			Debug.LogWarning("LALALA " + indice + " lalal le " + tileMaps.Length);	
			if(indice < tileMaps.Length){/*n sei pq mas só necessario qd o tilemap tem collider */
				Debug.LogWarning("VOU INSTANCIARAAA ");
				GameObject novoMapa = Instantiate(tileMaps[indice],spawnPoint, Quaternion.identity);
				novoMapa.AddComponent<SecundariesTilemapsManager>();
				novoMapa.GetComponent<SecundariesTilemapsManager>().InstantiateSecMap(spawnPoint,indice);	
				ConfigTilemap(indice,novoMapa);
			}			
	}
	private void LastMapManager(int index){
		/*no ultimo tilemap o grid para de se mexer e deleto os tiles abaixo e acima  do ultimo visivel */
		grid.GetComponent<MoveGrid>().SetStatus(false);
		Destroy(grid.transform.GetChild(index).gameObject);
		Destroy(grid.transform.GetChild(index-2).gameObject);
	}
	private void ConfigTilemap(int index,GameObject novoMapa){
		/*configura os atributos dps de instancia-lo */
		novoMapa.GetComponent<TilemapRenderer>().sortingLayerName = "BackTerrain";
		novoMapa.GetComponent<TilemapRenderer>().sortingOrder = -index;
		novoMapa.GetComponent<Tilemap>().CompressBounds();
	
		novoMapa.AddComponent<AdjustTilesMapSize>();

		TileMapFinalConfigs(index,novoMapa);
	}
	/*configuracoes finais como setar parent e layerorder */
	private void TileMapFinalConfigs(int index, GameObject novoMapa){
		novoMapa.transform.SetParent(grid.transform.GetChild(index));
		novoMapa.layer = 17;/*layer mapas */
		GameObject parent = novoMapa.transform.parent.gameObject;
		Vector3 aux = novoMapa.transform.position;
		aux.z = 0f;
		novoMapa.transform.position = aux;
		ResizeBounds(novoMapa.GetComponent<Tilemap>());
	}
	private void ResizeBounds(Tilemap tilemap){
		/*fundamental redimensiono pra ficar fluido o spawn qd muda de um tile pro outro*/
		Vector3Int aux = tilemap.origin;
		aux.y += 1;
	//	tilemap.origin = aux;/*muda origem pra ele refresh na linha debaixo ( resizebounds) */
		aux = tilemap.size;
		aux.y += 2;
		tilemap.size = aux;
		tilemap.ResizeBounds();
	}
	private void SetScreenAtributes(){
		cam = Camera.main;
 		height = 2f * cam.orthographicSize;
 		width = height * cam.aspect; 
	}
	
}