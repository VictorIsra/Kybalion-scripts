using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsPartsManager : MonoBehaviour {

	private Player player;
    private PlayerFireManager playerFireManager;
	/*MÉTODOS REFERENTES A PARTES "UPGRADIÁVEIS" DO PLAYER: */
	private void Awake() {
		SetPlayerReference();
		manageDefaultWeaponAndUpgradeParts();
	}
	private void SetPlayerReference(){
		player = gameObject.GetComponent<Player>();
        playerFireManager = gameObject.GetComponent<PlayerFireManager>();
	}
    private void manageDefaultWeaponAndUpgradeParts(){
        PlayerGlobalStatus.CountPlayerUpgradePartsSize();
        GameObject playerDefaultLaser = Resources.Load<GameObject>(Constantes.DEFAULT_PLAYER_WEAPON_PATH);
        AddWeaponAndManageAmmoVector(playerDefaultLaser);
        setDefaultWeaponUpgradePart(playerDefaultLaser);
    }
    /* essa funcao chamará o playerFireManager! passo fundamental e uma vez
    mais, um exemplo de onde um delegate e evento seria util... */
    private void AddWeaponAndManageAmmoVector(GameObject playerDefaultLaser){
        PlayerGlobalStatus.AddPlayerItem(playerDefaultLaser);
        playerFireManager.ManageAmmoVector();
    }
    private void setDefaultWeaponUpgradePart(GameObject playerDefaultLaser){
        checkplayerUpgradePart(playerDefaultLaser);//"upgrade" padrao ( nao aparece rs)
        restoreUpgradeParts();//restaura partes após try again
    }   
    public void checkplayerUpgradePart(GameObject weapon){
        try{
            //que local do player a parte continda na arma deverá ser spawnada/encaixada nele 

            string playerSpawPointTag = weapon.GetComponent<Weapon>().playerSpawPointTag();
            Transform spawPoint = player.findChildByTag(playerSpawPointTag);
            GameObject newPart = weapon.GetComponent<Weapon>().getWeaponUpgradePart();
            if(!partWithTagAlreadyExists(newPart)){//SERVE PRA LIDAR COM O CASO DA PARTE DA ARMA DEFAULT, APENAS
                GameObject UpgradePart = Instantiate(newPart,spawPoint.position,Quaternion.identity);
                UpgradePart.transform.parent = player.transform;

                PlayerGlobalStatus.addPlayerUpgradePart(newPart);
            }    
        }catch(System.Exception e){
            Debug.LogWarning(e);
        }
    }
    private void restoreUpgradeParts(){
        List<GameObject> playerWeapons = PlayerGlobalStatus.GetPlayerWeapons();
        foreach(GameObject weapom in playerWeapons)
            checkplayerUpgradePart(weapom);
    }
    private bool partWithTagAlreadyExists(GameObject part){
        /* SERVE PRA LIDAR COM O CASO DA PARTE DA ARMA DEFAULT, APENAS.
        A CHECAGEM DE OUTRAS PARTES É FEITA OLHANDO A LISTA DA MESMA,
        EM <PlayerGlobalStatus.addPlayerUpgradePart()> Mas COMO A ARMA DEFAULT É MAIS DELICADA, OPTEI
        POR TRATA-LA DE FORMA DIFERENCIADA AQUI*/
        if(player.findChildByTag(part.tag) == null )
            return false;  
        else
          return true;  
    }
}
