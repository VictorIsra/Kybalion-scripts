using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bomb: MonoBehaviour{

    [SerializeField] private GameObject bombExplosion;
    [SerializeField] public BombInfoContainer bombSettings;
     private static bool priceSetted = false;/* só uma vez, isso fara o preco current ser igual ao original */

    private void Awake() {
        if(!priceSetted){
            SetBombOriginalCurrentPrice();
            SetBombAtributes();
        }    
    }   
    private void SetBombOriginalCurrentPrice(){
        bombSettings.bombCurrentPrice = bombSettings.bombOriginalPrice;
        priceSetted = true;
    }
    private float delayBeforeExplode = 1f;
    public GameObject Teste(){
        return bombExplosion;
    }
    
    /*playerglobal status chama isso qd adiciona */
    public void SetBombAtributes(){
        bombSettings.bombCurrentPower = bombSettings.bombOriginalPower;
        bombSettings.bombCurrentRadius = bombSettings.bombOriginalRadius;
    }
    public Sprite GetBombIcon(){
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
    public float GetBombPower(){
        
        return bombSettings.bombCurrentPower;
    }
    public float GetBombRadius(){
        return bombSettings.bombCurrentRadius;
    }
    public float GetBombOriginalPrice(){
        return bombSettings.bombOriginalPrice;
    }
    public float GetBombCurrentPrice(){
        return bombSettings.bombCurrentPrice;
    }
    public float GetCurrentUpgradePrice(){
        return bombSettings.upgradeCurrentPrice;
    }
    public void SetUpgradeCurrentPrice(){
        bombSettings.upgradeCurrentPrice = bombSettings.upgradeOriginalPrice;
    }
    public void IncrementBombPrice(){
        bombSettings.bombCurrentPrice *= bombSettings.bombBuyPriceFactor;
    }
    public float IncrementUpgradePrice(){
        bombSettings.upgradeCurrentPrice += bombSettings.upgradeOriginalPrice * bombSettings.upgradePriceFactor;
        return bombSettings.upgradeCurrentPrice;
    }
    public void IncrementBombAtributes(){
       /* *Debug.LogWarning("VAI AUMENTAR");
        Debug.LogWarning("ANTES " + bombSettings.bombCurrentPower);*/
        bombSettings.bombCurrentPower +=  Mathf.Round(bombSettings.bombCurrentPower * bombSettings.bombPowerFactor);
        bombSettings.bombCurrentRadius += bombSettings.bombCurrentRadius * bombSettings.bombRadiusFactor;

    }
    /* só pra visualizacao, nao muda de verdade */
    public List<string> GetIncrementedBombAtributesString(){
        List<string> newAtributes = new List<string>();
        string bombPower = (  Mathf.Round(bombSettings.bombCurrentPower + (bombSettings.bombCurrentPower * bombSettings.bombPowerFactor))).ToString();
        newAtributes.Add(bombPower);
        string bombRadius = ( bombSettings.bombCurrentRadius + (bombSettings.bombCurrentRadius * bombSettings.bombRadiusFactor)).ToString();
        newAtributes.Add(bombRadius);

        return newAtributes;
    }
    public void ResetBombStatus(){
        //Debug.LogWarning("RESETEI BOMBA");
        bombSettings.upgradeCurrentPrice = bombSettings.upgradeOriginalPrice;
        bombSettings.bombCurrentPrice = bombSettings.bombOriginalPrice;
        bombSettings.bombCurrentPower = bombSettings.bombOriginalPower;
        bombSettings.bombCurrentRadius = bombSettings.bombOriginalRadius;
        bombSettings.bombLevel = 0;
        priceSetted = false;
    }
    public int GetSellPriceFraction(){
        return bombSettings.sellPriceFraction;
    }
    public void SetBombLevel(int bombLevel){
        bombSettings.bombLevel = bombLevel;
    }
    public int GetBombLevel(){
        return bombSettings.bombLevel;
    }
    public void IncrementBombLevel(){
        bombSettings.bombLevel ++;
        if(bombSettings.bombLevel > Constantes.ITEN_MAX_LEVEL)
            bombSettings.bombLevel = Constantes.ITEN_MAX_LEVEL;    
    }
    
    public void TriggerExplosionCoroutine(){
        StartCoroutine(TriggerExplosion());
    }
    IEnumerator TriggerExplosion(){
        yield return new WaitForSeconds(delayBeforeExplode);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //GameObject explosion = Instantiate(tesr, transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(bombExplosion, transform.position, Quaternion.identity);//, gameObject.transform);
        Destroy(gameObject);
    }
    /* pra só aparecer a explosao e nao a bomba em si, nao posso
    deleta-la pos sao objetos associado. podia mudar o alpha, mas
    setar o sprite pra null tb serve */
    public void MakeBombSpriteInvisible(){
        if(gameObject.GetComponent<SpriteRenderer>())
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // = null;
    }
}