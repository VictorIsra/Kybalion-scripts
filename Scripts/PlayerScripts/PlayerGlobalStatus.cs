using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* CLASSE ESTÁTICA QUE CONTROLARÁ ITENS GLOBAIS, COMO
AS CHANCES DO PLAYER, SUA PONTUAÇÃO E SEUS ITENS
 */
public static class PlayerGlobalStatus {
    
    private static List<GameObject> playerWeapons = new List<GameObject>();
    private static List<List<GameObject>> playerItens = new List<List<GameObject>>(2);
    private static List<GameObject> playerUpgradeParts = new List<GameObject>();
	private static float playerCash;
    private static float playerCashAux = 0;//global que auxiliara playercash
    private static int playerChances = Constantes.NUMERO_CHANCES_DEFAULT;
    private static int playerWeaponIndex = 0;
    /*guard referencia */
    private static GameObject bombRef;
    private static GameObject shieldRef;
    /*essa lista guarda uma referencia para bomba e um shield. Assim, posso
    resetar seu status independente da lista playerItens, de modo que
    os valores serão resetados de forma correta sempre, independente do player
    ter ou nao algo na lista!! fundamental isso! */
    private static List<GameObject> refControlList = new List<GameObject>();
    /* CONSTRUCTOR */
    static PlayerGlobalStatus(){
        InitializeItensList();
        LoadControlList();
        ResetAuxCash();
        LoadItemsRef();
    }
    private static void LoadItemsRef(){
        bombRef = Resources.Load(Constantes.BOMB_PATH) as GameObject; 
        shieldRef = Resources.Load(Constantes.SHIELD_PATH) as GameObject; 
        /*  for(int i = 0; i < 10; i++){
            playerItens[0].Add(bombRef);
            playerItens[1].Add(shieldRef);
        }*/

    }
    private static void LoadControlList(){
        try{
            GameObject bombRef = Resources.Load(Constantes.BOMB_PATH) as GameObject; 
            GameObject shieldRef = Resources.Load(Constantes.SHIELD_PATH) as GameObject; 
            
            refControlList.Add(bombRef);
            refControlList.Add(shieldRef);
            
            ResetBombAndShieldStatus();

        }catch(System.Exception e){
            Debug.LogWarning(e);
        }
    }
    private static void ResetBombAndShieldStatus(){
        /*posicao 0 guarda a ref de uma bomba, posicao 1 guarda ref de um shield */
        refControlList[0].GetComponent<Bomb>().ResetBombStatus();
        refControlList[1].GetComponent<Shield>().ResetShieldStatus();
      //  Debug.LogWarning("BOMBA E SHIELD RESETADOS");
    }
    /*caso eu colha um shield,uma bomba ou uma arma numa fase */
    public static bool AddCollectedItem(string itemTag){
        if(itemTag == Constantes.BOMB){
            if(playerItens[0].Count < Constantes.MAX_ITEM_CAPACITY){
                playerItens[0].Add(bombRef);
                return true;
            } 
            return false;   
        }
        else if(itemTag == Constantes.SHIELD){
            if(playerItens[1].Count < Constantes.MAX_ITEM_CAPACITY){
                playerItens[1].Add(shieldRef);
                return true;
            }   
            return false; 
        }    
        else{
            Debug.LogWarning(Constantes.NO_TAG_MATCHED);   
            return false;
        }     
    }
    /*nesse caso tentara adicionar uma arma */
    public static bool AddCollectedItem(GameObject weapon){

        return TryAddWeapon(weapon);
    }
    /*MÉTODOS COMUNS A ARMAS, BOMBAS E CHIELDS */
    private static bool CheckItemType(GameObject item){
        if(item.CompareTag(Constantes.WEAPON)){
           return TryAddWeapon(item); 
        }
        else if(item.CompareTag(Constantes.BOMB)){
            /*lembre: primeira lista da lista de listas playerItens se refere as bombas
            e a segunda lista aos escudos. */
            //só pode ter até 10 bombas por vez
        if(playerItens[0].Count < Constantes.MAX_ITEM_CAPACITY){
            playerItens[0].Add(item);
            
           // item.GetComponent<Bomb>().SetBombAtributes();
        }    
        return true;
        }
         else if(item.CompareTag(Constantes.SHIELD)){
            /*lembre: primeira lista da lista de listas playerItens se refere as bombas
            e a segunda lista aos escudos. */
           //só pode ter até 10 bombas por vez
            if(playerItens[1].Count < Constantes.MAX_ITEM_CAPACITY){
                playerItens[1].Add(item);
                
               // item.GetComponent<Shield>().SetShieldAtributes();
            }  
            return true;
        }
        else{
            Debug.LogWarning(Constantes.NO_TAG_MATCHED);
            return false;
        }
    }
    public static void RemoveItem(GameObject item){
        if(item.CompareTag(Constantes.WEAPON)){
            playerWeapons.Remove(item);
         /*    Debug.LogWarning("ARMA " + item.name + " REMOVIDA COM SUCESSO"
            + " vetorsize: " + playerWeapons.Count);*/
        }
        else if(item.CompareTag(Constantes.BOMB)){
            /*lembre: primeira lista da lista de listas playerItens se refere as bombas
            e a segunda lista aos escudos. */
            playerItens[0].Remove(item);
        }
         else if(item.CompareTag(Constantes.SHIELD)){
            /*lembre: primeira lista da lista de listas playerItens se refere as bombas
            e a segunda lista aos escudos. */
            playerItens[1].Remove(item);
        }
        else{
            Debug.LogWarning(Constantes.NO_TAG_MATCHED);
        }
    }
    public static bool PlayerHaveItem(GameObject item){
        //playeritens[0] é a list de bombas, playeritens[1] é a lista de shields
        if(item.CompareTag(Constantes.WEAPON))
            return playerWeapons.Contains(item);
        else if(item.CompareTag(Constantes.BOMB))
            return playerItens[0].Contains(item);
        else if(item.CompareTag(Constantes.SHIELD))
            return playerItens[1].Contains(item);   
        else{
            Debug.LogWarning(Constantes.NO_TAG_MATCHED);
            return false;
        }
    }
    /*MÉTODOS REFERENTES AOS ITENS DO PLAYER ( BOMBAS E SHIDLEDS) */
    public static int GetPlayerBombsNumber(){
        return playerItens[0].Count;
    }
     public static int GetPlayerShieldsNumber(){
        return playerItens[1].Count;
    }
     public static bool IsThereBomb(){
        return playerItens[0].Count > 0;
    }
     public static bool IsThereShield(){
        return playerItens[1].Count > 0;
    }
    public static GameObject GetBomb(){
        /*como as bombas sao iguais, basta pegar a primeira */
        return playerItens[0][0];
    }
    public static GameObject GetShieldAura(){
        /*como as bombas sao iguais, basta pegar a primeira */
        return playerItens[1][0].GetComponent<Shield>().GetShieldAura();
    }
    public static float GetShieldAuraCurrentDuration(){
        return refControlList[1].GetComponent<Shield>().shieldSettings.shieldCurrentDuration;
    }
    public static void RemoveBomb(){
        /*remove o primeiro elemento da lsita de bombas */
        playerItens[0].RemoveAt(0);
    }
    public static void RemoveShield(){
        /*remove o primeiro elemento da lsita de shields*/
        playerItens[1].RemoveAt(0);
    }
    public static void ResetPlayerItensStatus(){
               
        foreach(List<GameObject> itemList in playerItens){
            itemList.Clear();
        }   
     //   Debug.LogWarning("LISTA DE BOMBAS E SHIELDS LIMPADAS");
        /*qd esse método é chamado, o vetor de armas do player foi limpado
        portando o indice é o zero, que é a arma default. seto ela para o lvl 1
        ja que o player sempre a possuirá */
        if(playerWeapons.Count >= 1)
            playerWeapons[playerWeaponIndex].GetComponent<Weapon>().SetWeaponLevel(1); 
    }
    private static void InitializeItensList(){
        /*lembra: a primeira lista é de bombas e a segunda de shields */
        for(int i = 0; i < 2; i++)
           playerItens.Add(new List<GameObject>());
    }

