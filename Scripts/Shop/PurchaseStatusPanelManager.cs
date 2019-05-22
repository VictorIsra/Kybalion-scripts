using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*LIDA COM TUDO RELACIONADO AO PurchaseStatusPanel ( ver esse painel
no inspector. Ele inicialmente é invisivel ): o que acotence
quando clico no butao de comprar,vender,upgrade, que tipo de informacoes
serão msotradas etc...ele nao faz tudo isso sozinho, mas é a classe que
chama as outras para realizar isso! logo, tudo relacionado ao o que aparece
ao se clicar em qualquer coisa relacionado ao botao de upgrade, buy, sell comeca aqui! */

public class PurchaseStatusPanelManager: MonoBehaviour {
	[SerializeField] TextMeshProUGUI textReference;
	[SerializeField] GameObject upgradeInfoReference;
	[SerializeField] GameObject upgradeInfoPanelReference;
	[SerializeField] float CASH;
	private List<GameObject> currentPanel = new List<GameObject>();
	private Object[] purchaseTexts;
	private GameObject purchasePanelRef;
	private GameObject callerRef;
	private string action;
	private Sprite itemImage;
	private Vector2 buttonOriginalPosition;
	private int buttonClicks;
	private int buttonMaxClicks = 1;
	private float sellFactorAux;//um cash do valor do fator de venda
	/*variável auxiliar...*/
	private float currentSellPrice;

