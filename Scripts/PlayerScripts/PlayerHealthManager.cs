using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {
	
	private UiManagement uiManagement;
	private float playerHealth;
    private float inicialAmountLife;
 	
	private void Awake() {
		setUiManagementReference();
	}
	public void setInitialLife(){
        inicialAmountLife = playerHealth;
    }
    public void SetPlayerCurrentHealth(float health){
        playerHealth = health;
    }
    public void checkPlayerHealth(){
		if( playerHealth <= 0){
			playerHealth = 0;
        }    
		else if ( playerHealth > inicialAmountLife)
			playerHealth = inicialAmountLife;
           
     }
    public void AddHealth(float healthAmount){
		playerHealth += healthAmount;
        uiManagement.addPlayerHealthAmountUI(healthAmount);
		checkPlayerHealth();
	}
    public void RemoveHealth(float healthAmount){
	    playerHealth -= Mathf.Round(healthAmount);
        uiManagement.decreasePlayerHealthAmountUI(healthAmount);
		checkPlayerHealth();
	}
    public float GetPlayerHealth(){
        return playerHealth;
    }
    public float GetInicialAmountLife(){
		return inicialAmountLife;
	}
    public void SetPlayerHealth(float playerHealth){
        this.playerHealth = playerHealth;
        setInitialLife();
    }
	/*para garantir o sincronismo!!! */
	public void SetLifeBarUi(){
        if(! uiManagement)
            uiManagement = FindObjectOfType<UiManagement>();
        
        uiManagement.SetPlayerlifeBarUI();
        uiManagement.UpdatePlayerlifeBarUI();    
    }
	 public void setUiManagementReference(){
        try{
            uiManagement = FindObjectOfType<UiManagement>();
            uiManagement.SetPlayerlifeBarUI();
            uiManagement.UpdatePlayerlifeBarUI();
            uiManagement.UpdateWeaponIcon();
        }catch(System.Exception e){
            Debug.LogWarning(e);
        }
    }
}
