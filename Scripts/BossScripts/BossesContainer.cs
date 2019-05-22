using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*controlará quantos chefões tem na fase  e definirá quando poderei passar de level
( qd todos os chefoes forem destruidos etc)*/
public class BossesContainer : MonoBehaviour {
	private int bossesNumber;//numero de chefoes na fase
	private PlayerStatus playerStatus;
	private Level level;
	// Use this for initialization
	void Awake(){
		CountBosses();
		SetReference();
	}
	/*conta os chefoes na fase */
	private void CountBosses(){
		foreach(Transform childs in gameObject.transform){
			bossesNumber++;
		}
	}
	private void SetReference(){
		level = FindObjectOfType<Level>();
		playerStatus = FindObjectOfType<PlayerStatus>();
	}
	public void DecreaseBossesNumber(){
		bossesNumber --;
		if(bossesNumber == 0 )
			playerStatus.setFreezePlayerControlStatus(true);
	}
	public void CheckBossesNumber(){
		if(bossesNumber == 0){
			level.loadShopMenu();
		}	
	}
}