	void Start () {
		LoadPurchaseTexts();
		purchasePanelRef = textReference.gameObject.transform.parent.gameObject;
		/*unico método que nao se relaciona com o purchaseStatusPanel. */
		SetCash();
	}
	void LoadPurchaseTexts(){
		try{
			purchaseTexts = Resources.LoadAll("TextAssets/Shop/PurchaseStatus");
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
	public void NoCashText(float itemPrice,string action){
		EnablePurchaseStatusPanel();
		SetUpgradeInfoPanel(false);
		SetCancelButtonVisibility(false);
		SetPanelAtributes(true,false);
		string keyWord;
		if(action == Constantes.UPGRADE)
			keyWord = "UPGRADE PRICE:";
		else
			keyWord = "ITEM PRICE:";
	
		string preco =  (itemPrice).ToString("C2");
		string playerCash = PlayerGlobalStatus.getPlayerCash().ToString("C2");
		string texto = string.Format(purchaseTexts[0].ToString() + keyWord + " {0}\nYOU HAVE: {1}", preco, playerCash);
		textReference.text = texto;
	}
	public void NoWeaponText(){
		EnablePurchaseStatusPanel();
		SetUpgradeInfoPanel(false);
		SetCancelButtonVisibility(false);
		SetPanelAtributes(true,false);
		textReference.text = purchaseTexts[1].ToString();
	}
	public void SucessFullyPurchasedText(float boughPrice){
		EnablePurchaseStatusPanel();
		SetUpgradeInfoPanel(false);
		SetCancelButtonVisibility(false);
		SetPanelAtributes(true,false);
		textReference.text = purchaseTexts[2].ToString();
		PlayerGlobalStatus.decreaseCash(boughPrice);
		UpdateCash();
	}
	public void SucessSoldPurchasedText(float soldPrice){
		EnablePurchaseStatusPanel();
		SetUpgradeInfoPanel(false);
		SetCancelButtonVisibility(false);
		SetPanelAtributes(true, false);
		string texto = string.Format(purchaseTexts[3].ToString() + "{0,24}", soldPrice.ToString("C2"));
		textReference.text = texto;
		PlayerGlobalStatus.addPlayerCash(soldPrice);
		UpdateCash();
	}
	public void SucessUpgradedPurchasedText(){
		EnablePurchaseStatusPanel();
		SetUpgradeInfoPanel(false);	
		SetCancelButtonVisibility(false);
		SetPanelAtributes(true,false);
		textReference.text = purchaseTexts[4].ToString();
//		Debug.LogWarning("vai descontar " + callerRef.GetComponent<ItemManager>().GetUpgradePriceAux() );
		PlayerGlobalStatus.decreaseCash(callerRef.GetComponent<ItemManager>().GetUpgradePriceAux());
		UpdateCash();
	}
	private void WannaBuyWannaSellWannaUpgradeText(string action, float price, int sellPriceFraction){
		//v[5] = wannaBuy v[6] = wannaSell v[7] wannaupgrade
		if(action == Constantes.BUY){
			string  preco =  price.ToString("C2");
			string texto = string.Format(purchaseTexts[5].ToString() + "{0,12}", preco);
			textReference.text = texto;
			SetUpgradeInfoPanel(false);
		}
		else if(action == Constantes.SELL){
			string  preco = WeaponSellAjust(callerRef, price).ToString("C2");//(price/sellPriceFraction).ToString("C2");
			/*se der erro apague as proxima 2 linhas e descomente a de cima: */
			preco = preco.Replace("$","");
			preco = ((float)System.Convert.ToDouble(preco)/sellPriceFraction).ToString("C2");	
			sellFactorAux = sellPriceFraction;
			string texto;
			if(callerRef.CompareTag(Constantes.WEAPON))
				texto = string.Format(purchaseTexts[6].ToString() + "{0,38}", preco);
			/*o texto no caso de venda de bombas/shields é diferente */	
			else
				texto = string.Format(purchaseTexts[8].ToString() + "{0,12}", preco);
			SetSellPrice(preco);
			textReference.text = texto;
			SetUpgradeInfoPanel(false);

		}
		//upgrade caso contrario
		else{
			float upgradePrice = callerRef.GetComponent<ItemManager>().GetUpgradeCurrentPrice();
			string preco =  (upgradePrice).ToString("C2");
			preco = WeaponUpgradeAjust(callerRef, upgradePrice).ToString("C2");
			string texto = string.Format(purchaseTexts[7].ToString() + "{0,14}", preco);
			textReference.text = texto;
		}
		SetPanelAtributes(false, true);
	}
	/*saco cheio desses codigos da loja, resolve o problema nao de uma maneira bonita, mas resolve...entao n fode */
	public float WeaponUpgradeAjust(GameObject callerRef, float caseBasePrice){
		float price = caseBasePrice;
		/*se for uma arma */
		if(callerRef.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>()){
			Weapon WeaponRef = callerRef.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>();
			if(WeaponRef.GetWeaponLevel() == Constantes.ITEN_MAX_LEVEL - 1){//} || WeaponRef.GetWeaponLevel() == Constantes.ITEN_MAX_LEVEL){
				price =  WeaponRef.GetCurrentUpgradePrice() + (WeaponRef.GetCurrentUpgradePrice() * WeaponRef.GetUpgradePriceFactor());
				return price;
			}
			else return price;	
		}
		else return price;
	}
	public float WeaponSellAjust(GameObject callerRef, float caseBasePrice){
		float price = caseBasePrice;
		/*se for uma arma */
		if(callerRef.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>()){
			Weapon WeaponRef = callerRef.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>();
			if( WeaponRef.GetWeaponLevel() == Constantes.ITEN_MAX_LEVEL){
				price =  WeaponRef.GetCurrentUpgradePrice() + (WeaponRef.GetCurrentUpgradePrice() * WeaponRef.GetUpgradePriceFactor());
				return price;
			}
			else return price;	
		}
		else return price;
	}
	public void SetSellPrice(string preco){
		preco = preco.Replace("$","");
		currentSellPrice = ((float)System.Convert.ToDouble(preco));
	}
	/*/////acima são métodos "gambiarras" */
	public void OptionManager(GameObject caller, string action){
		//Debug.LogWarning(caller.name);
		if(!MaxClicksAchieved()){
			callerRef = caller;
			//itemImage = callerRef.GetComponent<ItemManager>().GetItemIcon();
			SetImageOrPanel(callerRef,action);
			float price = GetItemPrice(callerRef);
			int sellPriceFraction = GetSellPriceFraction(callerRef);
			this.action = action;

			EnablePurchaseStatusPanel();
			SetCancelButtonVisibility(true);
			WannaBuyWannaSellWannaUpgradeText(action, price, sellPriceFraction);
		}	
	}
	private float GetItemPrice(GameObject caller){
		if(caller.CompareTag(Constantes.WEAPON))
			return caller.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>().GetWeaponCurrentPrice();
		/*bombas e escudos basta isso: */
		else
			return callerRef.GetComponent<ItemManager>().GetItemPrice();
	}
	private int GetSellPriceFraction(GameObject caller){

		caller = caller.GetComponent<ItemManager>().GetItemPrefab();

		if(caller.CompareTag(Constantes.WEAPON))
			return caller.GetComponent<Weapon>().GetWeaponSellItemFraction();
		else if (caller.CompareTag(Constantes.BOMB)){
			return caller.GetComponent<Bomb>().GetSellPriceFraction();
		}		
		else if(caller.CompareTag(Constantes.SHIELD))
			return caller.GetComponent<Shield>().GetSellPriceFactor();			
		else
			return 1;
	}
	private float GetItemBuyPrice(GameObject caller){
		caller = caller.GetComponent<ItemManager>().GetItemPrefab();

		if(caller.CompareTag(Constantes.WEAPON))
			return caller.GetComponent<Weapon>().GetWeaponSellItemFraction();
		else if (caller.CompareTag(Constantes.BOMB)){
			return caller.GetComponent<Bomb>().GetSellPriceFraction();
		}		
		else if(caller.CompareTag(Constantes.SHIELD))
			return caller.GetComponent<Shield>().GetSellPriceFactor();			
		else
			return 1;
	}
	private void SetImageOrPanel(GameObject caller, string action){
		/* se for comprar ou vender, a imagem é a mesma,
		se for upgrade, mostrar painel com upgrades */
		if(action == Constantes.UPGRADE){
			GenerateNextUpgradeAtributesOutput(caller);
			itemImage = null;
		}
		else
			itemImage = callerRef.GetComponent<ItemManager>().GetItemIcon();

	}
	public void GenerateNextUpgradeAtributesOutput(GameObject caller){
		try{
			SetUpgradeInfoPanel(true);
			ClearPanels();
			caller.transform.Find(Constantes.ITEM_ICON_PANEL).GetComponent<AmmosPanelsManager>().GeneratePanelOutput(currentPanel, upgradeInfoReference, upgradeInfoPanelReference,true);
		 	ManagePanelVisibility(caller);
			   
			/*no caso de bombas ou shields, somente o painel referente ao caller deve ser exibido, sempre! */
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
	private void ManagePanelVisibility(GameObject caller){
		int nextItenLevel = 0;
			/*só as armas precisarão dessa checagem, pra só mostrar os paines de interesse */
		if(caller.CompareTag(Constantes.WEAPON)){
			Weapon currentWeaponRef = caller.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>();
			nextItenLevel =  currentWeaponRef.GetWeaponLevel() + 1;
			foreach(GameObject panel in currentPanel){
				if(currentPanel.IndexOf(panel) == nextItenLevel - 1){
					panel.SetActive(true);
				}		
				else{
					panel.SetActive(false);		
				}
			}
		}
	}
	private void SetUpgradeInfoPanel(bool active){
		upgradeInfoPanelReference.SetActive(active);
	}
	private bool MaxClicksAchieved(){
		/*como com cada click a posicao relativa do botoes poderia variar,
		essa funcao garante que pra cada evento de tentativa de compra/venda/update,
		as funcoes que alteram a posicao relativa dos botoes só sera chamada uma vez por evento */
		buttonClicks++;
		if(buttonClicks > buttonMaxClicks)
			return true;
		else
			return false;	
	}
	private void ResetClicks(){
		buttonClicks = 0;
	}
	public void ManageAction(){
		/*primeira coisa que rola qd confirma uma acao ( de compra,venda upgrde etc) */
		if(callerRef){
			if(action == Constantes.BUY){
				callerRef.GetComponent<ItemManager>().BuyItem();
				SucessFullyPurchasedText(callerRef.GetComponent<ItemManager>().GetItemPrice());
			}
			else if(action == Constantes.SELL){
				callerRef.GetComponent<ItemManager>().SellItem();
				SucessSoldPurchasedText(currentSellPrice);//callerRef.GetComponent<ItemManager>().GetItemPrice());
			}
			//upgrade
			else{
				callerRef.GetComponent<ItemManager>().UpgradeItem();
				SucessUpgradedPurchasedText();
			}
		}	
	}
	private void SetPanelAtributes(bool ignoreLayoutStatus, bool activeIconStatus){
		GameObject child = FindChildByTag(Constantes.CONFIRM_BUTTON);
		SetItemIcon(activeIconStatus);
		if(child.GetComponent<LayoutElement>()){
			child.GetComponent<LayoutElement>().ignoreLayout = ignoreLayoutStatus;
			child.GetComponent<RectTransform>().anchoredPosition = new Vector2(10.5f,10.5f);//(0.5f,0.5f);
			
		}
	}
	public void ClearPanels(){
		currentPanel.Clear();
	}
	private void SetItemIcon(bool activeIconStatus){
		GameObject child = purchasePanelRef.transform.Find(Constantes.ITEM_ICON).gameObject;
		child.SetActive(activeIconStatus);
		child.GetComponent<Image>().sprite = itemImage;
	}
	public void DisablePurchaseStatusPanel(){
		DestroyUpgradeInfoPanelChilds();
		purchasePanelRef.SetActive(false);
		ResetClicks();
	}
	private void DestroyUpgradeInfoPanelChilds(){
		GameObject painel = purchasePanelRef.gameObject.transform.Find(Constantes.UPGRADE_INFO_PANEL).gameObject;
		int index = 0;
		foreach(Transform child in painel.transform){
			Destroy(child.gameObject);
			index++;		
		}
	}
	public void EnablePurchaseStatusPanel(){
		purchasePanelRef.SetActive(true);
	}
	public void SetCancelButtonVisibility(bool status){
		if(purchasePanelRef){
			GameObject child = FindChildByTag(Constantes.CANCEL_BUTTON);
			child.SetActive(status);
			CheckStatus(status);
		}
	}
	private GameObject FindChildByTag(string tag){
		try{
			foreach(Transform child in purchasePanelRef.transform.Find(Constantes.PANEL))
				if(child.CompareTag(tag))
					return child.gameObject;
			return null;
		}catch(System.Exception e){
			Debug.LogWarning(e);
			return null;
		}			
	}
	private void CheckStatus(bool status){
		GameObject child = FindChildByTag(Constantes.CONFIRM_BUTTON);
		if(status)
			child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Constantes.CONFIRM_BUTTON_TEMP_TEXT;
		else
			child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Constantes.CONFIRM_BUTTON_ORIGINAL_TEXT;	
	}	
	private void SetCash(){
		PlayerGlobalStatus.addPlayerCash(CASH);
		GameObject cashPanel = transform.Find(Constantes.CASH_PANEL).gameObject;
		cashPanel.GetComponentInChildren<TextMeshProUGUI>().text = Constantes.CASH + "\n" + PlayerGlobalStatus.getPlayerCash().ToString("C2");
	}
	private void UpdateCash(){
		GameObject cashPanel = transform.Find(Constantes.CASH_PANEL).gameObject;
		cashPanel.GetComponentInChildren<TextMeshProUGUI>().text = Constantes.CASH + "\n" + PlayerGlobalStatus.getPlayerCash().ToString("C2");
	}
}
