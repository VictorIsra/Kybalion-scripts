using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManagement : MonoBehaviour {

 	/*EXPLICITE AS REFERENCIAS, POIS A AUSÊNCIA DISSO QUE CAUSAVA O COMPORTAMENTO
	 BUGADO AO MONTAR O BINÁRIO. ELE PERDIA A REFERENCIA E NAO ACHAVA POIS
	 TALVEZ NO BINARIO O CANVAS FOSSE RENDERIZADO DEPOIS. EM RESUMO: AO
	 EXPLICITAR AS REFERENCIAS, VOCÊ GARANTE QUE ELE NÃO IRÁ SE PERDER! */
	PlayerHealthManager playerHealthManager;
	BossHealthManager bossHealthManager;
	[SerializeField] Slider playerLifeBar;
	[SerializeField] Slider bossLifeBar;
	[SerializeField] Image weaponIcon;
	[SerializeField] TextMeshProUGUI cashText;
	[SerializeField] TextMeshProUGUI chancesText;
	[SerializeField] TextMeshProUGUI amountText;
	[SerializeField] Sprite bombIcon;
	[SerializeField] Sprite shieldIcon;
	[SerializeField] Image levelColor;

	private Image playerLifeBarFill; 
	private Image bossLifeBarFill; 
	private bool BossSpawned = false;
	
	private void Awake() {
		try{
			checkReferences();
			setScoreAndVidaText();
			UpdateChancesText();
			UpdateItenIcon(PlayerItensManager.currentItenIndex);
			SetBossLifeBarUIVisibility(false);
		}catch(System.Exception e){
			Debug.LogWarning(e);	
		}
	}
	
	private void checkReferences(){
		
		try{
			if(!playerHealthManager)
				playerHealthManager = FindObjectOfType<PlayerHealthManager>();	
			if(!playerLifeBarFill)
				playerLifeBarFill = playerLifeBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
			if(BossSpawned){
				if(!bossHealthManager)
					bossHealthManager = FindObjectOfType<BossHealthManager>();	
			}
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
	//PLAYER STATUS UI( HEALTH, CASH, CHANCES ):
	private void setScoreAndVidaText(){
		cashText.text = "CASH:  " + PlayerGlobalStatus.getPlayerCash().ToString("0.00");
		chancesText.text = "CHANCES:  " + PlayerGlobalStatus.getPlayerChances().ToString();
	}
	public void UpdateChancesText(){
		chancesText.text = "CHANCES:  " + PlayerGlobalStatus.getPlayerChances().ToString();
	}
	public void UpdateScoreText(){
		cashText.text = "CASH:  " + PlayerGlobalStatus.getPlayerCash().ToString("0.00");
	}
	public void UpdateWeaponIcon(){
		try{
		weaponIcon.sprite = PlayerGlobalStatus.getPlayerWeaponByIndex(PlayerGlobalStatus.GetPlayerWeaponIndex()).GetComponent<Weapon>().getWeaponIcon();
		}
		catch(System.Exception e){
			Debug.LogWarning("PATH DOS ÍCONES NÃO ENCONTRADO...CHEQUE O PATH EM <Constantes.cs>");
			Debug.LogWarning(e);
		}
	}	
	public void UpdateItenIcon(int itenIndex){
		/*indice 0 é bomba, 1 e shield */
		Transform itemUIRef = gameObject.transform.GetChild(0).transform.Find(Constantes.PLAYER_PANEL).transform.Find(Constantes.ITEM_UI);

		if(itemUIRef){
			if(itenIndex == 0){
				itemUIRef.Find(Constantes.ITEM_IMAGE).gameObject.GetComponent<Image>().sprite = bombIcon;
				amountText.text = "X " + PlayerGlobalStatus.GetPlayerBombsNumber().ToString();
			}	
			else if(itenIndex == 1){
				itemUIRef.Find(Constantes.ITEM_IMAGE).gameObject.GetComponent<Image>().sprite = shieldIcon;	
				amountText.text = "X " + PlayerGlobalStatus.GetPlayerShieldsNumber().ToString();
			}
		}
		else
			Debug.LogWarning(Constantes.MISSING_COMPONENT_NAME);
	}
	//PLAYER UI:
	public void SetPlayerlifeBarUI(){
		/* NAO tire a chamada de referencias daqui, pois outras
		classes referenciam essa classe, as vezes, antes do Awake daqui ser chamado...
		(estranho, sei, mas testei com os debugs, apenas deixe ;) )*/
		checkReferences();
		playerLifeBar.maxValue = playerHealthManager.GetInicialAmountLife();
		playerLifeBar.minValue = 0;
		playerLifeBarFill = playerLifeBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
	}
	public void addPlayerHealthAmountUI(float healthAmount){
		playerLifeBar.value += healthAmount;
		UpdatePlayerlifeBarUI();
	}
	public void decreasePlayerHealthAmountUI(float healthAmount){
		playerLifeBar.value -= healthAmount;
		UpdatePlayerlifeBarUI();
	}
	public void UpdatePlayerlifeBarUI(){
		//Debug.LogWarning()
		float playerHealth = playerHealthManager.GetPlayerHealth();
		float playerOriginalHealth = playerHealthManager.GetInicialAmountLife();
		playerLifeBar.value = playerHealth;

		if(playerHealth < Constantes.AVERAGE_HEALTH * playerOriginalHealth)
			playerLifeBarFill.color = Color.yellow;
		if(playerHealth < Constantes.LOW_HEALTH * playerOriginalHealth)
			playerLifeBarFill.color = Color.red;	
		if(playerHealth >= Constantes.AVERAGE_HEALTH * playerOriginalHealth)
			playerLifeBarFill.color = Color.green;	
	}
	//BOSS UI:
	public void SetBossLifeBarUIVisibility(bool bossSpawned){
		this.BossSpawned = bossSpawned;
		if(BossSpawned)
			bossLifeBar.gameObject.SetActive(true);
			
		else
			bossLifeBar.gameObject.SetActive(false);		
	}
	public void decreaseBossHealthAmountUI(float healthAmount){
		bossLifeBar.value -= healthAmount;
		updateBosslifeBarUI();
	}
	public void setBosslifeBarUI(){
		/* NAO tire a chamada de referencias daqui, pois outras
		classes referenciam essa classe, as vezes, antes do Awake daqui ser chamado...
		(estranho, sei, mas testei com os debugs, apenas deixe ;) )*/
		checkReferences();
		bossLifeBar.maxValue = bossHealthManager.GetOriginalHealth();
		bossLifeBar.minValue = 0;
		bossLifeBarFill = bossLifeBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
	}
	public void updateBosslifeBarUI(){
		
		float bossHealth = bossHealthManager.GetHealth();
		float bossOriginalHealth = bossHealthManager.GetOriginalHealth();
		bossLifeBar.value = bossHealth;

		if(bossHealth < Constantes.AVERAGE_HEALTH * bossOriginalHealth)
			bossLifeBarFill.color = Color.yellow;
		if(bossHealth < Constantes.LOW_HEALTH * bossOriginalHealth)
			bossLifeBarFill.color = Color.red;	
		if(bossHealth >= Constantes.AVERAGE_HEALTH * bossOriginalHealth)
			bossLifeBarFill.color = Color.green;	
	}	
}