  /*MÉTODOS REFERENTES DIRETAMENTE AS ARMAS DO PLAYER: */
    public static bool AddPlayerItem(GameObject item){
		return CheckItemType(item);
    }
    public static bool TryAddWeapon(GameObject weapon){
	    if (!playerWeapons.Contains(weapon)){
		    playerWeapons.Add(weapon);
			changeToNewWeapon(countPlayerWeapons() - 1);
            weapon.GetComponent<Weapon>().SetWeaponLevel(1);
            weapon.GetComponent<Weapon>().SetAndConfigWeapon();
			Debug.Log("ARMA NOVA PSEERO: " + weapon.name + " tamanho vetor " + playerWeapons.Count);
            return true;
		}	
        else{
			Debug.Log("ARMA REPETIDA BROTHER");
            return false;
		}
    }
    public static void CleanPlayerWeapons(){
      int i = 1; //remove todas menos  posicao 0 que é a arma default
      
      if(playerWeapons.Count > 1)
            playerWeapons.RemoveRange(i, playerWeapons.Count - 1);

      playerWeaponIndex = 0;        
    }
    public static bool  changeWeapon(){
        //nao ira trocar se só tiver uma arma
        if(countPlayerWeapons() > 1){
		    try{
			if(playerWeaponIndex + 1 == countPlayerWeapons())
			        playerWeaponIndex = 0;
			    else
		 		    playerWeaponIndex++;      
		    }catch(System.Exception e){
			    Debug.LogWarning(e);
		    }
            //troco de arma
            return true;
        }
        //n troco pois só tinha uma
        else
            return false;
	}
    public static void changeToNewWeapon(int index){
		playerWeaponIndex = index;
	}
    public static void SetPlayerWeaponIndex(int index){
        playerWeaponIndex = index;
    }
    public static void setPlayerWeaponInitialIndex(){
        playerWeaponIndex = 0;
    }
    public static void ResetPlayerWeaponsStatus(){
       // Debug.LogWarning("RESETARI ARMAS DO PLAER GLOBAL STUTUS.CS");
        foreach(GameObject weapon in playerWeapons)
            weapon.GetComponent<Weapon>().ResetWeaponStatus();

        /*qd esse método é chamado, o vetor de armas do player foi limpado
        portando o indice é o zero, que é a arma default. seto ela para o lvl 1
        ja que o player sempre a possuirá */
        if(playerWeapons.Count > 1)
            playerWeapons[playerWeaponIndex].GetComponent<Weapon>().SetWeaponLevel(1); 
    }
    public static int GetPlayerWeaponIndex(){
        return playerWeaponIndex;
    }
    public static int countPlayerWeapons(){
        return playerWeapons.Count;
    }
    public static GameObject getPlayerCurrentWeapon(){
        return playerWeapons[playerWeaponIndex];
    }
    public static GameObject getPlayerWeaponByIndex(int weaponIndex){
        return playerWeapons[weaponIndex];
    }
    public static List<GameObject> GetPlayerWeapons(){
        return playerWeapons;
    } 
    public static List<Ammo> getWeaponAmmos(){
       
        return getPlayerCurrentWeapon().GetComponent<Weapon>().GetWeaponAmmos();
    }
    public static bool isAmmoDiretionalFire(Ammo ammo){
        return ammo.getIsDiretionallFire();
    }
    public static bool isAmmoTrackerFire(Ammo ammo){
        return ammo.getIsTrackerFire();
    }

