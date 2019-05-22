using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
ESSA CLASSE DEFINE E GERENCIA O COMPORTAMENTO DO ITEM SORTEADO/GERADO PELA CLASSE:
						<CollectableManager.cs>
 */
public class CollectableBehaviour : MonoBehaviour {
/* 
	private UiManagement uiManagement;
	private PlayerStatus playerStatus;
	private AudioManager audioManager;
	private CollectableManager collectableManager;
	private string itemType;

	void Start () {
		uiManagement = FindObjectOfType<UiManagement>();
		playerStatus = FindObjectOfType<PlayerStatus>();
		audioManager = AudioManager.getInstance();
		collectableManager = FindObjectOfType<CollectableManager>();
	}
	void OnTriggerEnter2D(Collider2D colisor){
		if(colisor.gameObject.tag == Constantes.PLAYER){
			handleCollectableBehaviour(itemType);
			Destroy(gameObject);
		}	
	}
	public void setItemType(string itemType){
		this.itemType = itemType;
	}
	private void handleCollectableBehaviour(string itemType){
		try{
			if(itemType == Constantes.MUNICAOCOLETABLE){
				GameObject sortedWeapon = collectableManager.sortWeapon();
			bool wasNewWeaponFound = PlayerGlobalStatus.AddPlayerItem(sortedWeapon);
				if(wasNewWeaponFound){
					uiManagement.UpdateWeaponIcon();
					playerStatus.checkplayerUpgradePart(sortedWeapon);
				audioManager.play_itemColect_audio();
				}
			

			}		
			else if(itemType == Constantes.VIDACOLETABLE){
				playerStatus.AddHealth(collectableManager.sortLife());
				audioManager.play_itemColect_audio();
			}	
			else if(itemType == Constantes.PONTUACAOCOLETABLE){
				PlayerGlobalStatus.addPlayerCash(collectableManager.sortCash());
				uiManagement.UpdateScoreText();
				audioManager.play_itemColect_audio();
			}	
		}
		catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
*/
}
