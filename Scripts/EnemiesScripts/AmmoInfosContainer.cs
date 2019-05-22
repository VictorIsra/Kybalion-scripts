using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* essa struct gravará as informacoes referentes a uma municao,o tempo entre uma
onda de tiros e outra etc.*/
[System.Serializable]
public struct AmmoInfosContainer {
	/* ao atirar um numero maxFireNumberBeforeNewFireWaveDelay,
	haverá uma espera de fireWaveDelay.*/
	private Ammo currentAmmo;
	public int maxFireNumberBeforeNewFireWaveDelay;
	public float fireWaveDelay;
	public int fireCounter;
	//tipos de tiro:
	public bool circularFire;
	public bool directionalFire;
	public bool radialFire;
	public bool defaultFire;
}