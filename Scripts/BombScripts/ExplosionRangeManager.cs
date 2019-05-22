using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider2D))]
public class ExplosionRangeManager : MonoBehaviour {
	//CircleCollider2D explosionCollider;
	[SerializeField] GameObject bombRef;
	private FadeScreenManager screenRef;
	/* só pra economizar as chamadas do bombRef */
	Bomb bomb;
	private bool fadaded = false;
	/*a id é única para cada objeto! Por isso a usarei */
	private List<int> thingsThatIcollided = new List<int>();
	
	void Awake(){
		SetRef();
	}
	
	private void SetRef(){
		bomb = bombRef.GetComponent<Bomb>();
		screenRef = FindObjectOfType<FadeScreenManager>();
	}
	//colisao baseada em layers!
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.layer == Constantes.ENEMY_LAYER || other.CompareTag(Constantes.BOSS) && !other.CompareTag(Constantes.ENEMYPROJETIL)){	
			CheckEnemyColision(other);
		}
		/*se colidiu com um container e tal */
		else if(other.gameObject.CompareTag(Constantes.SCENE_OBJECT)){
			CheckObjectColision(other);
		}
	}
	private void DestroyMe(){
		Destroy(gameObject);
	}
	private void CheckEnemyColision(Collider2D enemy){	
		/*lembre a id do enemy.gameobject que é unica! só do enemy ( colldier2d ) varia! atencao ;) */
			DamageDealer damageDealer = enemy.GetComponent<DamageDealer>();
			if(!damageDealer)
				return;
			/*garante que um determinado inimigo só ira colidir uma unica vez em cada bomba */
			if(!thingsThatIcollided.Contains(enemy.gameObject.GetInstanceID())){
				thingsThatIcollided.Add(enemy.gameObject.GetInstanceID());
				damageDealer.HitDueBombExplosion(bomb.bombSettings.bombCurrentPower);
			}		
	}

	private void CheckObjectColision(Collider2D sceneObject){
		if(sceneObject.gameObject.GetComponent<ObjectHandler>()){
			if(!thingsThatIcollided.Contains(sceneObject.gameObject.GetInstanceID())){
				thingsThatIcollided.Add(sceneObject.gameObject.GetInstanceID());
				sceneObject.gameObject.GetComponent<ObjectHandler>().RemoveHealth(bomb.bombSettings.bombCurrentPower);
			}
		}
		else
			Debug.LogWarning(Constantes.OBJECT_HANDLER_SCRIPT_REQUIRED);	
	}
	private void BlinkScreen(){
		if(screenRef && !fadaded){
			fadaded = true;
			screenRef.WhiteFate();
		}
	}
}
