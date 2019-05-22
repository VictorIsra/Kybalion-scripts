using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Ammo")]
[System.Serializable]
public class Ammo:ScriptableObject  {
	[SerializeField] private GameObject ammoPrefab;
	[SerializeField] private float shotSpeed;
	[SerializeField] private float firePower;
	[SerializeField] private float fireDelay;
	[SerializeField] private bool playerDiretionalFire = false;//se a munição surgirá de forma radial
	[SerializeField] private bool playerTrackerFire = false;
	[SerializeField] private bool apendPositionToPlayer = false;

	private bool useAmmo = false;

	public string GetAmmoChildTag(){
		/* essa tag será usada para
		saber quais pontos num upgradePart poderao atirar essa municao */
		
		/* nunca ocorrerá esse caso a nao ser que eu tenha me esquecido de criar
		e marcar um filho! */
		if(ammoPrefab.transform.childCount == 0){
			return null;
		}	
		else
			return ammoPrefab.transform.GetChild(0).tag;
		/* cada municao tem apenas um únic filho */
	}
	public void SetAmmoStatus(bool ammoStatus){
		useAmmo = ammoStatus;
	}
	public bool GetAmmoStatus(){
		return useAmmo;
	}
	public float GetFireDelay(){
		return fireDelay;
	}
	public string GetAmmoType(){
		if(playerDiretionalFire)
			return Constantes.DIAGONAL;
		else if(playerTrackerFire)
			return Constantes.TRACKER;
		else
			return Constantes.LINEAR;		
	}
	public Sprite GetAmmoSprite(){
		return ammoPrefab.gameObject.GetComponent<SpriteRenderer>().sprite;
	}
	public GameObject getAmmoPrefab(){
		return ammoPrefab;
	}
	public float getAmmoShotSpeed(){
		return shotSpeed;
	}
	public float GetAmmoFirePower(){
		return firePower;
	}
	//seta o fire power default e atualiza quando mudam
	/* WEAPON.CS, FIREMANAGER.CS E ROBOTFIREMAANAGER.CS CHAMAM ESSE MÉTODO */
	public void SetFirePower(){
		try{
			if(ammoPrefab.GetComponent<AmmoHandler>())
				ammoPrefab.GetComponent<AmmoHandler>().SetDamage(firePower);
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	
	}
	public bool ApendPositionToPlayer(){
        return apendPositionToPlayer;
    }
	
	/*SÓ SERVEM PARA O PLAYER, OS INIMIGOS LIDAM COM ISSO ATRAVES DO INSPECTOR: */
	//define se a munição será do tipo direcional ( na direcao de um ponto de origem ) ou linear( default ):
	public void setIsDiretionalFire (bool diretionalFire ){
		playerDiretionalFire  = diretionalFire ;
	}
	public bool getIsDiretionallFire(){
		return playerDiretionalFire ;
	}
	public bool getIsTrackerFire(){
		return playerTrackerFire;
	}
	//fazr amamnha
	public void SetAmmoSpawPoints(){

	}
	public List<Transform> GetAmmoSpawPoints(){
		return null;
	}

}
