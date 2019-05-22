﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
/* essa classe é complementar ao ItemManager.cs, porém esse aqui trata especificamente
o que acontece ao passar o mouse sobre uma arma/item: ela controla
o texto no painel de informacao que cada item irá exibir, bem como o que
aparecerá no AmmosPanels ( que na verdade nao é só de ammo mas tb de bomba e shields)
e no InfoPanel
Por essa interacao ser grande, fiz uma classe só pra isso, que é esta aqui */
public class AmmosPanelsManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] GameObject ammosPanelsRef;
	[SerializeField] GameObject panelRef;
	[SerializeField] GameObject itemReference;

	[SerializeField] TextMeshProUGUI[] outPutPanel;
	private OrderedDictionary panels = new OrderedDictionary(); //pra referenciar por strings em vez de indices numericos
	private OrderedDictionary inputTexts = new OrderedDictionary();
	[SerializeField] TextAsset[] inputsTextAsset;
	private List<GameObject> currentPanel = new List<GameObject>();
	private List<Ammo> ammoList = new List<Ammo>();
	private TextAsset originalTextOutput;
	Color colorToFadeTo;
	private float currentFirePower;
	private float currentFireDelay;

	void Start () {
		SetCrossFadeAlpha();
		GenerateDictionarys();
		CheckItemType();
		LoadDefaultMessage();
	}
	/*esses dois métodos são os gatilhos iniciais */
	public void OnPointerEnter(PointerEventData eventData){
		GenerateOutputTexts(inputsTextAsset[(int)inputTexts[Constantes.WEAPON_INFO]].text);
		gameObject.GetComponent<Image>().CrossFadeAlpha(1,0.5f,false);
	}
	public void OnPointerExit(PointerEventData eventData){
		GenerateInfoPanelOutput(originalTextOutput.text);
		ClearPanels();
		gameObject.GetComponent<Image>().CrossFadeAlpha(0,0.5f,false);

	}
	private void SetCrossFadeAlpha(){
		gameObject.GetComponent<Image>().CrossFadeAlpha(0,0f,false);
	}
	private void GenerateDictionarys(){
		GenerateOutputPanelsDicionary();
		GenerateInputTextsDicionary();
	}
	private void GenerateOutputPanelsDicionary(){
		panels.Add(Constantes.INFO_PANEL_OUTPUT, 0);
		panels.Add(Constantes.AMMO_INFO_PANEL_OUTPUT, 1);
	}
	//só usei dicionarios pra nao referenciar o indice por numero, mas sim por strings
	private void GenerateInputTextsDicionary(){
		inputTexts.Add(Constantes.WEAPON_INFO, 0);
		inputTexts.Add(Constantes.CURRENT_AMMO_FIRE_POWER, 1);
		inputTexts.Add(Constantes.CURREN_AMMO_FIRE_RATE, 2);
		inputTexts.Add(Constantes.CURRENT_AMMO_FIRE_DELAY, 3);
		inputTexts.Add(Constantes.AMMO_INFO, 4 );
		inputTexts.Add(Constantes.BOMB_CURRENT_POWER, 5 );
		inputTexts.Add(Constantes.BOMB_CURRENT_RADIUS, 6 );
		inputTexts.Add(Constantes.SHIELD_CURRENT_DURATION, 7 );
		
	}
	private void CheckItemType(){
		GameObject itemRef =  itemReference.GetComponent<ItemManager>().GetItemPrefab();
		if(itemRef.CompareTag(Constantes.WEAPON))
			GetCurrentWeaponAmmos();
		else if(itemRef.CompareTag(Constantes.BOMB)){
			//Debug.LogWarning("CHECITEMTYPE BOMBAA");
		}	
		else if(itemRef.CompareTag(Constantes.SHIELD)){
			//Debug.LogWarning("CHECITEMTYPE shield");
		}	
	}
	private List<Ammo> GetCurrentWeaponAmmos(){
		ammoList = itemReference.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Weapon>().GetWeaponAmmos();
		return ammoList;
	}
	private void LoadDefaultMessage(){
		try{
			originalTextOutput = Resources.Load<TextAsset>("TextAssets/" + Constantes.DEFAULT_OUTPUT_PANEL_TEXT);
		}catch(System.Exception e){
			Debug.LogWarning("Path não encontrado, checar <Constantes.cs>" + e);
		}	
	}
	private void GenerateOutputTexts(string weaponInfoText){//TextAsset[] inputTexts){
		foreach(string key in panels.Keys){
			if(key.Equals(Constantes.INFO_PANEL_OUTPUT))
				GenerateInfoPanelOutput(weaponInfoText);
			else if(key.Equals(Constantes.AMMO_INFO_PANEL_OUTPUT))
				GeneratePanelOutput(currentPanel, panelRef, ammosPanelsRef);
			else
			Debug.LogWarning("N tem key encontrada");
		}
	}
	private void GenerateInfoPanelOutput(string texto){
		outPutPanel[0].text = texto;
	}
	public void ClearPanels(){
		foreach(GameObject panel in currentPanel){
			Destroy(panel);
		}	
		currentPanel.Clear();	
	}
	private void ShowItemAmountOnInfoPanel(string itemName, int amount){
		outPutPanel[0].text += string.Format("\n\nYOU HAVE: {0}/{1} {2}",amount, Constantes.MAX_ITEM_CAPACITY, itemName);
	}
	private string GetItemTypeTag(){
		return gameObject.transform.parent.tag;
	}
	public void GeneratePanelOutput(List<GameObject> currentPanel, GameObject panelRef, GameObject parentPanelRef, bool showNextAtributes = false){
		string itemType = GetItemTypeTag();

		if(itemType == Constantes.WEAPON){
			
			foreach(Ammo ammo in ammoList){

				string output = "";
				//ordem aqui é importante, instanciar antes de usar ( logico rs)
				currentPanel.Add(Instantiate(panelRef));
		
				//se tiver a municao habilitada
				if(ammo.GetAmmoStatus() || parentPanelRef.name == Constantes.UPGRADE_INFO_PANEL){
					output = SetPanelOutputText(ammo.getAmmoPrefab().name, ammo.GetAmmoFirePower().ToString("#.##"),
						(1/ammo.GetFireDelay()).ToString("#.#"), ammo.GetAmmoType());
				}
				//se ainda nao tiver a municao habilitada	
				else{
					output = SetPanelOutputText("???", ammo.GetAmmoFirePower().ToString("#.##")
						,(1/ammo.GetFireDelay()).ToString("#.#"), ammo.GetAmmoType());
					var colorAlpha = Color.black;
					colorAlpha.a = 0.75f;
					currentPanel[currentPanel.Count - 1].transform.Find(Constantes.PANEL).transform.Find(Constantes.AMMOS_IMAGE).gameObject.GetComponent<Image>().color = colorAlpha;
					currentPanel[currentPanel.Count - 1].GetComponent<Image>().color = colorAlpha;
					currentPanel[currentPanel.Count - 1].transform.Find(Constantes.AMMOS_INFO_PANEL).gameObject.GetComponent<TextMeshProUGUI>().color = colorAlpha;
				}
				//formatacao padrao pra ambos os casos
				currentPanel[currentPanel.Count - 1].transform.SetParent(parentPanelRef.transform, false);
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.PANEL).transform.Find(Constantes.AMMOS_IMAGE).gameObject.GetComponent<Image>().sprite = ammo.GetAmmoSprite();
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.AMMOS_INFO_PANEL).gameObject.GetComponent<TextMeshProUGUI>().text = output;
						
			}	
		}
		//se for uma bomba
		else if(itemType == Constantes.BOMB){
			string output = "";
				//ordem aqui é importante, instanciar antes de usar ( logico rs)
				currentPanel.Add(Instantiate(panelRef));
				Bomb bombRef = itemReference.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Bomb>();
				/* se for o painel de upgrade, mostrará o status do próximo level */
				if( parentPanelRef.name == Constantes.UPGRADE_INFO_PANEL ){
					output = SetPanelOutputText(bombRef.name, bombRef.GetBombPower().ToString("#.##"), bombRef.GetBombRadius().ToString("#.##"),bombRef,showNextAtributes);
				}
				/*bombas e shields sempre mostrarão a info, tendo o item ou nao */
				else{
					output = SetPanelOutputText(bombRef.name, bombRef.GetBombPower().ToString("#.##"), bombRef.GetBombRadius().ToString("#.##"),bombRef);
				}
				//formatacao padrao pra ambos os casos
				currentPanel[currentPanel.Count - 1].transform.SetParent(parentPanelRef.transform, false);
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.PANEL).transform.Find(Constantes.AMMOS_IMAGE).gameObject.GetComponent<Image>().sprite = bombRef.GetBombIcon();
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.AMMOS_INFO_PANEL).gameObject.GetComponent<TextMeshProUGUI>().text = output;
			
			/* PRINTA AS INFORMACOES DA QUANTIDADE */
			ShowItemAmountOnInfoPanel("BOMBS", PlayerGlobalStatus.GetPlayerBombsNumber());

		}
		//se for um shield
		else{
			string output = "";
				//ordem aqui é importante, instanciar antes de usar ( logico rs)
				currentPanel.Add(Instantiate(panelRef));
				Shield shieldRef = itemReference.GetComponent<ItemManager>().GetItemPrefab().GetComponent<Shield>();
				
				/* se for o painel de upgrade, mostrará o status do próximo level */
				if( parentPanelRef.name == Constantes.UPGRADE_INFO_PANEL ){
					output = SetPanelOutputText(shieldRef.name, shieldRef.GetshieldDuration().ToString("#.##"),shieldRef,showNextAtributes);
				}
				else{
					output = SetPanelOutputText(shieldRef.name, shieldRef.GetshieldDuration().ToString("#.##"),shieldRef);
				}
				//formatacao padrao pra ambos os casos
				currentPanel[currentPanel.Count - 1].transform.SetParent(parentPanelRef.transform, false);
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.PANEL).transform.Find(Constantes.AMMOS_IMAGE).gameObject.GetComponent<Image>().sprite = shieldRef.GetShieldIcon();
				currentPanel[currentPanel.Count - 1].transform.Find(Constantes.AMMOS_INFO_PANEL).gameObject.GetComponent<TextMeshProUGUI>().text = output;
			
			/* PRINTA AS INFORMACOES DA QUANTIDADE */
			ShowItemAmountOnInfoPanel("SHIELDS", PlayerGlobalStatus.GetPlayerShieldsNumber());
		}
	}
	private string SetPanelOutputText(string ammoName, string firePower,
	string fireRate,string fireDelay){
		fireRate += " RPS";
		string outputText = string.Format("{0, 20}\n\n{1} {2}\n{3} {4}\n{5} {6}", ammoName,
			inputsTextAsset[(int)inputTexts[Constantes.CURRENT_AMMO_FIRE_POWER]].text,firePower,
			inputsTextAsset[(int)inputTexts[Constantes.CURREN_AMMO_FIRE_RATE]].text,fireRate,
			inputsTextAsset[(int)inputTexts[Constantes.CURRENT_AMMO_FIRE_DELAY]].text,fireDelay);
		return outputText;
	}
	private List<string> GetBombUpdatedValues(string bombPower, string bombRadius
	,Bomb bombref){
		List<string> values = new List<string>();
		values = bombref.GetIncrementedBombAtributesString();
		return values;
	}
	private string SetPanelOutputText(string bombName, string bombPower, string bombRadius
	,Bomb bombRef, bool showNextAtributes = false){
		string outputText = "";
		//Debug.LogWarning("entrei com "  + showNextAtributes);
		//caso onde quero mostrar o valor upgradiado
		if(showNextAtributes){
			List<string> updatedValues = GetBombUpdatedValues(bombPower,bombRadius,bombRef);
			outputText = string.Format("{0, 20}\n\n{1} {2}", bombName,
			inputsTextAsset[(int)inputTexts[Constantes.BOMB_CURRENT_POWER]].text,updatedValues[0]);//inputsTextAsset[(int)inputTexts[Constantes.BOMB_CURRENT_RADIUS]].text,updatedValues[1]);
		}
		else{//caso  onde nao quero mostrar o valor do item upgradiado
			outputText = string.Format("{0, 20}\n\n{1} {2}", bombName,
			inputsTextAsset[(int)inputTexts[Constantes.BOMB_CURRENT_POWER]].text,bombPower);/*,
			inputsTextAsset[(int)inputTexts[Constantes.BOMB_CURRENT_RADIUS]].text,bombRadius);*/
		}
		return outputText;
	}
	private string SetPanelOutputText(string shieldName, string shieldDuration,Shield shieldRef, bool showNextAtributes = false){
		string outputText = "";
				//Debug.LogWarning("entrei com "  + showNextAtributes);

		if(showNextAtributes){
			outputText = string.Format("{0, 22}\n\n{1} {2}s", shieldName,
				inputsTextAsset[(int)inputTexts[Constantes.SHIELD_CURRENT_DURATION]].text
					 ,shieldRef.GetIncrementedShieldDurationString());
		}
		else{
			outputText = string.Format("{0, 22}\n\n{1} {2}s", shieldName,
				inputsTextAsset[(int)inputTexts[Constantes.SHIELD_CURRENT_DURATION]].text, shieldDuration);
		}
		return outputText;
	}	 
}