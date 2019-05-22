using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield: MonoBehaviour{

    [SerializeField] public ShieldInfoContainer shieldSettings;
    [SerializeField] private GameObject shieldAura;
    private static bool priceSetted = false;/* só uma vez, isso fara o preco current ser igual ao original */

    private void Awake() {
        if(!priceSetted)
            SetShieldOriginalCurrentPrice();
    }
    private void SetShieldOriginalCurrentPrice(){
        shieldSettings.shieldCurrentPrice = shieldSettings.shieldOriginalPrice;
        priceSetted = true;
    }
    public Sprite GetShieldIcon(){
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
    public int GetSellPriceFactor(){
        /* é um int..se for 4 será preco/4 etc */
        return shieldSettings.sellPriceFraction;
    }
    public float GetshieldDuration(){
        return shieldSettings.shieldCurrentDuration;
    }
    public float GetShieldOriginalPrice(){
        return shieldSettings.shieldOriginalPrice;
    }
    public float GetShieldCurrentPrice(){
        return shieldSettings.shieldCurrentPrice;
    }
    public void IncrementShieldPrice(){
        shieldSettings.shieldCurrentPrice *= shieldSettings.shieldBuyPriceFactor;
    }
    public float GetCurrentUpgradePrice(){
        return shieldSettings.upgradeCurrentPrice;
    }
    public void SetUpgradeCurrentPrice(){
        shieldSettings.upgradeCurrentPrice = shieldSettings.upgradeOriginalPrice;
    }
    public float IncrementUpgradePrice(){
        shieldSettings.upgradeCurrentPrice *=  shieldSettings.upgradePriceFactor;
        return shieldSettings.upgradeCurrentPrice;
    }
    public GameObject GetShieldAura(){
        return shieldAura;
    }
    public float IncrementShieldDuratation(){
        shieldSettings.shieldCurrentDuration += shieldSettings.shieldDurationFactor;//*= shieldSettings.shieldDurationFactor;
        return shieldSettings.shieldCurrentDuration;
    }
    /* só pra visualizacao, nao muda de verdade */
    public string GetIncrementedShieldDurationString(){
        string upgradeShieldDuration = "";
        upgradeShieldDuration = ( shieldSettings.shieldCurrentDuration + shieldSettings.shieldDurationFactor).ToString();//* shieldSettings.shieldDurationFactor).ToString();
        return upgradeShieldDuration;
    }
    public void ResetShieldStatus(){
        //Debug.LogWarning("RESETEI SHIELD");
        shieldSettings.upgradeCurrentPrice = shieldSettings.upgradeOriginalPrice;
        shieldSettings.shieldCurrentPrice = shieldSettings.shieldOriginalPrice;
        shieldSettings.shieldCurrentDuration = shieldSettings.shieldOriginalDuration;
        shieldSettings.shieldLevel = 0;
        priceSetted = false;
    }
     public void SetShieldLevel(int shieldLevel){
        shieldSettings.shieldLevel = shieldLevel;

    }
    public int GetShieldLevel(){
        return shieldSettings.shieldLevel;
    }
    public void IncrementShieldLevel(){
        shieldSettings.shieldLevel ++;
        if(shieldSettings.shieldLevel > Constantes.ITEN_MAX_LEVEL)
            shieldSettings.shieldLevel = Constantes.ITEN_MAX_LEVEL;    
    }
    
}