using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BossHealthManager))]
[RequireComponent(typeof(BossSpriteManager))]
[RequireComponent(typeof(BossMoveManager))]
[RequireComponent(typeof(Shadow))]

public class Boss : MonoBehaviour{
	
	private AudioManager audioManager;
	private float healthOriginal;//valor original da vida, para usar como referência.
	private UiManagement uiManagement;
	private PlayerStatus playerStatus;
	private Level level;
	private bool stopMoving = false;
	private float delayTime = 0.5f;
	private BossMoveManager bossMoveManager;
	private BossHealthManager bossHealthManager;
	private BossSpriteManager bossSpriteManager;
	private BossesContainer bossesContainer;
	private List<GameObject> fireParts = new List<GameObject>();
	//

	private float bossWidth;
	private float bossHeight;

	private void Awake() {
		SetReferences();
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}
	void OnBecameVisible(){
		StartCoroutine(StartFire());
	}
	void FixedUpdate () {
		if(!stopMoving){
	 		bossMoveManager.MoveManager();
		}
		else
			bossSpriteManager.SetDieColor();

	}
	void OnTriggerEnter2D(Collider2D other){
		if(!stopMoving){
			if(other.gameObject.tag == Constantes.PLAYERPROJETIL){
				AmmoHandler playerProjectile = other.GetComponent<AmmoHandler>();
				if(!playerProjectile)
					return;
				ManageHit(playerProjectile);
			}
			CheckHealth();	
		}
	}
	private void SetReferences(){
		bossMoveManager = GetComponent<BossMoveManager>();
		bossHealthManager = GetComponent<BossHealthManager>();
		bossSpriteManager = GetComponent<BossSpriteManager>();
		uiManagement = FindObjectOfType<UiManagement>();
		bossesContainer = FindObjectOfType<BossesContainer>();
		audioManager = AudioManager.getInstance();
		SetFireParts();
		
	}
	private void SetFireParts(){
		foreach(Transform child in transform){
			if(child.gameObject.CompareTag(Constantes.RED_EYE))
				fireParts.Add(child.gameObject);
		}
	}
	private void CheckHealth(){
		if( bossHealthManager.GetHealth() <= 0){
			stopMoving = true;
			StopFire();
			AddCash();
			TriggerEndLevelEvents();
			return;
		}	
	}
	private void StopFire(){
		foreach(GameObject firePart in fireParts){
			if(firePart.GetComponent<BossFirePart>())
				firePart.GetComponent<BossFirePart>().SetFire(false);
			else
				Debug.LogWarning(Constantes.MISSING_REF);	
		}
	}
	private void TriggerEndLevelEvents(){
		uiManagement.SetBossLifeBarUIVisibility(false);
		if(bossesContainer)/*posso tar só testando entao ter ele desativa */
			bossesContainer.DecreaseBossesNumber();
		bossSpriteManager.SetDieColor();
		bossSpriteManager.GenerateExplosions(delayTime);
	}
	private void AddCash(){
		PlayerGlobalStatus.addPlayerCash(GetComponent<DamageDealer>().GetCash());
		uiManagement.UpdateScoreText();
	}
	private void ManageHit(AmmoHandler playerProjectile){
		bossHealthManager.RemoveEnemyHealth(playerProjectile.GetDamage());
		bossSpriteManager.InstantiateDamagedEffect(playerProjectile);
		ProjectileHandler(playerProjectile);			
	}
	private void ProjectileHandler(AmmoHandler playerProjectile){
		playerProjectile.GetComponent<AmmoHandler>().InstantiateEffect();
		playerProjectile.DestroyMe();
		audioManager.play_die_audio();
	}
	public void DestroyBoss(){
		bossesContainer.CheckBossesNumber();
		Destroy(gameObject);
		/*alem de destruí-lo,ao final da explosao de animacao será chamado esse método
		que decontará um bosso do pool de bos ( bossesContainer) */
	}
	IEnumerator StartFire(float delay = 1.25f){
		yield return new WaitForSeconds(delay);
		foreach(GameObject firePart in fireParts){
			if(firePart.GetComponent<BossFirePart>())
				firePart.GetComponent<BossFirePart>().SetFire(true);
		}	
	}
}