    /*MÉTODOS REFERENTES AS PARTES "UPGRADIÁVEIS" DO PLAYER: */
    public static void cleanPlayerUpgradeParts(){
        playerUpgradeParts.Clear();
    }
    public static void addPlayerUpgradePart(GameObject upgradePart){
      // Debug.LogWarning("VOU ADICIONAR O SEGUINTE: " + upgradePart);
        if(!playerUpgradeParts.Contains(upgradePart)){
            playerUpgradeParts.Add(upgradePart);
        }    
       /*  else    
         Debug.LogWarning("PARTE REPETIDA BROTHERZAO LESKAO"); */ 
    }
    public static void CountPlayerUpgradePartsSize(){
         Debug.LogWarning( "TAMANHO DO DA LISTA DE PARTES: " + playerUpgradeParts.Count);
    }
    public static string getCurrentWeaponUpgradeTag(){
        return getPlayerCurrentWeapon().GetComponent<Weapon>().getWeaponUpgradePartTag();
    }  
    public static  List<GameObject> getPlayerUpgradeParts(){
        return playerUpgradeParts;
    }
    /*MÉTODOS REFERENTES AO CASH DO PLAYER: */
    public static void addPlayerCash(float cashAmount){
        playerCash += cashAmount;
        playerCash = (float) System.Math.Round(playerCash,2);
    }
    /*pra ficar congruente, o que eu gastei na loja tem que ser levado
    em conta na variavel auxiliar! só o flynextmission através
    do método level.loadnextgamescene que chamará esse metodo ;) */
    public static void UpdateSpendedCash(){
        playerCashAux = playerCash;
    }
    public static void decreaseCash(float cashAmount){
        playerCash -= cashAmount;
        playerCash = (float) System.Math.Round(playerCash,2);
    }  
    public static void SavePlayerCashAux(){
        /*salva a grana, pois ela é resetada qd morre num llv,
        mas precisa persistir o acumulado entre os lvls! */
        playerCashAux = (float) System.Math.Round(playerCash,2);
    }  
    public static float getPlayerCash(){
        return playerCash;
    }
   
    public static void ResetLevelCash(){
        playerCash = playerCashAux;
               Debug.LogWarning("GITAAtemp casH " + playerCashAux + " cashnormal " + playerCash);

    }
    /*MÉTODOS REFERENTES AS CHANCES DO PLAYER */    
    public static void removeChance(){
        playerChances --;
    }
    public static void addChance(){
        playerChances ++;
    }
    public static int getPlayerChances(){
        return playerChances;
    }
    public static void ResetAuxCash(){
        /* ja que que o resetPlayerStatus é só no game over, aqui garante qd eu fica testando um lvl, ela vai zerar*/ 
        playerCashAux = 0;
    }
    /*MÉTODO REFERENTE AO STATUS GERAL DO PLAYER: */
    public static void resetPlayerStatus(){
	    CleanPlayerWeapons();
        cleanPlayerUpgradeParts();
        ResetPlayerWeaponsStatus();
        ResetPlayerItensStatus();
        ResetBombAndShieldStatus();
        playerChances = Constantes.NUMERO_CHANCES_DEFAULT;
        playerCash = 0f;
        playerCashAux = 0;
	}   
}