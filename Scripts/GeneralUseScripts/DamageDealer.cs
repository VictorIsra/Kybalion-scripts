using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour, IDamageHandler {
	/* qual fracao da vida do que colidir comigo irá perder */
	[SerializeField] private float myDamagePercentageOnHit = 10;
	[SerializeField] private float cash = 100;
	private bool canHurt = true;/*enqt isso for true, quem tiver o script ainda pode causar dano
	. qd eu destruir um inimigo de solo, como ele n é deletado da cena, eu boto isso como false nessa hora */
	/*aqui é uma porcentagem e nao o dano, mas manterei esse nome por causa da
	exigencia da interface iDamageHandler */
	public float GetDamage(){
		return myDamagePercentageOnHit;
	}
	public void SetDamage(float damage){
		myDamagePercentageOnHit = damage;
	}
	public float GetCash(){
		return cash;
	}
	public void SetCash(float cashValue){
		cash = cashValue;
	}
	public bool CanHurt(){
		return canHurt;
	}
	public void SetHurt(bool canHurt){
		this.canHurt = canHurt;
	}
	/*dano que o par inimigo-player receberão ao se colidirem diretamente */
	public void HitDueDirectColision(float damagePercentage){
		if(gameObject.CompareTag(Constantes.PLAYER))
			gameObject.GetComponent<Player>().ReducePlayerHealth(damagePercentage);
		else if(gameObject.CompareTag(Constantes.ENEMY))
			gameObject.GetComponent<Enemy>().ReduceEnemyHealth(damagePercentage);
		else if(gameObject.CompareTag(Constantes.BOSS)){

		}		
	}
	/*dano que o que colidiu sofrera ao estar no range de uma bomba/explosao */
	public void HitDueBombExplosion(float firePower){
		Debug.LogWarning("FIRE POWER EM DAMAGEDALR " + firePower);
		if(gameObject.layer == Constantes.ENEMY_LAYER)
			gameObject.GetComponent<Enemy>().RemoveEnemyHealth(firePower);
		else if(gameObject.CompareTag(Constantes.BOSS)){
			gameObject.GetComponent<BossHealthManager>().RemoveEnemyHealth(firePower);

		}
		else if(gameObject.CompareTag(Constantes.PLAYER)){
			Debug.LogWarning("eu player sangrei  " + firePower);
			gameObject.GetComponent<PlayerHealthManager>().RemoveHealth(firePower);
		}			
			
	}
	public void DestroyMe(){
		Destroy(gameObject);
	}
}
