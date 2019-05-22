using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShieldInfoContainer {
	public float shieldOriginalPrice;//usado para backup
    public float shieldCurrentPrice;
    public float shieldBuyPriceFactor;
    public float upgradeOriginalPrice;//usado para backup
    public float upgradePriceFactor;//pra calcular novo preco
    public float shieldOriginalDuration;
    public float shieldDurationFactor;
    public int shieldLevel;
	public float upgradeCurrentPrice;  
    public float shieldCurrentDuration;
    public int sellPriceFraction;
}
