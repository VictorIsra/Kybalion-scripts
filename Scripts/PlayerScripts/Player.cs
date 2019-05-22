using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DamageDealer))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerItensManager))]
[RequireComponent(typeof(PlayerHealthManager))]
[RequireComponent(typeof(PlayerFireManager))]
[RequireComponent(typeof(PlayerMoveManager))]
[RequireComponent(typeof(PlayerWeaponsPartsManager))]
[RequireComponent(typeof(PlayerSpritesManager))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(Shadow))]

public class Player : MonoBehaviour {
	
	private PlayerFireManager playerFireManager;
	private PlayerHealthManager playerHealthManager;
	private PlayerSpritesManager playerSpritesManager;
	private PlayerMoveManager playerMoveManager;
	[SerializeField] private float playerHealth = 500f;
	//variáveis de controle e debug:
	[SerializeField] private bool imortal = false;
//	[SerializeField] Sprite[] playerSprites = new Sprite[3];

	private float tryAgainDelay = 1f;
	private bool alreadyMissedChanceOnScene = false;//como as partes upgrade sao players, sem esse controle, tem a chance de ser removida mais de uma chance por tentativa, o que nao é legal
	
	//variáveis que referenciam outros classes:
	private AudioManager audioManager;
	private UiManagement uiManagement;
	private Level level;
	private PlayerStatus playerStatus;
	private PlayerItensManager playerItensManager;
		
