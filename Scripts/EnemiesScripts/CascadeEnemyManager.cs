using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectHandler))]
/*há inimigos como dragoes e trens que são composto de
varias subpartes,essa classe lida com eles */
public class CascadeEnemyManager : MonoBehaviour {
	private ObjectHandler objectHandler;
	private int childsNumbers;/*numero de filhos...qd todos tiverem explodido, posso "morrer"..se nem todos foram explodidos, continua andando normal */
	private int childsDestroyed = 0;/*qts filhos ja explodiram */
	private bool fireEvent;/*evento dps q destrui ele e todos os filhos */
	private bool trigger = false;

	void Awake(){
		objectHandler = GetComponent<ObjectHandler>();
	}
	 void Start(){
		 /*precisa ser no start, pois no awake n tem a instancia do prefab, logo os filgos seriam zero..aqui ja da pra contar */
		Initialize();
	}
	// Update is called once per frame
	void Update () {
		if(!trigger)
			CheckChildsStatus();
	}
	private void Initialize(){
		childsNumbers = gameObject.transform.childCount - 1 ;/*lmebre q um é destruido logo dps de criado */
	}
	private void CheckHealth(){
		if(objectHandler.GetHealth() <= 0)
			CheckChildsStatus();
	}
	public void IncrementDestroyedCounter(){
		childsDestroyed++;
		Debug.LogWarning("qts ja " + childsDestroyed + " muem " + childsNumbers);
	}
	private void CheckChildsStatus(){
		if(childsDestroyed == childsNumbers){
			Debug.LogWarning("ENCADEIA LALAL");
			trigger = true;

		}
	}
}
