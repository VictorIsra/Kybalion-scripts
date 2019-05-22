using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* pra criar objetos como pontws e afins em quantidades dinamicas
por exemplo, posso spawnar uma ponte de comprimento 10..outra de 2 etc.. 
lembre pra ficar ideal, de alinhar os sprites conforme o que vc quer..ex: alinhas a esquerda*/
public class CascadeSpawner : MonoBehaviour, ICascade {
	[SerializeField] GameObject objectToSpawn;
	[SerializeField] GameObject tailSprite;/*se quiser que o ultimo seja diferente */
	[SerializeField] GameObject parentPrefab;/*sera o objeto pai de todos, que se destruido, desencadeara explosoes em cadeia xD ( deve ser alguem que tem o script objecthandler) */
	[SerializeField] int objectsNnumber = 1;
	[SerializeField] bool rotateTail = true;/*default é o tail ter a orientacao flipada caso seja vertical..se desativado, n será rotacionado */
	private float size;/*tamanho dos bounds do sprite..uso pra calcular o offset entre os spawns */
	private int counter = 0;/*numero de escolhas: ex spanwhorizontal e veticaltop dá 2, só marcar spawnleft dá 1 etc..conta qts booleanos marquei 
	/*posso querer criar horizontalmente ou vertical...isso determinará se pego
		os bonds x ou y..aqui definirei isto! obs: sempre desenho na horizontal, só fliparei o sprite no caso vertical */
	[SerializeField] bool spawnHorizontalRight = true;/*se nao for, implicitamente será vertical */
	[SerializeField] bool spawnHorizontalLeft;/*spawna a esquerda e horizontalmente */
	[SerializeField] bool spawnVerticalTop;//(spawna acima e verticalmente)
	[SerializeField] bool spawnVerticalBotton;/*spawna abaixo e verticalmente */
	private bool flip = false;/*flipará 90 graus se for casos verticais */ 
	

	void Awake(){
		 /*  CountFlags();
		SetBoundsDimensions();
		CreateChilds();*/
	}
	public GameObject GetParentPrefab(){
		return parentPrefab;
	}
	public void TriggerCascade(){
		CountFlags();
		SetBoundsDimensions();
		CreateChilds();
	}
	public GameObject GetPathToFollow(){
		/*como n quero que objetos que usem essa classe retornem nada,boto pra null
		pode parecer inutil mas isso dá estabilidade/impende erros caso eu passa atributs invalidos pro inspector */
		return null;
	}
	private void CountFlags(){
		/*dependendo de qts spawnpoint escolhe, ele iterará pra spawnar nas direcoes escolhoidas */
		if(spawnHorizontalLeft)
			counter++;
		if(spawnHorizontalRight)
			counter++;
		if(spawnVerticalTop)
			counter++;
		if(spawnVerticalBotton)
			counter ++;			
	}	
	private void SetBoundsDimensions(){
		if(objectToSpawn){/*só checagem de ref mesmo */
			if(objectToSpawn.GetComponent<SpriteRenderer>())
				/*msm no caso vertical, uso o componente X, pois ao girar eu nao mudo os valores absolutos dos bounds.
				verticalmente, só tenho que cuidar para que o offset (i*size) seja aplicado no eixo Y em vez de no X. */
				/*nao posso usar o bounds do collider pq este só é diferente de zero se tiver algo instanciado. assim, uso o sprite! mas cuidado, lembre de desenhar direito!! ( sem sobras) */
				size = objectToSpawn.GetComponent<SpriteRenderer>().sprite.bounds.size.x; //objectToSpawn.GetComponent<Collider2D>().bounds.size.x;
		}
	}
	private void CreateChilds(){
		for(int i = 0; i < objectsNnumber; i++){
			CalculatePosition(i,size);
		}
		ReorderChilds();
	}
	private void CalculatePosition(int i, float size){
		/*(i*size) dá o offset de onde o objeto será criado */
		Vector2 temp = new Vector2(0f,0f);
		if(spawnHorizontalRight){
			flip = false;
			if(i == 0)
				temp = transform.position;
			else	
				temp = new Vector2(transform.position.x + (i*size), transform.position.y);
			InstanTiateObject(temp);
		}	
		if(spawnVerticalTop){
			flip = true;
			if(i == 0)
				temp = transform.position;
			else	
				temp = new Vector2(transform.position.x, transform.position.y + (i*size));
			InstanTiateObject(temp);
		}	
		if(spawnHorizontalLeft){
			flip = false;
			if(i == 0){
				/*como o pivot é a esquerda, o i zero aqui ficaria "cortando" o ponto de origem. pra isso nao ocorrer, mudo o offset*/	
				temp = new Vector2(transform.position.x - ((size)), transform.position.y);
			}
			else
				temp = new Vector2(transform.position.x - ((i+1)*size), transform.position.y);
			InstanTiateObject(temp);
		}	
		if(spawnVerticalBotton){
			flip = true;
			if(i == 0)
				temp = new Vector2(transform.position.x, transform.position.y - ((size)));
			else	
				temp = new Vector2(transform.position.x, transform.position.y - ((i+1)*size));
			InstanTiateObject(temp);
		}
	}
	private void ReorderChilds(){
		/*reordena os filhos pra ficarem na ordem correta: primeiro os da direita,dps de cima,dps eaquerda dps de baixo */
		List<Transform> childs = new List<Transform>();
		foreach(Transform child in transform)
			childs.Add(child);
		
		int desireIndex = 0;
		for(int j = 0; j < counter; j ++){
			for(int i = 0; i < objectsNnumber; i++){
				childs[j + (i*counter)].transform.SetSiblingIndex(desireIndex);
				desireIndex++;
			}
		}
		if(tailSprite)
			AddtailSprite(childs);

		//Unparent();	
	}
	private void AddtailSprite(List<Transform> childs){
		int aux = 0;
		/*como inicialmente n tava ordenado, reassing o vetor, agora ordenado */
		childs.Clear();
		foreach(Transform child in transform)
			childs.Add(child);
		/*acha as ultimas posicoes */
		if(tailSprite){
			for(int i = 0; i < counter;i++){
				if(i == 0){
					aux = objectsNnumber - 1;	
				}
				else
					aux += objectsNnumber;
				/*destroi oq ta na posicao alvo e cria outro */	
				CreateTailObject(childs[aux], aux);
				Destroy(childs[aux].gameObject);
			}
		}
	}
	private void CreateTailObject(Transform child, int i){
		/*substitui o antigo por novo..nao mudo o sprite pq tem animation clip associado */
		Vector3 pos = child.position;
		Quaternion rotation = tailSprite.transform.rotation;
		if(rotateTail)/*default é esse...ms pode ser que queira que a ponta tenha orientacao padrao do sprite */
			rotation = child.rotation;

		GameObject objectSpawned = Instantiate(tailSprite,pos,rotation);
		objectSpawned.transform.SetParent(gameObject.transform);

	}
	private void InstanTiateObject(Vector2 spawnPoint){
		GameObject objectSpawned = Instantiate(objectToSpawn,spawnPoint,Quaternion.identity);
		/*o flip tem que ser antes de assinalar o parent, se nao distorce o sprite! ( isso é algo documentado...n sei pq mas isso só ocorre qd tentava spawnar dinamicamente pelo pool, mas enfim...faca isso que funciona xD) */
		if(flip){/* se for vertical, flipo o sprite, ja que sempre desenho horizontal */
			objectSpawned.transform.rotation = Quaternion.Euler(0f,0f,Constantes.VERTICAL_ANGLE);	
		}	
		objectSpawned.transform.SetParent(gameObject.transform);
	}
	
}
