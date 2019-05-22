using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
/*nao só instancia inimigos como containers e itens em geral */
/* Por que nao instancio direto nos pontos que tem esse script ao inves de
criar outros pontos como filho desses para fazer a msm coisa depois?
simples: quero que o inimigo seja gerado sem que o jogador veja que ele "surgiu",
assim, ele será instanciado imeditamente após esses pontos "prerenders" serem vistos pela camera.
a ideia fica assim: qd um ponto prerender ( n tem imagem ) ficar visivel na camera,
imediatamente crie um inimigo "atras" desse ponto, assim o player terá a ilusao
de que o inimigo já estava lá, quando na verdade foi criado segundos antes
 */
public class InstantiateGroundEnemies : MonoBehaviour {
	[SerializeField] GameObject enemyPrefab;
	
	private void OnBecameVisible() {
			instantiateOnCHilds();
	}
	void instantiateOnCHilds(){
		GameObject[] gameObjectTransformCopy =  new GameObject[gameObject.transform.childCount];
		int i = 0;
		foreach(Transform childs in gameObject.transform){
			gameObjectTransformCopy[i] = Instantiate(enemyPrefab,childs.transform.position,Quaternion.Euler(GetRotationAngles()));
			i++;
		}	
		//nao posso fazer direto acima porque acima estou iterando no transform,
		//entao se tento adicionar um filho a ele nessas condicoes, ele crasha a engine
		foreach(GameObject child in gameObjectTransformCopy){
			child.transform.parent = gameObject.transform;
			CheckDestroyScript(child.gameObject);
		}
	}
	/*como certos inimigos terrenos sao "flipados" por mim, é preciso
	garantir isso na hora de instanciar. Pois se eu usar o quartenium.identity
	vai modificar os prefab flipados e tal */
	private Vector3 GetRotationAngles(){
		/*provavelmente nunca terá o else nesse jogo ams enfim... */
		if(enemyPrefab.GetComponent<Enemy>())
			return enemyPrefab.GetComponent<Enemy>().GetEulerAngles();
		else
			return new Vector3 (0,0,0);	
	}
	private void CheckDestroyScript(GameObject child){
		/*adiciona caso o objeto spawnada nao tenha o script...provavelmente vai ter, mas nao custa nada checar*/
		if (!child.GetComponent<DestroyOnBecomeInvisible>())
			child.AddComponent<DestroyOnBecomeInvisible>();
	}
}
