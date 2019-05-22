using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Shadow))]

/*tb é usado por inimigos feitos em cascata, pois estes nem atiram e sao mais
como um "objeto do cenario" do que como um inimigo comum */
public class ObjectHandler : MonoBehaviour {

	[SerializeField] private float health;
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] GameObject[] item;
	[SerializeField] bool cascadeExplosion = false;//true se quiser que um pai gatilhe explosoes nos filhos
	[SerializeField] float cash = 0f;
	[SerializeField] bool destroyAfterExplode = false;/*item será destruido ou somente dps de sair da cena */
	[SerializeField] bool InstantiateItensOnChilds = true;/*qd explodir, sorteia o item(s) nos filhos...comportamento padrao */
	[SerializeField] bool floatingItem = false;/*se o item via flutuar ou ficar preso ao parent */
	private UiManagement uiManagement = null;
	private Animator animatorController;
	private Color spriteDefaultColor = Color.white;
	private Color spriteDamagedColor = Color.red;
	private bool destroyed = false;
	
	private int currentSortingLayerOrder;
	private string currentSortingLayerName;

	private void Awake() {
		ManageAnimator();
		GetSortingLayerInfo();
		SetReference();

		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}
	public void SetCascadeExplosionTrigger(bool cascadeExplode = true){
		/*nem todos que usam esse script precisam ter isso, mas caso eu queira, esse método ativa a flag por script */
		cascadeExplosion = cascadeExplode;
	}
	public bool GetCascadeExplosionFlag(){
		/*se ta ativo ou nao o tragger de destruir os filhos em cascata...
		pode ser que num prefab eu tenha botao que sim,mas num momento eu queira que nao ( em agum spawn em cascata por ex),essa funcao me permitira sabe se
		devo desativar a flag para uma dada instancia..se sim, a f acima o fará xD
		 */
		return cascadeExplosion;
	}
	void OnTriggerEnter2D(Collider2D other){
		
		AmmoHandler projectile  = other.GetComponent<AmmoHandler>();
		/* só importa o player e o projetil dele, qqr outra coisa que colidir com um inimigo será indiferente a ele */
		if(!projectile)
			return;
		/*checagem do destroye aqui é só pra o tiro atravessar o objeto depis de destrui-lo xD*/	
		if( other.gameObject.tag == Constantes.PLAYERPROJETIL && !destroyed){
			projectile.GetComponent<AmmoHandler>().InstantiateEffect();
		//	projectile.DestroyMe();	 
			RemoveHealth(projectile.GetDamage());
		}	
		
	}
	
	private void GetSortingLayerInfo(){
		currentSortingLayerOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
		currentSortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
	}
	private void SetReference(){
		if(cash > 0)
			uiManagement = FindObjectOfType<UiManagement>();
	}
	private void ManageAnimator(){
		if(gameObject.GetComponent<Animator>()){
			animatorController = GetComponent<Animator>();
			//animatorController.enabled = false;
		}	
	}
	public bool IsDestroyed(){
		return destroyed;
	}
	/*só inimigos em cascatas usarão isso */
	private void SendDestroyedFlag(){
		if(gameObject.transform.parent != null){
			/*saber que um filho foi destruido */
			if(gameObject.transform.parent.gameObject.GetComponent<CascadeEnemyManager>()){
				gameObject.transform.parent.gameObject.GetComponent<CascadeEnemyManager>().IncrementDestroyedCounter();
			}
		}
	}
	/*inimigos em cascata usam isso(cascadeenemymanagar) */
	public float GetHealth(){
		return health;
	}
	public void RemoveHealth(float damage){
		/*pra nao repetir a animacao qd uma bomba explodir por ex ou qd o player atirar nele dps de destrui-lo*/
		if(!destroyed){
			health -= Mathf.Round(damage);
			StartCoroutine(ChangeColor(0.1f));
			CheckHealth();
		}
	}
	private void CheckHealth(){
		if (health <= 0f){
			/*se for um objeto normal, que somente ele explode: */
			if(!cascadeExplosion)
				Explode();
			else{/*se for um objeto que quer se destruir e depois explodir os filhos em cascata */	
				Explode();
				StartCoroutine(CascadeExplosionCoroutine());
			}
		}	
	}
	private void Explode(){
		if(explosionPrefab){
			DestroyedHandler();
			AddCash();
			GameObject explosao = Instantiate(explosionPrefab,ExplosionSpawnPoint(),Quaternion.identity);
			SetExplosionSortingLayer(explosao);
			if(item.Length > 0)
				InstantiateItens();
			if(destroyAfterExplode)/*alguns objetos sao destruidos dps de explodir(parte de alguns monstro/naves),outros permanecem na cena(container,pontes) */
				Destroy(gameObject);	
			Destroy(explosao,1f);
			FireAnimation();
			ManageChildSortingOrder();
		}
		else/*caso esqueca de por o prefab rs */
			Debug.LogWarning(Constantes.MISSING_REF);	
	}
	private void DestroyedHandler(){
		destroyed = true;
		SendDestroyedFlag();
	}
	private Vector3 ExplosionSpawnPoint(){
		/*como alguns objetos tem seu sprite alinhado a esquerda, é preciso fazer 
		essa conta pra garantir que sempre fique no centro dele */
		Vector3 spawnPoint = transform.position;
		if(gameObject.GetComponent<SpriteRenderer>())
			/*centro do objeto independente do pivo e tal */
			spawnPoint = GetComponent<Renderer>().bounds.center;
		return spawnPoint;
	}
	private GameObject SetExplosionSortingLayer(GameObject explosao){
		/*pra se tiver algo na frente do objeto que explosiu. a explosao fique na frente deste */
		explosao.GetComponent<SpriteRenderer>().sortingLayerName = currentSortingLayerName;
		explosao.GetComponent<SpriteRenderer>().sortingOrder = currentSortingLayerOrder + 2;
		UpdateSortingLayer(explosao);

		return explosao;
	}
	/*tvz eu nem precise qd eu pensar direito nas layers e no que vai ser oq! */
	private void UpdateSortingLayer(GameObject explosao){
		/*pra caso tenha item, ele fique atras da explosoes */
		currentSortingLayerName = explosao.GetComponent<SpriteRenderer>().sortingLayerName;
		currentSortingLayerOrder = explosao.GetComponent<SpriteRenderer>().sortingOrder - 1;
	}
	private GameObject SetItemToSpawSortingLayer(GameObject itemToSpawn){
		itemToSpawn.GetComponent<SpriteRenderer>().sortingLayerName = currentSortingLayerName;
		itemToSpawn.GetComponent<SpriteRenderer>().sortingOrder = currentSortingLayerOrder;
		return itemToSpawn;
	}
	private void AddCash(){
		/* só existira a referencia se o valor do objeto for maior que zero hee */
		if(uiManagement){
			PlayerGlobalStatus.addPlayerCash(cash);
			uiManagement.UpdateScoreText();
		}	
	}
	private void InstantiateItens(){
		//sorteia qts spawnPoints;
		int childNumbers = transform.childCount;
		childNumbers -= IgnoredChilds();
		int itensToSpawn = Random.Range(1, childNumbers + 1);//pq é exclusivo
		GameObject itemToSpawn = null;
		/*pode instanciar em todos os filhos ou em uma uqantidade menor apenas rs */
			for(int i = 0; i < itensToSpawn; i++){
				if(childNumbers > 0 && InstantiateItensOnChilds){
					itemToSpawn = Instantiate(SetItemToSpawSortingLayer(item[SortItenIndex()]),transform.GetChild(i).position,Quaternion.identity);	
					if(!floatingItem)
						itemToSpawn.transform.SetParent(gameObject.transform);
				}
				else{/*se n tem filho ou n só quer spawnar no proprio objeto, é pq quer spawnar no "ar msm" ou tem  */
					itemToSpawn = Instantiate(SetItemToSpawSortingLayer(item[SortItenIndex()]),transform.position,Quaternion.identity);	
					if(!floatingItem)
						itemToSpawn.transform.SetParent(gameObject.transform);
				}
			}	
	}
	private int IgnoredChilds(){
		/*ignora os filhos que sao uma sombra,
		logo, esses pontos n serao levados em conta/enxergados como spawnpoints ;D */
		int ignoredChilds = 0;
		foreach(Transform child in transform){
			if(child.CompareTag(Constantes.SHADOW))
				ignoredChilds++;
		}
		return ignoredChilds;
	}
	/*acima sortiei qts spawn points, aqui qual iten(s) será(ao) instanciado(s): */
	private int SortItenIndex(){
		int sortedItemIndex = Random.Range(0, item.Length );
		return sortedItemIndex;
	}
	private void FireAnimation(){
		if(animatorController)
			animatorController.SetBool(Constantes.OBJECT_DESTROYED, true);
	}
	/*caso queira que um objeto pai ao ser destruido gatilhe explosoes em cascatas nos filhos */
	IEnumerator CascadeExplosionCoroutine(){
		/* destruirá em cadeia de modo a esperar 1s antes de explodir o proximo filho */
		yield return new WaitForSeconds(1);
		foreach(Transform child in gameObject.transform){
			if(child.gameObject.GetComponent<ObjectHandler>()){
				if(!child.gameObject.GetComponent<ObjectHandler>().IsDestroyed()){
					child.gameObject.GetComponent<ObjectHandler>().ClearHealthAndFireExplosion();
					yield return new WaitForSeconds(1);
				}
			}
			else
				Debug.LogWarning(Constantes.MISSING_REF);	
		}	
	}
	public void ClearHealthAndFireExplosion(){
		/* seta pra zero a vida ( usado nos filhos numa explosao em cascata) */
		health = 0;
		StartCoroutine(ChangeColor(0.1f));
		CheckHealth();
	}
	
	IEnumerator ChangeColor(float delay){	
		gameObject.GetComponent<SpriteRenderer>().color = spriteDamagedColor;
		yield return new WaitForSeconds(delay);
		gameObject.GetComponent<SpriteRenderer>().color = spriteDefaultColor;
	}	

	private void ManageChildSortingOrder(){
		/*nao é -2 e sim -3 pq qd chama essa f,
		o codigo ainda n atualizou a incrementacao do parent!! lembre disso!!!
		num mundo ideal sincronizado, aki seria sortingorder +2, mas por dif no sincronismo é +3 */
		/*todo filho sera 2 ("3") camada a frente */
		/*todo filho sera uma camada a frente */
		if(gameObject.GetComponent<SpriteRenderer>()){
			string myLayer = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
			int myOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
			
			foreach(Transform child in transform){
				if(child.GetComponent<Shadow>()){
					child.GetComponent<SpriteRenderer>().sortingOrder = myOrder +2;
					child.GetComponent<Shadow>().LayerAdjust();
				}
			}
		}
	}
}