	void Awake() {
		 try{
			MouseVisibility.HideCursor();
			setReferences();
			SetPlayerHealth();//seta a vida em PlayerStatus.cs
		 }catch(System.Exception e){
			 Debug.LogWarning(e);
		 }
	}
	void FixedUpdate() {
		if(!playerStatus.getFreezePlayerControlStatus()){//atrelada ao evento de matar o chefao
			playerMoveManager.move();
		}	
		else
			playerMoveManager.FlyAway();
	}
	void Update () {
		if(!playerStatus.getFreezePlayerControlStatus()){
			fire();
			VolumeControl();
		}	
	}
	void OnCollisionStay2D(Collision2D other) {
		if( other.gameObject.tag == Constantes.obstaculoInimigo){
			DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
			if(!damageDealer)
				return;	
			ProcessHit(damageDealer);
		}
	}
	private void CheckTileMapColision(GameObject tileMap){
		/*criar meu tile */
		Tilemap tileMapComponent = tileMap.GetComponent<Tilemap>();
		Vector3Int pPos = tileMapComponent.WorldToCell(transform.position);
        Debug.LogWarning("pPos:" + pPos + " spite nome " + tileMapComponent.GetSprite(pPos));  	     
       
		tileMap.GetComponent<Tilemap>().SetTile(pPos,null);
	}
	void OnTriggerEnter2D(Collider2D other){	
		//if(other.gameObject.layer == 17)/*layer mapas */
		//	CheckTileMapColision(other.gameObject);

		DamageDealer damageDealer = other.GetComponent<DamageDealer>();
		AmmoHandler enemyProjectile = other.GetComponent<AmmoHandler>();

		//é importante... n tire esse if ( se n tiver nenhum dos dois);
		if(!damageDealer && !enemyProjectile)
			return;	
		/*independente do que colidiu, o player irá tomar um hit... */	
		if(damageDealer){
			if(damageDealer.CanHurt())/*pois pode ser que o inimigo ja tenha sido morto, entao n é pra me ferir/mudar minha cor mais */
				ProcessHit(damageDealer);
				
		}	
		else if(enemyProjectile)
			ProcessHit(enemyProjectile);
		/* dependendo do que colidiu com o player, esse irá ser destruído */
		if(other.gameObject.tag != Constantes.BOSS && !IsGroundEnemy(other)){//destroi inimigos comuns apenas
			if(damageDealer)//inimigo perdera 25% da vida que tem ao colidir diretamente com o player
				damageDealer.HitDueDirectColision(GetHitPercentage());
			else if(enemyProjectile){
				enemyProjectile.InstantiateEffect();
				enemyProjectile.DestroyMe();
			}		
		}	
	}
 	void OnTriggerStay2D(Collider2D other) {
		 
		 DamageDealer damageDealer = other.GetComponent<DamageDealer>();

		 if(!damageDealer)
		 	return;

		 if(other.gameObject.tag == Constantes.BOSS)
		 	ProcessHit(damageDealer);
	}
	private void ProcessHit<T>(T damageHandler) where T: IDamageHandler{
		if(!playerStatus.getFreezePlayerControlStatus()){
			RemovePlayerHealth(damageHandler.GetDamage());
			playerSpritesManager.changeColor(0.25f);
			/*  GameObject explosao_effect = Instantiate(damagedPrefab, transform.GetChild(3).position,Quaternion.identity);
			explosao_effect.transform.parent = gameObject.transform;
			Destroy(explosao_effect, 0.6f);*/ //mt feio, preferi deixar só que mude a cor
			if (playerHealthManager.GetPlayerHealth() <= 0 && !imortal)
				explode();	
		}		
	}
	public void IsPlayerImortal(bool playerImortal = false){
		imortal = playerImortal;
	}
	public bool GetIsPlayerImortal(){
		return imortal;
	}
	private float GetHitPercentage(){
		return gameObject.GetComponent<DamageDealer>().GetDamage();
	}
	private bool IsGroundEnemy(Collider2D other){
		if(other.GetComponent<SpriteRenderer>()){
			if(other.GetComponent<SpriteRenderer>().sortingLayerName == Constantes.BACK_TERRAIN)
				return true;
			else 
				return false;	
		}
		return false;	
	}
	/*reduzira a vida do player a uma portengaem */
	public void ReducePlayerHealth(float reductionPercentage){
		if(!playerStatus.getFreezePlayerControlStatus() && !imortal){
			Debug.LogWarning("reduziu minha life por " + playerHealthManager.GetInicialAmountLife() * reductionPercentage);
			playerHealthManager.RemoveHealth(playerHealthManager.GetInicialAmountLife() * reductionPercentage);
		}
	}
	public void RemovePlayerHealth(float damage){
		if(!imortal)
			playerHealthManager.RemoveHealth(damage);
	}
	/* assim garante que mesmo que o player aperte o mouse
	que nem um louco, o tempo entre os tiros será respeitado */
	private void fire(){
		try{
			if(Input.GetButton("Fire1"))
				playerFireManager.Fire();
						if(Input.GetKeyDown(KeyCode.G))
							GL.Clear(true,true,Color.black);

			if(Input.GetKeyDown(KeyCode.Space)){
					if(PlayerGlobalStatus.changeWeapon()){
						playerFireManager.ManageAmmoVector();
						uiManagement.UpdateWeaponIcon();
					}
			 }	
			 if(Input.GetKeyDown(KeyCode.E)){
				//playerItensManager.TrowBombIfExists();
				 playerItensManager.ChangeItem();
			 }	
			 if(Input.GetKeyDown(KeyCode.R)){
				playerItensManager.UseItem();
			 }		 			
		}catch(System.Exception e){
				Debug.Log(e);
		}
	}
	public void explode(){
		try{
			GameObject explosionPrefab = playerSpritesManager.GetExplosionPrefab();
			GameObject explosao_effect = Instantiate(explosionPrefab, transform.position,Quaternion.identity);
			Destroy(gameObject);
			updatePlayerStatus();

			if(!playerStatus.isGameOver()){
				//Debug.LogWarning("CHANCWS " + PlayerGlobalStatus.getPlayerChances());
				level.waitBeforeTryAgain(tryAgainDelay);
			}else{
				playerStatus.gameOverTrigger();
			}
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}	
	}	
	public void VolumeControl(){
		if(Input.GetMouseButtonDown(1))
			audioManager.CheckVolume();
	}
	private void updatePlayerStatus(){
		audioManager.play_die_audio();
		playerHealthManager.checkPlayerHealth();
		checkChancesAndRemoveIt();
		PlayerGlobalStatus.SetPlayerWeaponIndex(0);
		uiManagement.UpdateChancesText();
		uiManagement.UpdateScoreText();
	}
	private void checkChancesAndRemoveIt(){
		/* como as partes upgrade também sao da layer "Player", sem esse controle,
		 tem a chance de ser removida mais de uma chance por tentativa, o que nao é legal*/
		if(!alreadyMissedChanceOnScene)
			PlayerGlobalStatus.removeChance();
		alreadyMissedChanceOnScene = true;	
	}
	public Transform findChildByTag(string childTag){
	//	Debug.LogWarning("tag a ser procurada: " +  childTag);
		foreach(Transform child in transform){
			if(child.tag == childTag)
				return child;
		}
	//	Debug.LogWarning("Nenhum child com a tag " + childTag + " encontrado.");	
		return null;
	}
	public int countChildsOfChildWithTag(string childTag){
		int counter = 0;
		foreach(Transform child in transform){
			if(child.tag == childTag){
				foreach(Transform childOfChild in child)
					counter ++;
			}		
		}	
		return counter;
	}
	private void setReferences(){
		level = FindObjectOfType<Level>();
		uiManagement = FindObjectOfType<UiManagement>();
		playerStatus = FindObjectOfType<PlayerStatus>();
		audioManager = AudioManager.getInstance();
		playerItensManager = gameObject.GetComponent<PlayerItensManager>();
		playerFireManager = gameObject.GetComponent<PlayerFireManager>();
		playerHealthManager = gameObject.GetComponent<PlayerHealthManager>();
		playerSpritesManager = gameObject.GetComponent<PlayerSpritesManager>();
		playerMoveManager = gameObject.GetComponent<PlayerMoveManager>();

		//audioManager = FindObjectOfType<AudioManager>();	
	}
	private void SetPlayerHealth(){
		if(playerHealthManager){
			playerHealthManager.SetPlayerHealth(playerHealth);
			SetLifeBarUI();
		}	
		else
			Debug.LogWarning("Atencao, verifique se o metodo <setReferences> foi chamado!");		
	}
	/* é importante pq a vida é gerenciada no plaeyerstatus,
	isso garante o "sincronismo" dos elementos de ui, no que diz respeito especial a vida... */
	private void SetLifeBarUI(){
		playerHealthManager.SetLifeBarUi();
	}
}