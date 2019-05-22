using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]

/*o tilemap tem um tamanho particular menor que a area
da tela. essa classe ajusta ele de modo a se encaixa em qqr tela,
independente do aspect ratio: o tileMap sempre ocupará a area visivel da camera por completo */
public class AdjustTilesMapSize : MonoBehaviour {

  	private void Awake() {
		AdjustSize();
	}
	private void AdjustSize(){
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
				
		/*nao é ortodoxo, essa formula vale pro meu tilemap particular,
		de modo que ele se enquadrará com essa formula em qqr aspect ratio */
		float width = cam.aspect + cam.aspect/2;
		height = (height/10)/4 + height/10;
		GetComponent<Tilemap>().transform.localScale = new Vector3(width,height,0);
		AddExcess();
		/*se eu ad o collider, o spawn point fica estranho ver isso jaja */
		//gameObject.AddComponent<TilemapCollider2D>();
	}
	private void AddExcess(){
		/*qd fica do tamanho exato da tela,
		as vezes por causa da velocidade de deslocamento,
		é possivel ver um serrilhado..esse metodo o previne  mexendo no eixo y, um micro offset*/
		Vector3 aux = GetComponent<Tilemap>().transform.localScale;
		aux.y += 0.03f;
		GetComponent<Tilemap>().transform.localScale = aux;
	}
}
