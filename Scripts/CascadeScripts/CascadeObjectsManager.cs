using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*sinaliza quando um objeto em cascata (cascadeEnemySpawner ou cascadeSpawner devem instanicar as coisas) */
public class CascadeObjectsManager : MonoBehaviour {
	[SerializeField] bool parentToContainer= true;/*default é ficar preso ao container/mapa
	mas no caso dos monstros por ex, isso será false */
	[SerializeField] bool cascadeExplosion = true; /*default é, se destruir o pai, os filhos se destuirem em cascata tb */
	// Use this for initialization
	private bool instantiated = false;
	void OnBecameVisible(){
		/*só fará uma vez, se nao, caso eu dê um zum na camera pra debug, ocorre erros! msm no jogo isso nunca iria acontencer, faco isso pra ficar direito */
		if(!instantiated){
			instantiated = true;
			foreach(Transform child in transform)
				TriggerCascades(child.gameObject);
		}	
	}
	private void TriggerCascades(GameObject child){
		if(child.GetComponent<CascadeEnemySpawner>()){
			child.GetComponent<CascadeEnemySpawner>().TriggerCascade();
			CreateParentPrefab(child.GetComponent<CascadeEnemySpawner>(),child.transform);
		}	
		if(child.GetComponent<CascadeSpawner>()){
			child.GetComponent<CascadeSpawner>().TriggerCascade();	
			CreateParentPrefab(child.GetComponent<CascadeSpawner>(),child.transform);
		}	
	}
	private void CreateParentPrefab<T>(T childScript,Transform childsTransform) where T : ICascade{
		/*cria o objeto que terá os spawns como filho,
		se eu detruir esse objeto, os filhos se destruirao em cascata xD(opcional) */
		if(childScript.GetParentPrefab()){
			 if(childScript.GetParentPrefab().GetComponent<ObjectHandler>()){
				
				List<Transform> childList = new List<Transform>();/*pq se faco direto do transform, ele muda o tamanho a medida que eu unparent algo...entao essa é as olucao */

			 	GameObject parentPrefab = Instantiate(childScript.GetParentPrefab(),childsTransform.transform.position,Quaternion.identity);
			 	SetParentBehaviour(parentPrefab);

				string parentLayer = parentPrefab.GetComponent<SpriteRenderer>().sortingLayerName;
				int  parentOrder = parentPrefab.GetComponent<SpriteRenderer>().sortingOrder;
				
				if(parentToContainer){
					parentPrefab.transform.SetParent(gameObject.transform);
				}	
				else/*caso dos monstros, que ficarão livres */{
					CheckIfMoves(parentPrefab,childScript);/*se entrar nesse else é pq é um monstro, e provavelmente se move..se se move, procure o compoennte enemymove e sete o path */
					parentPrefab.transform.SetParent(null);
				}	
				/*tenho que usar uma copia pq ao dar unparent, o transform perde a conta etc */
				foreach(Transform spawnedChild in childsTransform)
					childList.Add(spawnedChild);
				foreach(Transform spawnedChild in childList){
					spawnedChild.SetParent(parentPrefab.transform);
					AjustLayer(spawnedChild,parentLayer,parentOrder);
					CheckDestroyScript(spawnedChild.gameObject);
				}	
			 }
			 else
			 	Debug.LogWarning(Constantes.OBJECT_HANDLER_SCRIPT_REQUIRED);
		}
		else{
			/*caso nao tenha um pai, os objetos instanciados serao filhos do objeto que os envocou, que no caso vai ser um spawnpoint! */
		}
	}
	private void SetParentBehaviour(GameObject parentPrefab){
		/*aqui adiciono ao parent o scrip que irá destruí-lo da cena
		apenas qd seus filhos ja tiverem sido deletados(os filhos tem o DestroyOnBecomeInvisible atlelados ;) 
		e decido se dps que eu destruir o parent, os filhos explodem ou nao em cascata*/
		ObjectHandler scriptRef = parentPrefab.GetComponent<ObjectHandler>();
		if(cascadeExplosion){
			scriptRef.SetCascadeExplosionTrigger();
		}
		else{
			/*se flag tiver ativada....desativa pra instancia */
			if(scriptRef.GetCascadeExplosionFlag()){
				scriptRef.SetCascadeExplosionTrigger(false);/*desativo pra uma instancia caso o prefab tenha ela ativsda por default */			
			}
		}	

		parentPrefab.AddComponent<DestroyAfterChildsDestroyed>();		
	}
	private void CheckIfMoves<T>(GameObject parentPrefab,T cascadeEnemySpawner) where T: ICascade{
		if(parentPrefab.GetComponent<EnemyMoveManager>()){
			if(cascadeEnemySpawner.GetPathToFollow() != null){/*se for um monstro msm msm... */
				parentPrefab.GetComponent<EnemyMoveManager>().SetPath(cascadeEnemySpawner.GetPathToFollow());			
			}	
			else
				Debug.LogWarning(Constantes.MISSING_REF);		
		}
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	private void AjustLayer(Transform child,string parentLayer, int parentOrder){
		/*filhos sempre ficarão atras do pai */
		if(child.gameObject.GetComponent<SpriteRenderer>()){/*pois posso instanciar um map, que n tem sprite render por ex */
			child.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = parentLayer;
			child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = parentOrder -1;
		}
	}
	private void CheckDestroyScript(GameObject child){
		/*caso um filho nao tenha o script "destroyOnBecomeInvisible atrelado, irei adiciona-lo
		, é melhor isso do que definir isso nos prefabs, pois posso querer que um prefab hora seja normal, hora seja o centro de algo etc */
		if(!child.GetComponent<DestroyOnBecomeInvisible>())
			child.AddComponent<DestroyOnBecomeInvisible>();
	}
}
