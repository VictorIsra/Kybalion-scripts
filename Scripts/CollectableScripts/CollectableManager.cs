using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
SORTEIA,DEFINE ATRIBUTOS E INSTANCIA NA CENA OS ITENS QUE TERÃO SEU COMPORTAMENTO DEFINIDO NA CLASSE:
				<CollectableBehaviour.cs>
 */
public class CollectableManager: MonoBehaviour{/* ,ISetLimitsInterface {
 
	[SerializeField] int lifeAmount;
	[SerializeField] float maxCashColetable = 750f;//maximo de grana que dá pra coletar
	[SerializeField] GameObject[] Levelweapons;//armas possiveis de serem sorteadas numa fase particular
	[SerializeField] GameObject[] collectableObjectsPrefabs;
	[SerializeField] private float spawnTime = 5f;
	private GameObject itemToSpaw;
	private float currentTime;
	private string tipo;
	private OrderedDictionary tipos;//dicionari bom q da pra referenciar por indice	
	private DelimitaMundo delimitaMundo;
	private PlayerStatus playerStatus;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	void Start () {
		playerStatus = FindObjectOfType<PlayerStatus>();
		delimitaMundo = FindObjectOfType<DelimitaMundo>();
		delimitaMundo.DelimitarArea(this,0.5f);
		generateDictionary();
		loadPossiveisWeapons();
		
	}
	
	private void FixedUpdate() {
		controlItens();
	}
	public void setLimits(float minX, float maxX, float minY, float maxY){
		this.minX = minX;
		this.maxX = maxX;
		this.minY = minY;
		this.maxY = maxY;
	}
	private void generateDictionary(){
		tipos = new OrderedDictionary();
		tipos.Add(0,Constantes.MUNICAOCOLETABLE);
		tipos.Add(1,Constantes.PONTUACAOCOLETABLE);
		tipos.Add(2,Constantes.VIDACOLETABLE);
	
	}
	private void controlItens(){
		currentTime +=Time.deltaTime;
		if(currentTime >= spawnTime){
			sortItemType();
			instantiateItemAndHandleIt();
			currentTime = 0;	
		}	
	}
	private void sortItemType(){
		int randIndex = Random.Range(0,tipos.Count);
		tipo = tipos[randIndex].ToString();
	}
	private Vector2 generatePosition(){
		return new Vector2(Random.Range(minX,maxX), Random.Range(minY,maxY));
	}
	private void instantiateItemAndHandleIt(){
		Vector2 vec;
		vec = generatePosition();
		if(tipo == Constantes.PONTUACAOCOLETABLE){
			itemToSpaw = Instantiate(collectableObjectsPrefabs[0],vec,Quaternion.identity);
			itemToSpaw.GetComponent<CollectableBehaviour>().setItemType(tipo);
		}
		else if(tipo == Constantes.VIDACOLETABLE){
			itemToSpaw = Instantiate(collectableObjectsPrefabs[1],vec,Quaternion.identity);
			itemToSpaw.GetComponent<CollectableBehaviour>().setItemType(tipo);
		}
		else if(tipo == Constantes.MUNICAOCOLETABLE){	
			itemToSpaw = Instantiate(collectableObjectsPrefabs[2],vec,Quaternion.identity);
			itemToSpaw.GetComponent<CollectableBehaviour>().setItemType(tipo);
		}
		//destruirá o item sorteado independente do caso:
		Destroy(itemToSpaw, spawnTime / 2);
		
			
	}
	
	//dinamicamente irá adicioar o collider que se adequará a forma do sprite sorteado.

	private void loadPossiveisWeapons(){
		//int offset = -1;
		//offset corrigira o offset dado pelas cenas iniciais
		//importantissimo!!!
		int lvlPathIndex = SceneManager.GetActiveScene().buildIndex;
		Levelweapons = Resources.LoadAll<GameObject>(Constantes.GAME_AMMOS_PATH + lvlPathIndex);
	
		if(Levelweapons.Length == 0 )
			Debug.LogWarning("NENHUMA ARMA ENCONTRADA NA PASTA " + Constantes.GAME_AMMOS_PATH + lvlPathIndex + " melhor verificar manualmente o conteudo dela");
	}
	 public GameObject sortWeapon(){
		 int index = Random.Range(0, Levelweapons.Length);
		 return Levelweapons[index];
	}
	public float sortLife(){
		float playerLife = playerStatus.GetInicialAmountLife();
		float amount = Random.Range(0.15f * playerLife, 0.75f * playerLife);
		return amount;
	}
	public float sortCash(){
		float amount = Random.Range( (0.15f * maxCashColetable), (0.75f * maxCashColetable));
		return amount;
	}*/
}

