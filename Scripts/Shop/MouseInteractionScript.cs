using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MouseInteractionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	[SerializeField] private  GameObject OutputPanel;
	[SerializeField] private TextAsset inputText;
	private float originalAlpha = 1f;
	private float hoverAlpha = 0.75f;
	private TextAsset originalTextOutput;

	void Awake(){
		SetAlphaOnExceptionalCase();
		originalTextOutput = Resources.Load<TextAsset>("TextAssets/" + Constantes.DEFAULT_OUTPUT_PANEL_TEXT);
	}
	public void OnPointerEnter(PointerEventData eventData){
		if(inputText)
			OutputPanel.GetComponent<TextMeshProUGUI>().text =  inputText.text;
		if(!gameObject.GetComponent<Button>()){/*nao quero
		que isso ocorra com os botoes, pois eles tem seu proprio controle do alpha */
		ChangeAlpha(hoverAlpha);	
		}
		
	}
	public void OnPointerExit(PointerEventData eventData){
		OutputPanel.GetComponent<TextMeshProUGUI>().text = originalTextOutput.text;
		if(!gameObject.GetComponent<Button>()){/*nao quero
		que isso ocorra com os botoes, pois eles tem seu proprio controle do alpha */
		ChangeAlpha(originalAlpha);	
		}
	}
	private void ChangeAlpha(float factor, float duration = 0.5f){
		if(gameObject.GetComponent<Image>())
			gameObject.GetComponent<Image>().CrossFadeAlpha(factor,duration,false);
	}
	private void SetAlphaOnExceptionalCase(){
		if(gameObject.name == Constantes.UPGRADES_PANEL){
			originalAlpha = 0f;
			hoverAlpha = 0.75f;
			ChangeAlpha(originalAlpha);
		}
	}
}
