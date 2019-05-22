using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FireManager))]
public class BossFirePart : MonoBehaviour {
	/*vao ter objetos atrelhados a nave mae que atiram
	, e usaram o firemanager para tal.posso atrelhar ao propria nave mae,
	mas acho mais legal atrelhar a partes separadas,pra elas serem autonomas
	e mais legais de controlar o comportamento
	 */
	private bool canFire = false;
	private FireManager fireManager;

	void Awake(){
		SetReferences();
	}
	void FixedUpdate(){
		if(canFire)
			fireManager.Fire();
	}
	public void SetFire(bool canFire){
		this.canFire = canFire;
	}

	private void SetReferences(){
		fireManager = GetComponent<FireManager>();
	}
}
