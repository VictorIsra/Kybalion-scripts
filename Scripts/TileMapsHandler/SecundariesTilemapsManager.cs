using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*tilemaps secundarios, ou seja,basicamente
tudo que é tilemap mas que nao é "terreno":  ex de
tile secundarios: arvores, tiles que se sobrepoe etc,
essa classe guarda a ref pra eles, assim, qd na classe generegragrid
eu carregar a ref dos tilemaps, indiretamente estarei pegando a ref dos tilemaps secundarios
atraves dessa classe ;D */
public class SecundariesTilemapsManager : MonoBehaviour {
	private GameObject[] secMaps;
	private float height; /*height da tela, funciona indepenende do apsect ratio */
	
	private void Awake() {
		LoadSecMaps();
		height = 2f * Camera.main.orthographicSize;
				
	}
	private void LoadSecMaps(){
	 	secMaps = Resources.LoadAll<GameObject>("lvls/lvl1/interactiveLayer");
	}
	public void InstantiateSecMap(Vector3 spawnPoint, int mapIndex){
		Debug.LogWarning("ENTRADA (INDEX " + mapIndex + " sec len " + secMaps.Length);
		if(secMaps.Length > 0 && mapIndex < secMaps.Length){
		Debug.LogWarning("VO ISNTNACIA SEC");
			GameObject novoMapa = Instantiate(secMaps[mapIndex], spawnPoint, Quaternion.identity);
			novoMapa.AddComponent<DestroyMyParent>();
			ConfigTilemap(mapIndex,novoMapa);
		}
		else
			Debug.LogWarning("NADA A TENTAR");
	}
	private void ConfigTilemap(int index,GameObject novoMapa){
		/*configura os atributos dps de instancia-lo */
		novoMapa.GetComponent<TilemapRenderer>().sortingLayerName = "BackTerrain";
		novoMapa.GetComponent<TilemapRenderer>().sortingOrder = -index + 1;
		Tilemap tilemap = novoMapa.GetComponent<Tilemap>();
		
		tilemap.CompressBounds();
		Debug.LogWarning("SURIVVI");
		TileMapFinalConfigs(index,novoMapa);
	}
	private void ResizeBounds(Tilemap tilemap){
		/*fundamental redimensiono pra ficar fluido o spawn qd muda de um tile pro outro*/
		Vector3Int aux = tilemap.transform.parent.GetComponent<Tilemap>().origin;//tilemap.origin;
		//aux.y += 1;
		tilemap.origin = aux;/*muda origem pra ele refresh na linha debaixo ( resizebounds) */
		aux = tilemap.transform.parent.GetComponent<Tilemap>().size;//tilemap.size;
		aux.y += 2;
		tilemap.size = aux;
		tilemap.ResizeBounds();
	}
	/*configuracoes finais como setar parent e layerorder */
	private void TileMapFinalConfigs(int index, GameObject novoMapa){
//Debug.LogWarning("PAREN NOME " + transform.parent.name);
		novoMapa.transform.SetParent(transform);
		novoMapa.layer = 17;/*layer mapas */
		Debug.LogWarning("SERERRR" + index + " mapa " + novoMapa.name);
		ResizeBounds(novoMapa.GetComponent<Tilemap>());

		Vector3 aux = novoMapa.transform.position;
		aux.z = 0f;
		novoMapa.transform.position = aux;
	}
	private void AddExcess(){
		/*qd fica do tamanho exato da tela,
		as vezes por causa da velocidade de deslocamento,
		é possivel ver um serrilhado..esse metodo o previne  mexendo no eixo y, um micro offset*/
		Vector3 aux = GetComponent<Tilemap>().transform.localScale;
	//	aux.y += 0.03f;
		GetComponent<Tilemap>().transform.localScale = aux;
	}
	
}
