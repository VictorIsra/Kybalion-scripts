using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossHealthManager))]

public class BossSpriteManager : MonoBehaviour {
	[SerializeField] GameObject DamagePrefab;
	[SerializeField] GameObject littleExplosionPrefab;
	[SerializeField] GameObject superEplosionPrefab;
	[SerializeField] Sprite[] bossSprites = new Sprite[3];
	private BossHealthManager bossHealthManager;
	private Color spriteDefaultColor = Color.white;
	private Color spriteDamagedColor = Color.red;
	/*pra explosoes ficarem acima */
	private string bossSortingLayer;
	private int bossSortingOrder;

	private void Awake() {
		SetReferences();
		SertSortingLayerInfo();
	}
	private void SetReferences(){
		bossHealthManager = GetComponent<BossHealthManager>();
	}
	private void SertSortingLayerInfo(){
		if(gameObject.GetComponent<SpriteRenderer>()){
			SpriteRenderer spriteRendererRef = gameObject.GetComponent<SpriteRenderer>();
			bossSortingLayer = spriteRendererRef.sortingLayerName;
			bossSortingOrder = spriteRendererRef.sortingOrder;
		}
	}
	public void ChangeColor(float delay){
		StartCoroutine(changeColorCoroutine(delay));
	}
	IEnumerator changeColorCoroutine(float delay){	
		gameObject.GetComponent<SpriteRenderer>().color = spriteDamagedColor;
		yield return new WaitForSeconds(delay);
		gameObject.GetComponent<SpriteRenderer>().color = spriteDefaultColor;
	}	
	public void CheckSprite(){
		if(bossHealthManager.GetHealth() <  Constantes.AVERAGE_HEALTH * bossHealthManager.GetOriginalHealth())
			gameObject.GetComponent<SpriteRenderer>().sprite = bossSprites[1];
		if(bossHealthManager.GetHealth() <  Constantes.LOW_HEALTH * bossHealthManager.GetOriginalHealth())
			gameObject.GetComponent<SpriteRenderer>().sprite = bossSprites[2];
	}
	public void SetDieColor(){
		gameObject.GetComponent<SpriteRenderer>().color = spriteDamagedColor;
	}
	public void InstantiateDamagedEffect(AmmoHandler playerProjectile){
		GameObject explosao_effect = Instantiate(DamagePrefab, playerProjectile.transform.position,Quaternion.identity);
		explosao_effect.transform.SetParent(gameObject.transform);
	}
	public void  GenerateExplosions(float delay){
		StartCoroutine(GenerateExplosionsCoroutine(delay));
	}
	IEnumerator GenerateExplosionsCoroutine(float delay){
		/*brincando com corotina pra ter o efeito irado xD */
		int childNumbers = FindChildsWithTag(Constantes.BOSS_EXPLOSION_SPAWNPOINT);
		foreach(Transform child in transform)
			yield return StartCoroutine(MultipleExplosions(0.5f, child));
		foreach(Transform child in transform){
			GameObject littleExplosion = Instantiate(littleExplosionPrefab,child.transform.position,Quaternion.identity);
			SetExplosionsLayer(littleExplosion);
		}
		yield return new WaitForSeconds(1f);
		GameObject Bigexplosao_effect = Instantiate(superEplosionPrefab,transform.GetChild(childNumbers - 1).position,Quaternion.identity);
		Bigexplosao_effect.transform.SetParent(gameObject.transform);
		SetExplosionsLayer(Bigexplosao_effect);
	}
	IEnumerator MultipleExplosions(float delay, Transform child){
		GameObject littleExplosion = Instantiate(littleExplosionPrefab,child.transform.position,Quaternion.identity);
		SetExplosionsLayer(littleExplosion);
		yield return new WaitForSeconds(delay);
	}
	private int FindChildsWithTag(string tag){
		int childsFound = 0;
		foreach(Transform child in gameObject.transform){
			if(child.CompareTag(tag))
				childsFound++;
		}
		return childsFound;
	}
	private void SetExplosionsLayer(GameObject explosionInstance){
		/*explosao será sempre a frente do parent */
		explosionInstance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = bossSortingLayer;
		explosionInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder = bossSortingOrder + 2;/*pq tem a parte que atira tb */
	}
}
