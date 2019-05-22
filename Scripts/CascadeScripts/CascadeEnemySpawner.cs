using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ANÁlogo ao cascadeSpawner.cs porém muito mais simples,pela simplicidade
e pelo fato do alinhamento ods inimigos serem verticais e n horizontais como seria o caso pressuposto no cascadespawner,
fiz numa classe a parte. por simplicidade e lógica*/
public class CascadeEnemySpawner : MonoBehaviour , ICascade{
	[SerializeField] GameObject enemyMiddle;
	[SerializeField] GameObject enemyEnd;/*n precisa do "start" pq ele que usa essa classe xD */
	[SerializeField] GameObject parentPrefab;/*objeto que sera o pai deles, será o fodao que qd eu desturir destruira os filhos em caedeia  */
	[SerializeField] bool spawnTop;//(spawna acima e verticalmente)
	[SerializeField] bool spawnBotton;/*spawna abaixo e verticalmente */
	[SerializeField] int objectsNnumber = 1;
	[SerializeField] GameObject pathToFollow;/*só pra caso o objeto/inimigo ande..escolhe um path pra ele */

	private float size;/*tamanho dos bounds do sprite..uso pra calcular o offset entre os spawns */
	private int counter = 0;
	
	void Awake(){
	/* 	CountFlags();
		SetBoundsDimensions();
		CreateChilds();*/
	}
	public GameObject GetParentPrefab(){
		return parentPrefab;
	}
	public GameObject GetPathToFollow(){
		return pathToFollow;
	}
	public void TriggerCascade(){
		CountFlags();
		SetBoundsDimensions();
		CreateChilds();
	}
	private void CountFlags(){
		/*dependendo de qts spawnpoint escolhe, ele iterará pra spawnar nas direcoes escolhoidas */
		if(spawnTop)
			counter++;
		if(spawnBotton)
			counter ++;			
	}	
	private void SetBoundsDimensions(){
		if(enemyMiddle){/*só checagem de ref mesmo */
			if(enemyMiddle.GetComponent<SpriteRenderer>())
				size = enemyMiddle.GetComponent<SpriteRenderer>().sprite.bounds.size.y; //objectToSpawn.GetComponent<Collider2D>().bounds.size.x;
		}
	}
	private void CreateChilds(){
		for(int i = 0; i < objectsNnumber; i++){
			CalculatePosition(i,size);
		}
		AddEndObject();/*muda o ultimo filho pro objeto da "cauda" */
	}
	/*essa classe é mais simples: só instancia ou acima ou abaixo, e sao casos mutualmente excludentes*/
	private void CalculatePosition(int i, float size){
		/*(i*size) dá o offset de onde o objeto será criado */
		Vector2 temp = new Vector2(0f,0f);
		
		if(spawnTop){
			if(i == 0)
				temp = transform.position;
			else	
				temp = new Vector2(transform.position.x, transform.position.y + (i*size));
			InstanTiateObject(temp);
		}	
		
		else if(spawnBotton){
			if(i == 0)
				temp = new Vector2(transform.position.x, transform.position.y - ((size)));
			else	
				temp = new Vector2(transform.position.x, transform.position.y - ((i+1)*size));
			InstanTiateObject(temp);
		}
	}
	private void InstanTiateObject(Vector2 spawnPoint){
		GameObject objeto = Instantiate(enemyMiddle,spawnPoint,Quaternion.identity);
		objeto.transform.SetParent(gameObject.transform);
	
	}	
	private void AddEndObject(){
		if(counter > 0){/*caso eu tenha esquecido de escolher top ou botton, nao de erro */
			/*substitui o antigo por novo..nao mudo o sprite pq tem animation clip associado */
			Transform tailChild = transform.GetChild(objectsNnumber - 1);/*pega ultimo filho */
			Vector3 pos = tailChild.position;
			Destroy(tailChild.gameObject);
			GameObject enemy = Instantiate(enemyEnd,pos,Quaternion.identity);
			enemy.transform.SetParent(gameObject.transform);

		}
		else
			Debug.LogWarning(Constantes.ASSING_SOMETHING);
	}
}
