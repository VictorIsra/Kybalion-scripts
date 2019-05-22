using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* ESSA CLASSE DETERMINA O COMPORTAMENTO DE UMA PLATAFORMA:
SE ELA IRÁ FICAR EM LOOP, SE ELA IRÁ SER DESTRUIDA, SE DEVERÁ SPAWNAR ALGO NELA ETC */
public class TerrainMovementManager : MonoBehaviour {
	[SerializeField] float velocidadeDeslocamento = 0.5f;
	[SerializeField] bool loopAfterDesapear = true;
	[SerializeField] bool destroyAfterDesapear = false;
	private int loopTimes = 30;//caso queira loopar. definir quantas vezes
	private string behavior = Constantes.LOOP;//comportamento default
	private Vector2 originPosition;
	//GameObject xd;
	// Use this for initialization
	void Start () {
		originPosition = transform.position;
	//	instantiateOnCHilds();
	}
	
	void FixedUpdate () {
		MoveTerrain();
		manageTerrainBehaviorCheck();
	}
	/* void instantiateOnCHilds(){
		GameObject[] gameObjectTransformCopy =  new GameObject[gameObject.transform.childCount];
		int i = 0;
		foreach(Transform childs in gameObject.transform){
			gameObjectTransformCopy[i] = Instantiate(teste,childs.transform.position,Quaternion.identity);
			i++;
		}	
		//nao posso fazer direto acima porque acima estou iterando no transform,
		//entao se tento adicionar um filho a ele nessas condicoes, ele crasha a engine
		foreach(GameObject childs in gameObjectTransformCopy){
			childs.transform.parent = gameObject.transform;
		}

	}*/
	void MoveTerrain(){
		transform.Translate( Vector2.down * velocidadeDeslocamento * Time.deltaTime);

	}
	private void OnBecameInvisible() {
		behaviorOnInvisible(behavior);
	}
	private void OnBecameVisible() {
	}
	private void manageTerrainBehaviorCheck(){
		if(loopAfterDesapear){
			behavior = Constantes.LOOP;
			destroyAfterDesapear = false;
		}
		if(destroyAfterDesapear){
			behavior = Constantes.DESTROY;
			destroyAfterDesapear = true;

		}
	}
	private void loopBehaviorManager(){
		//ira definir se o loop será infinito ou uma quantidade finita de vezes apenas
		if(loopTimes > 0 ){
			transform.position = originPosition;
			loopTimes --;
		}	
		else {
			loopTimes = 0;
			Destroy(gameObject);
		}	

	}
	public void behaviorOnInvisible(string behavior){
		switch(behavior){//as constantes tao definidas em Constantes.cs
			case "LOOP":
				loopBehaviorManager();
				break;
			case "DESTROY":
				Destroy(gameObject);
				break;	
			default:
				Debug.LogWarning("NENHUM DOS CASES...CUIDADO E CHEQUE ESSE MÉTODO");	
				break;
		}
	}
}
