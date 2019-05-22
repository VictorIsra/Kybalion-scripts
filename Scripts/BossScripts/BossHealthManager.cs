using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BossSpriteManager))]

public class BossHealthManager : MonoBehaviour {
	private float originalHealth;//valor original da vida, para usar como referência.
	[SerializeField] float health = 1000;
	private UiManagement uiManagement;
	private BossSpriteManager bossSpriteManager;

	private void Awake() {
		/*Set a vida antes de atualizar no uimanagement, obviamente... */
		setInitialLife();
		SetRefs();
	}
	private void SetRefs(){
        uiManagement = FindObjectOfType<UiManagement>();
		uiManagement.SetBossLifeBarUIVisibility(true);
        uiManagement.setBosslifeBarUI();
        uiManagement.updateBosslifeBarUI();
		bossSpriteManager = GetComponent<BossSpriteManager>();
    }	
	public void RemoveEnemyHealth(float damage){
		health -= Mathf.Round(damage);
		uiManagement.decreaseBossHealthAmountUI(damage);
		bossSpriteManager.ChangeColor(0.1f);
		bossSpriteManager.CheckSprite();
	}
	private void setInitialLife(){
        originalHealth = health;
    }
	public float GetHealth(){
		return health;
	}
	public float GetOriginalHealth(){
		return originalHealth;
	}
}
