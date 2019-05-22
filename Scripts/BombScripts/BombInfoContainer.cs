using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BombInfoContainer {
 	public float bombOriginalPrice;//usado para backup
	public float bombCurrentPrice;
	public float bombBuyPriceFactor;
	public float upgradeOriginalPrice;//usado para backup
	public float upgradePriceFactor;//pra calcular novo preco
    public float bombOriginalPower;
	public float bombPowerFactor;//calcular o novo dano
    public float bombRadiusFactor;//calcular o novo raio
	public float bombOriginalRadius;
	public float bombCurrentPower;
	public float bombCurrentRadius;
	public float upgradeCurrentPrice;
	public int bombLevel;
	public int sellPriceFraction;
}

