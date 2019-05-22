using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DamageDealer))]
[RequireComponent(typeof(DestroyOnBecomeInvisible))]
[RequireComponent(typeof(FireManager))]
[RequireComponent(typeof(Shadow))]

public class Enemy : MonoBehaviour {
	//poderia fazer um get e set nelas para não ser "publica", mas deixa quieto por hora.

	private bool canFire;
	private  GameObject danoPrefab;
	private AudioManager audioManager;
	private Color spriteDefaultColor = Color.white;
	private Color spriteDamagedColor = Color.red;
	private FireManager fireManager;
	private Animator animatorController;
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] GameObject DamagePrefab;
	[SerializeField] float health = 100f;
	[SerializeField] private bool fireAfterAnimation = false;
	[SerializeField] bool destroyAfterExplode = true;/*removera o inimigo dps de explodir..é o default,mas n sera o caso pra inimigos de solo */
	private GameObject cashPrefab;
	private float initialHealth;
	/* garante q nao chame mais de uma vez o explode ( condicao de corrida) ( event seria util hehe) */
	private bool explodeTriggerFired = false;
	
	//métodos internos
	void Start () {
		setReferencesAndInitializeValues();
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
		LoadCashPrefab();
		CheckAnimationFlag();
		ManageAnimator();
		
	}
	void FixedUpdate () {
		if(!explodeTriggerFired)/*pois inimgios de solo n sao destruidos dps de explodir, ai n devem ficar atirando */
			ManageFire();
	}
	void OnBecameVisible(){
		StartCoroutine(delayBeforeAnimation());
	}
	
	private void setReferencesAndInitializeValues(){
		try{
			audioManager = AudioManager.getInstance();
			fireManager = gameObject.GetComponent<FireManager>();
			initialHealth = health;
		}catch(System.Exception e){
			Debug.LogWarning("IREI RETORNAR, POIS UMA EXCEÇÃO FOI ENCONTRADA: " + e);
			return;
		}	
	}
	/*podia sempre por no inspector..mas como é o msm pra todos os inimigos vou carregar automatic */
	private void LoadCashPrefab(){
		cashPrefab = Resources.Load<GameObject>(Constantes.CASH_PREFAB_CASH) as GameObject;
	}
	public float GetInitialHealth(){
		return initialHealth;
	}
	private void CheckAnimationFlag(){
		if(fireAfterAnimation)
			canFire = false;	
		else
			canFire = true;	
	}
	private void ManageAnimator(){
		if(gameObject.GetComponent<Animator>()){
			animatorController = GetComponent<Animator>();
			animatorController.enabled = false;
		}	
	}
	private void ManageFire(){
		if(canFire){//canFire nada tem a ver com delays pra atirar, mas sim em o inimigo poder ou nao atirar ( esperar uma animacao completar antes por exemplo)
			fireManager.Fire();
		}	
	}
	void OnTriggerEnter2D(Collider2D other){
		if(!explodeTriggerFired){ /*inimigos do solo por ex n sao deletados dps de explodir, por isso essa checagem */
			//o que colidiu pode ser o player ou municao ( damageDelar) ou municao (AmmoHandler)
			DamageDealer damageDealer = other.GetComponent<DamageDealer>();
			AmmoHandler projectile  = other.GetComponent<AmmoHandler>();
			/* só importa o player e o projetil dele, qqr outra coisa que colidir com um inimigo será indiferente a ele */
			if(!damageDealer && !projectile)
				return;
			/*destroi municoes do player quando tocar no inimigo*/	
			if( other.gameObject.tag == Constantes.PLAYERPROJETIL){
				if(projectile){
					ProcessHit(projectile);
					projectile.GetComponent<AmmoHandler>().InstantiateEffect();
					projectile.DestroyMe();
				}		 
				else
					Debug.LogWarning(Constantes.MISSING_AMMO_HANDLER_SCRIPT);		 
			}
			else if( other.gameObject.tag == Constantes.PLAYER){
				if(damageDealer)/*reduzira a vida do player em 55% se colidiur com um inimigo */
					damageDealer.HitDueDirectColision(GetHitPercentage());
			}
		}
	}
	public void ProcessHit<T>(T damageHandler) where T: IDamageHandler{
		try{
			/*essa funcao também muda a cor do inimigo */
		//	Debug.LogWarning("sangrei " +damageHandler.GetDamage() );
			RemoveEnemyHealth(damageHandler.GetDamage());
			audioManager.play_die_audio();
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}		
	}
	private float GetHitPercentage(){
		return gameObject.GetComponent<DamageDealer>().GetDamage();
	}
	public void ReduceEnemyHealth(float reductionPercentage){
		RemoveEnemyHealth( GetInitialHealth() * reductionPercentage);	
	}
	public void RemoveEnemyHealth(float damage){
		if(!explodeTriggerFired){
			health -= Mathf.Round(damage);
			StartCoroutine(changeColor(0.1f));
			CheckHealth();
		}
	}
	private void CheckHealth(){
		if (health <= 0f){
			health = 0f;
			explode();
		}	
	}
	private void explode(){
		if(!explodeTriggerFired){
			explodeTriggerFired = true;
			SetCash();
			Instantiate(explosionPrefab, transform.position,Quaternion.identity);
			if(destroyAfterExplode)
				Destroy(gameObject);
			else
				ManagePosExplode();
			audioManager.play_die_audio();
		}	
	}
	private void ManagePosExplode(){
		if(animatorController){
			animatorController.SetTrigger(Constantes.ENEMY_DESTROYED);
		}
		gameObject.GetComponent<DamageDealer>().SetHurt(false);

		if(!gameObject.GetComponent<DestroyOnBecomeInvisible>()){
			gameObject.AddComponent<DestroyOnBecomeInvisible>();
		}
	}
	public void SetCash(){
		if(cashPrefab){
			Debug.LogWarning("setouuu ");
			GameObject cash = Instantiate(cashPrefab, transform.position,Quaternion.identity);
			Cash cashRef = cash.GetComponentInChildren<Cash>();
			cashRef.SetCash(GetComponent<DamageDealer>().GetCash());
		}
		else
			Debug.LogWarning(Constantes.MISSING_REF);		
	}
	public Vector3 GetEulerAngles(){
		/* ao instaniar num terreno, garantir que ele
		ira instanciar com a rotacao desejada. Já que a quartenium.identity
		"caga" isso */
		return transform.localRotation.eulerAngles;
	}
	//corrotinas:
	IEnumerator changeColor(float delay){	
		gameObject.GetComponent<SpriteRenderer>().color = spriteDamagedColor;
		yield return new WaitForSeconds(delay);
		gameObject.GetComponent<SpriteRenderer>().color = spriteDefaultColor;
	}	
	
	IEnumerator delayBeforeAnimation(){
		yield return new WaitForSeconds(0f);
		if(animatorController)
			animatorController.enabled = true;
	}
	//funcoes chamadas na animacao:
	public void CanFire(){
		canFire = true;
	}
}
