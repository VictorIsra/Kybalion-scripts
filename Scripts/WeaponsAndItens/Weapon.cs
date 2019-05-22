using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    [SerializeField] Sprite weaponIcon;
    [SerializeField] GameObject weaponUpgradePartPrefab;//Peca que ira ser adicionada ao player ao ter a arma.
    [SerializeField] Transform playerSpawPoint;//é transform mas o que eu uso é a TAG associada a esse transform!
    [SerializeField] List<Ammo> ammoList;
    [SerializeField] float weaponOriginalPrice;//usado para backup
    [SerializeField] float upgradeOriginalPrice;//usado para backup
    [SerializeField] float upgradePriceFactor;//pra calcular novo preco
    [SerializeField] private int weaponSellItemFraction;
    
    /*teste */
    private float upgradeCurrentPrice;
    private float weaponCurrentPrice;
    private float upgradePriceBeforeLastUpgrade;//serve pra calcular o preco de venda da arma apenas
    
    [SerializeField] private int weaponLevel = 0;
    private Player player;
    private List<Transform> upgradePartPrefabSpawnPoints = new List<Transform>();

    public void ConfigWeaponAmmos(){
        try{
            foreach(Ammo ammo in ammoList){
                ammo.SetFirePower();
                if(ammoList.IndexOf(ammo) < weaponLevel)
                    ammo.SetAmmoStatus(true);
                else
                    ammo.SetAmmoStatus(false);    
            }   
        } 
        catch(System.Exception e){
            Debug.LogWarning(e);
        }
    }
    /*se refere ao preco de compra da arma e ao preco que ela valerá ao vende-la baseado no lvl que ela estiver */
    public float GetWeaponCurrentPrice(){
        if(weaponLevel <= 1)
            weaponCurrentPrice = weaponOriginalPrice;
        else
            weaponCurrentPrice = upgradePriceBeforeLastUpgrade;    
        return weaponCurrentPrice;    
    }
    public int GetWeaponSellItemFraction(){
        return weaponSellItemFraction;
    }
    public void SetAndConfigWeapon(){
        ConfigWeaponAmmos();
        SetWeaponUpgradePartSpawnPoints();
    }
    public float GetWeaponOriginalPrice(){
        return weaponOriginalPrice;
    }
    public float GetCurrentUpgradePrice(){
        return upgradeCurrentPrice;
    }
    public void SetUpgradeCurrentPrice(){
       upgradeCurrentPrice = upgradeOriginalPrice;
    }
    public float IncrementUpgradePrice(){
        upgradePriceBeforeLastUpgrade = upgradeCurrentPrice;
        upgradeCurrentPrice += upgradeOriginalPrice * upgradePriceFactor;
        Debug.LogWarning("NOVO VALOR " + upgradeCurrentPrice);
        return upgradeCurrentPrice;
    }
    public void ResetWeaponStatus(){
        //Debug.LogWarning("RESEI ARMA");
        upgradeCurrentPrice = upgradeOriginalPrice;
        weaponCurrentPrice = weaponOriginalPrice;
        weaponLevel = 0;
        ConfigWeaponAmmos();
    }
    public float GetUpgradePriceFactor(){
        return upgradePriceFactor;
    }
    /* só sera necessaria no caso da arma padrao, que sempre estará com o player e portando
    sempre será pelo menos lvl 1. Tb na hora de adicionar uma arma, pois
    prefabs sao nebulosos em permanencias de dados, entao setando na hora de adicionar garante
    que toda arma comprada comecara no lvl 1*/
    public void SetWeaponLevel(int weaponLevel){
        this.weaponLevel = weaponLevel;
        ammoList[0].SetAmmoStatus(true);
    }
    public int GetWeaponLevel(){
        return weaponLevel;
    }
    public void IncrementWeaponLevel(){
       weaponLevel ++;
        if(weaponLevel > Constantes.ITEN_MAX_LEVEL)
            weaponLevel = Constantes.ITEN_MAX_LEVEL;    
    }
    public Sprite getWeaponIcon(){
        return weaponIcon;
    }
    public GameObject getWeaponUpgradePart(){
        return weaponUpgradePartPrefab;
    }
    public string playerSpawPointTag(){
        return playerSpawPoint.tag;
    }
    public string getWeaponUpgradePartTag(){
        return weaponUpgradePartPrefab.tag;
    }
    public List<Ammo> GetWeaponAmmos(){
      
        return ammoList;
    }
    public void SetWeaponUpgradePartSpawnPoints(){
        foreach(Transform child in weaponUpgradePartPrefab.transform )
            upgradePartPrefabSpawnPoints.Add(child);
    }
    public List<Transform> GetWeaponUpgradePartSPawPoints(){
        return upgradePartPrefabSpawnPoints;
    }
}