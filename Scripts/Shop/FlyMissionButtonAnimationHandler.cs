using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;


public class FlyMissionButtonAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	Animator animatable;
	
	void Awake(){
		animatable = GetComponentInChildren<Animator>();
	}
	public void OpenGateAnimation(){
		animatable = GetComponentInChildren<Animator>();
		if(animatable){	
			animatable.GetComponent<Animator>().SetTrigger(Constantes.OPEN);
			animatable.GetComponent<Animator>().ResetTrigger(Constantes.CLOSE);
		}	
	}
	public void CloseGateAnimation(){
		animatable = GetComponentInChildren<Animator>();
		if(animatable){	
			animatable.GetComponent<Animator>().SetTrigger(Constantes.CLOSE);
			animatable.GetComponent<Animator>().ResetTrigger(Constantes.OPEN);

		}	

	}
	public void OnPointerEnter(PointerEventData eventData){
		OpenGateAnimation();
	}
	public void OnPointerExit(PointerEventData eventData){
		CloseGateAnimation();
	}
}
