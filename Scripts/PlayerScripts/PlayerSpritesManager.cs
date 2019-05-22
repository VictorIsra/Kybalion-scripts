using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealthManager))]
[RequireComponent(typeof(PlayerStatus))]

public class PlayerSpritesManager : MonoBehaviour {
	
	private PlayerStatus playerStatus;
	private PlayerHealthManager playerHealthManager;
	private Player player;
	//sprites do player
//	[SerializeField] Sprite[] playerSprites = new Sprite[1];
	//variáveis relacionadas as explosões do player e afins:
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] GameObject damagedPrefab;//quando a nave tiver com pouca vida
	private GameObject smoke;//se tiver nulo é pq nao tem uma instancia
	private Transform smokeChild;//child que é o ponto onde a fumaca deve ser spawnada
	
	//variaveis relacionadas as cores dos sprites:
	private Color spriteDefaultColor = Color.white;
	private Color spriteDamagedColor = Color.red;
	
	void Awake(){
		SetReferences();
		
	}
	void Update () {
		if(!playerStatus.getFreezePlayerControlStatus()){
			SpritesManager();
		}
	}
	private void SetReferences(){
		playerStatus = gameObject.GetComponent<PlayerStatus>();
		player = gameObject.GetComponent<Player>();
		playerHealthManager = gameObject.GetComponent<PlayerHealthManager>();
		smokeChild = findChildByTag(Constantes.PLAYER_SMOKE_SPAW_POINT);

	}
	private void SpritesManager(){
		try{
			if(playerHealthManager.GetPlayerHealth()  <  Constantes.AVERAGE_HEALTH * playerHealthManager.GetInicialAmountLife() 
			&& playerHealthManager.GetPlayerHealth()  >= Constantes.LOW_HEALTH * playerHealthManager.GetInicialAmountLife()){
				if(smoke != null)
					Destroy(smoke);
			}	
			if(playerHealthManager.GetPlayerHealth()  < Constantes.LOW_HEALTH * playerHealthManager.GetInicialAmountLife()){
				if(smoke == null){//se nao existir uma instancia (evitar criar varios clones)
					smoke = Instantiate(damagedPrefab, smokeChild.position,Quaternion.identity);/*  Instantiate(damagedPrefab, new Vector3( transform.GetChild(3).position.x,
						transform.GetChild(3).position.y,explosionZpositon),Quaternion.identity);*/
					smoke.transform.parent = gameObject.transform;
				}
			}	
			if(playerHealthManager.GetPlayerHealth()  >=  Constantes.AVERAGE_HEALTH * playerHealthManager.GetInicialAmountLife()){
				if(smoke != null)
					Destroy(smoke);
			}		
		}
		catch(System.Exception e){
			Debug.LogWarning(e);
		}	
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
	public void changePlayerAndChildsSpritesColor(Color spriteColor){

		gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
		foreach(Transform playerUpgradeParts in gameObject.transform){
			if(playerUpgradeParts.GetComponent<SpriteRenderer>())
				playerUpgradeParts.GetComponent<SpriteRenderer>().color = spriteColor;
		}	
	}
	public void changeColor(float delay){
		StartCoroutine(changeColorCoroutine(delay));
	}
	public GameObject GetExplosionPrefab(){
		return explosionPrefab;
	}
	IEnumerator changeColorCoroutine(float delay){	
		if(!player.GetIsPlayerImortal())
			changePlayerAndChildsSpritesColor(spriteDamagedColor);
		yield return new WaitForSeconds(delay);
		changePlayerAndChildsSpritesColor(spriteDefaultColor);
	}	
}
