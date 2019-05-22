using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandler : MonoBehaviour, IDamageHandler {

	[HideInInspector]
	public float myDamage;
	private float destroyDelay = 5f;
	[SerializeField] GameObject hitEffect;/*efeito que aparece qd a municao colide com algo */
	/*legal pq simula um efeito bacana caso use o vetor nulo... */
	private Vector2 ammoVelocity = new Vector2(0,0);
	private float bulletSpeed = 0f; /*é a velo ( lembre que essa é float n vect ) definida no scriptable ammo */
	/*legal brincar com isso ein xD ( chame esse metodo no firemanager hehe) */
	public void UpdateAmmoVelocity(Vector2 ammoVelocity, float delayBeforeUpdate = 0.25f){
		this.ammoVelocity = ammoVelocity;
		StartCoroutine(WaitBeforeUpdateVelocity(delayBeforeUpdate));
	}
	public float GetAmmoSpeed(){
		return bulletSpeed;
	}
	public void SetAmmoSpeed(float bulletSpeed){
		this.bulletSpeed = bulletSpeed;
	}
	private void Awake() {
		StartCoroutine(DestroyAfterDelay());
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}
	IEnumerator WaitBeforeUpdateVelocity(float delay){
		yield return new WaitForSeconds(delay);
		gameObject.GetComponent<Rigidbody2D>().velocity = ammoVelocity;
    }
	public float GetDamage(){
//		Debug.LogWarning("Meu dano " + myDamage);
		return myDamage;
	}		
	public void SetDamage(float damage){
		myDamage = damage;
	}
	public void DestroyMe(){
		Destroy(gameObject);
	}
	public void InstantiateEffect(){
		if(hitEffect)
			Instantiate(hitEffect,transform.position,Quaternion.identity);
	}
	/*msm com o scrip de destroy on invisible..o tiro pode ser instanciado
	imediatamente antes do inimigo ficar invisivel, de modo que o método nunca
	vai ser chamado. essa corotina garante que qqr municao sera sempre destruida */
	IEnumerator DestroyAfterDelay(){
		yield return new WaitForSeconds(destroyDelay);
		Destroy(gameObject);
	}
	private void MovePattern(){
		/*movimento da bala: senoidal, logaritmo..enfim, a curva que ela faz */
	}
	void FixedUpdate()
	{
		MovePattern();
	}
}
