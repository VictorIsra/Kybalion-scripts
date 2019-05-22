using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
/* ESSA classe efetivamente manipula a lógica das interacoes de comprar,vender,upgrades.
Ela controla o lvl do upgrade, se o player tem dinheiro ou nao etc...ela é auxiliar
ao PurchaseStatusPanelManager.cs. Enquanto a última é a porta de entrada, ou seja, apenas
direciona os eventos, essa classe aqui já trata esses eventos especificamente. Portanto,
basicamente purchaseStatusManager chama os métodos dessa classe aqui!(inclusive).
Essa classe aqui é complementar a classe UpgradesPanel.cs também, mas em menor grau. */

public class ItemManager : MonoBehaviour {
	
	[SerializeField] GameObject item;
	[SerializeField] GameObject globalPanel;
	private int itemUpgradeLevel;
	private float itemPrice;
	private float upgradeCurrentPrice;
	private float upgradePriceAux;/*guar o valor do upgradeCurrentPrice antes de atualizar ( pra no menu de vender, bater o preco) */
	private Sprite itemIcon;

	void Start(){
		CheckChildsTags();
		SetItemIcon();
		UpdatePrices();
	}
	private void CheckChildsTags(){
		foreach(Transform child in transform)
			CheckTag(child.gameObject);
	}
	private void CheckTag(GameObject child){
		try{
			if(child.CompareTag(Constantes.BUY_BUTTON))
				CheckBuyButtonStatus(child);
			else if(child.CompareTag(Constantes.SELL_BUTTON))
				CheckSellButtonStatus(child);
			else if(child.CompareTag(Constantes.UPGRADE_BUTTON))
				CheckUpgradeButtonStatus(child);		
			else if(child.CompareTag(Constantes.ITEM_PRICE))
				SetItemPrice(child);
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}		
	}
	private void CheckBuyButtonStatus(GameObject child){
		SetButtonsStatus(child,false);
	}
	private void CheckSellButtonStatus(GameObject child){
		SetButtonsStatus(child, true, true);
	}
	private void CheckUpgradeButtonStatus(GameObject child){
		if(item.CompareTag(Constantes.WEAPON)){
			if(item.GetComponent<Weapon>().GetWeaponLevel() >= Constantes.ITEN_MAX_LEVEL){
				child.GetComponent<Button>().interactable = false;
			}	
			else
				child.GetComponent<Button>().interactable = true;
		}
		else if(item.CompareTag(Constantes.BOMB)){
			if(item.GetComponent<Bomb>().GetBombLevel() >= Constantes.ITEN_MAX_LEVEL)
				child.GetComponent<Button>().interactable = false;
			else
				child.GetComponent<Button>().interactable = true;	
		}
		else if(item.CompareTag(Constantes.SHIELD)){
			if(item.GetComponent<Shield>().GetShieldLevel() >= Constantes.ITEN_MAX_LEVEL)
				child.GetComponent<Button>().interactable = false;
			else
				child.GetComponent<Button>().interactable = true;	
		}
		else{
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);
		}
	}	
	
	private void SetItemIcon(){
		foreach(Transform child in transform){
			if(child.CompareTag(Constantes.ITEM_ICON_PANEL)){
				itemIcon = child.GetChild(0).GetComponent<Image>().sprite;
			}
		}
	}
	private void UpdatePrices(){
		UpdateItemPricesAndSetFactor();

	}
	private void SetItemPrice(GameObject child){
		string outputText = "";
		if(item.CompareTag(Constantes.WEAPON)){
			/*se vc tiver a arma.... */
			if(item.GetComponent<Weapon>().GetWeaponLevel() > 0)
				outputText = string.Format("");
			else
				outputText = string.Format("WEAPON PRINCE: {0}", item.GetComponent<Weapon>().GetWeaponCurrentPrice().ToString("C2"));

		}
		else if(item.CompareTag(Constantes.BOMB))
			outputText = string.Format("BOMB PRINCE: {0}", item.GetComponent<Bomb>().GetBombCurrentPrice().ToString("C2"));
		else if(item.CompareTag(Constantes.SHIELD))
			outputText = string.Format("SHIELD PRINCE: {0}", item.GetComponent<Shield>().GetShieldCurrentPrice().ToString("C2"));

		child.GetComponent<TextMeshProUGUI>().text = outputText;
	}
	private void UpdateItemPricesAndSetFactor(){
		if(item.CompareTag(Constantes.WEAPON)){
			item.GetComponent<Weapon>().SetUpgradeCurrentPrice();

			if(CalculateBasedUpgrade())
				itemPrice = item.GetComponent<Weapon>().GetCurrentUpgradePrice();
			else
				itemPrice = item.GetComponent<Weapon>().GetWeaponCurrentPrice();
						
			upgradeCurrentPrice = item.GetComponent<Weapon>().GetCurrentUpgradePrice();
		}
		else if(item.CompareTag(Constantes.BOMB)){
			itemPrice = item.GetComponent<Bomb>().GetBombCurrentPrice();
			upgradeCurrentPrice = item.GetComponent<Bomb>().GetCurrentUpgradePrice();	
		}	
		else if(item.CompareTag(Constantes.SHIELD)){
			itemPrice = item.GetComponent<Shield>().GetShieldCurrentPrice();
			upgradeCurrentPrice = item.GetComponent<Shield>().GetCurrentUpgradePrice();
		}		
		else
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);	
			
	}
	private bool CalculateBasedUpgrade(){
		/*se o nivel for um, o preco de venda é baseado no da propria arma, caso contrário será
		baseado no valor do ultimo upgrade */
		if(item.GetComponent<Weapon>().GetWeaponLevel() == 1 || item.GetComponent<Weapon>().GetWeaponLevel() == 0)
			return false;
		else
			return true;	
	}
	/* MÉTODOS CHAMADOS POR OUTRAS CLASSES: */
	public GameObject GetItemPrefab(){
		return item;
	}
	public int GetItemUpgradeLevel(){
		return itemUpgradeLevel;
	}
	public Sprite GetItemIcon(){
		
		return itemIcon;
	}
	public float GetItemPrice(){
		return itemPrice;
	}
	public float GetUpgradeCurrentPrice(){
		return upgradeCurrentPrice;
	}
	public void BuyItemOption(){
		try{
			CheckCash(Constantes.BUY);
		}
		catch(System.Exception e){
			Debug.LogWarning("Referencia perdida!B " + e);
			return;
		}		
	}
	public void UpgradeItemOption(){
		try{
			CheckCash(Constantes.UPGRADE);
		}
		catch(System.Exception e){
			Debug.LogWarning("Referencia perdida!B " + e);
			return;
		}		
	}
	public void SellItemOption(){
		try{
			globalPanel.GetComponent<PurchaseStatusPanelManager>().OptionManager(gameObject, Constantes.SELL);
		}
		catch(System.Exception e){
			Debug.LogWarning("Referencia perdida! " + e);
			return;
		}		
	}
	public void SellItem(){
		PlayerGlobalStatus.RemoveItem(item);
		UpdateUpgradePanelStatus();
		CheckChildsTags();
	}
	public void BuyItem(){
		/*chamado dps que o item fom comprado efetivamente!! */
		/* se a arma nao for repetida ( conseguiu adicionar ), incremente o lvl dela
		e atualize na variavel local itemUpgradeLevel*/
		if(PlayerGlobalStatus.AddPlayerItem(item)){
			if(item.CompareTag(Constantes.WEAPON))
				itemUpgradeLevel = item.GetComponent<Weapon>().GetWeaponLevel();
			else if(item.CompareTag(Constantes.BOMB))
				itemUpgradeLevel = item.GetComponent<Bomb>().GetBombLevel();
			else if(item.CompareTag(Constantes.SHIELD))
				itemUpgradeLevel = item.GetComponent<Shield>().GetShieldLevel();
		}	
		UpdateItemPricesAndSetFactor();		
		UpdateUpgradePanelStatus();
		CheckChildsTags();	
	}
	public void  SetUpgradePriceAux(){
		/*como o upgradeItem() é chamado antes de descontar o valor
		do player, é necessario cashear o valor antigo, pra nao private void OnRenderImage(RenderTexture src, RenderTexture dest) {
			a chance deu descontar um valor maior do que o que deveria ( que era o bug xD)*/
		upgradePriceAux = upgradeCurrentPrice;
	}
	public float GetUpgradePriceAux(){
		return upgradePriceAux;
	}
	private bool AuxAfter(int weaponLevel){
		if(weaponLevel == 1)
			return false;
		else if (weaponLevel == 2)
			return true;	
		else return false;	
		/*dependendo do nivel, a funcao setupgradepriceaux deve ser chamada em momentos dif */
	}
	public void UpgradeItem(){
		//nao preciso referenciar o PLayerGlobalStatus pois o prefab é que é compartilhado, nao uma instancia do mesmo!
		if(item.CompareTag(Constantes.WEAPON)){
			item.GetComponent<Weapon>().IncrementWeaponLevel();
			itemUpgradeLevel = item.GetComponent<Weapon>().GetWeaponLevel();
			if(!AuxAfter(itemUpgradeLevel -1)){
				SetUpgradePriceAux();
				upgradeCurrentPrice =  item.GetComponent<Weapon>().IncrementUpgradePrice();
			}
			else{
				upgradeCurrentPrice =  item.GetComponent<Weapon>().IncrementUpgradePrice();
				SetUpgradePriceAux();
			}	
			UpdateItemPricesAndSetFactor();

		}
		else if(item.CompareTag(Constantes.BOMB)){
			item.GetComponent<Bomb>().IncrementBombLevel();
			itemUpgradeLevel = item.GetComponent<Bomb>().GetBombLevel();
			item.GetComponent<Bomb>().IncrementBombAtributes();
			item.GetComponent<Bomb>().IncrementBombPrice();
			SetUpgradePriceAux();
			upgradeCurrentPrice =  item.GetComponent<Bomb>().IncrementUpgradePrice();
			UpdateItemPricesAndSetFactor();
		}
		else if(item.CompareTag(Constantes.SHIELD)){
			item.GetComponent<Shield>().IncrementShieldLevel();
			itemUpgradeLevel = item.GetComponent<Shield>().GetShieldLevel();
			item.GetComponent<Shield>().IncrementShieldDuratation();
			item.GetComponent<Shield>().IncrementShieldPrice();
			SetUpgradePriceAux();
			upgradeCurrentPrice =  item.GetComponent<Shield>().IncrementUpgradePrice();
			UpdateItemPricesAndSetFactor();
		}
		else{
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);
		}
		//Métodos comuns a todos os tipos de itens
		UpdateUpgradePanelStatus();
		CheckChildsTags();	
	}
	private void CheckCash(string action){
		if(action == Constantes.UPGRADE){
			if(PlayerGlobalStatus.getPlayerCash() >= upgradeCurrentPrice){
				if(PlayerHaveItem() || !item.CompareTag(Constantes.WEAPON))
					globalPanel.GetComponent<PurchaseStatusPanelManager>().OptionManager(gameObject, action);
				else
					globalPanel.GetComponent<PurchaseStatusPanelManager>().NoWeaponText();
			}
			else{
				globalPanel.GetComponent<PurchaseStatusPanelManager>().NoCashText(upgradeCurrentPrice,action);
			}
		}
		//comprar
		else{
			if(PlayerGlobalStatus.getPlayerCash() >= itemPrice)
				globalPanel.GetComponent<PurchaseStatusPanelManager>().OptionManager(gameObject, action);
			else
				globalPanel.GetComponent<PurchaseStatusPanelManager>().NoCashText(itemPrice,action);

		}
	}
	public void UpdateUpgradePanelStatus(){
		//atualiza o painel de upgrades:
		if(transform.Find(Constantes.UPGRADES_PANEL))
			transform.Find(Constantes.UPGRADES_PANEL).gameObject.GetComponent<UpgradesPanel>().CheckUpgrades();
		else
			Debug.LogWarning(Constantes.MISSING_COMPONENT_NAME);
	}
	public bool PlayerHaveItem(){
		if(PlayerGlobalStatus.PlayerHaveItem(item))
			return true;
		else
			return false;	
	}
	public void ResetItemPrefabStatus(){
		//reseta tanto do prefab quanto do vetor do player
		if(item.CompareTag(Constantes.WEAPON)){
			item.GetComponent<Weapon>().ResetWeaponStatus();
			itemUpgradeLevel = 	item.GetComponent<Weapon>().GetWeaponLevel();
			upgradeCurrentPrice = item.GetComponent<Weapon>().GetCurrentUpgradePrice();
		}	
		else if(item.CompareTag(Constantes.BOMB)){
			item.GetComponent<Bomb>().ResetBombStatus();
			itemUpgradeLevel = item.GetComponent<Bomb>().GetBombLevel();
		}
		else if(item.CompareTag(Constantes.SHIELD)){
			item.GetComponent<Shield>().ResetShieldStatus();
			itemUpgradeLevel = item.GetComponent<Shield>().GetShieldLevel();
		}
		else
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);
		CheckChildsTags();		
	}
	private void SetButtonsStatus(GameObject child, bool status, bool sellFlag = false){
		if(item.CompareTag(Constantes.WEAPON)){
			if(PlayerHaveItem())
				child.GetComponent<Button>().interactable = status;	
			else
				child.GetComponent<Button>().interactable = !status;
		}//caso o item seja bomba ou shield, poderá comprar até 10
		else if(item.CompareTag(Constantes.BOMB)){
			//sellFlag serve apenas na tentativa de vender bombas ou shields
			if(!sellFlag){
				if(PlayerGlobalStatus.GetPlayerBombsNumber() >= Constantes.MAX_ITEM_CAPACITY){
					child.GetComponent<Button>().interactable = status;
				}		
				else{
					child.GetComponent<Button>().interactable = !status;
				}
			}else{
				if(PlayerGlobalStatus.GetPlayerBombsNumber() > 0 ){
					child.GetComponent<Button>().interactable = status;
				}		
				else{
					child.GetComponent<Button>().interactable = !status;
				}
			}		
		}
		else if(item.CompareTag(Constantes.SHIELD)){
			if(!sellFlag){
				if(PlayerGlobalStatus.GetPlayerShieldsNumber() >= Constantes.MAX_ITEM_CAPACITY){
					child.GetComponent<Button>().interactable = status;
				}		
				else{
					child.GetComponent<Button>().interactable = !status;
				}
			}else{
				if(PlayerGlobalStatus.GetPlayerShieldsNumber() > 0 ){
					child.GetComponent<Button>().interactable = status;
				}		
				else{
					child.GetComponent<Button>().interactable = !status;
				}
			}		
		}
		else{
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);
		}
	}
}
