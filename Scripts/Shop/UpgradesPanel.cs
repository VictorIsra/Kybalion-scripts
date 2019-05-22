using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

/* essa classe controla o que será exibido no UpgradesPanel: o lvl do item em questão,
se a barra do level permanece ou não caso o player tenha ou nao o item etc.. */
public class UpgradesPanel : MonoBehaviour	{
	private GameObject itemPrefabRef;

	private void Start() {
		SetItenRef();
		CheckUpgrades();
	}
	private void SetItenRef(){
		itemPrefabRef = transform.parent.GetComponent<ItemManager>().GetItemPrefab();
	}
	public void CheckUpgrades(){
		
		bool haveItem = transform.parent.GetComponent<ItemManager>().PlayerHaveItem();
		if(haveItem){
			CheckItemLevel();
		}
		else{	
			/*sempre que nao tiver o item quero disabilitar o icone, mas só no caso
			das armas que quero resetar os status apos vender. bombas e shields persistirão
			o lvl de upgrade! */
			if(itemPrefabRef.CompareTag(Constantes.WEAPON)){
				EnableAllChildsImage(false);
				ResetItemStatus();
			}
			else
				CheckItemLevel();
		}
	}
	private void EnableAllChildsImage(bool enableChilds){
		foreach (Transform child in transform){
			if(child.GetComponent<Image>())
				child.GetComponent<Image>().enabled = enableChilds;
		}
	}
	private void CheckItemLevel(){
		int currentUpgradeLevel = 0;
		if(itemPrefabRef.CompareTag(Constantes.WEAPON))
			currentUpgradeLevel = itemPrefabRef.GetComponent<Weapon>().GetWeaponLevel();
		else if(itemPrefabRef.CompareTag(Constantes.BOMB))
			currentUpgradeLevel = itemPrefabRef.GetComponent<Bomb>().GetBombLevel();
		else if(itemPrefabRef.CompareTag(Constantes.SHIELD))
			currentUpgradeLevel = itemPrefabRef.GetComponent<Shield>().GetShieldLevel();

		if(currentUpgradeLevel >= Constantes.ITEN_BASIC_LEVEL){
			SetUpgrade(0);
			if(currentUpgradeLevel >= Constantes.ITEN_INTERMEDIARY_LEVEL){
					SetUpgrade(1);
					if(currentUpgradeLevel  == Constantes.ITEN_MAX_LEVEL)
						SetUpgrade(2);
			}
		}	
	}
	private void ResetItemStatus(){
		//resetara o upgrade e o poder de fogo das armas, ja que o prefab persiste
		transform.parent.GetComponent<ItemManager>().ResetItemPrefabStatus();
	}
	private void SetUpgrade(int childIndex){
		GameObject itemRef = transform.parent.GetComponent<ItemManager>().GetItemPrefab();
		try{
			transform.GetChild(childIndex).GetComponent<Image>().enabled = true;
			if(itemRef.CompareTag(Constantes.WEAPON))
				itemRef.GetComponent<Weapon>().ConfigWeaponAmmos();
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
}
