using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*gerencia os levels e qual irá carregar, msm que visualmente seja um efeito no canvas.
Por isso, considero um script da pasta Levels e nao da pasta UI */
public class FadeScreenManager : MonoBehaviour {
	Image imageRef;
	
	void Awake(){
		SetRef();
		Reset();
	}

	private void SetRef(){
		imageRef = GetComponent<Image>();
	}
	public void MotherFade(float delay, Level levelRef = null,bool redColor = false, string loadLeveL = "NONE"){
		Time.timeScale = 1f;
		if(redColor)
			SetRedColor();
		StartCoroutine(MotherFadeCoroutine(0.1f, levelRef,redColor,loadLeveL));
	}
	private void SetRedColor(){
		/*antes do game over hehe */
		var tempColor = Color.red;
		tempColor.a = 0f;
        imageRef.color = tempColor;
	}
	private void SelectLevelToLoad(Level levelRef, string loadLevel){
		if(loadLevel == Constantes.START_MENU)
			levelRef.LoadStarMenuFromTryAgainPosFade();
		else if(loadLevel == Constantes.LEVEL1)
			levelRef.LoadFirstLevelPosFade();
		else if(loadLevel == Constantes.HOW_TO_MENU)
			levelRef.LoadHowToMenuPosFade();
		else if (loadLevel == Constantes.HOW_TO_MENU2)
			levelRef.LoadHowToMenu2PosFade();	
		else if(loadLevel == Constantes.CREDITS_MENU)
			levelRef.LoadCredtisMenuPosFade();
		else if(loadLevel == Constantes.SHOP_MENU)
			levelRef.LoadShopMenuPosFade();
		else if(loadLevel == Constantes.QUIT)
			levelRef.QuitPosFade();
		else if (loadLevel == Constantes.SAME_LEVEL)
			levelRef.LoadLevelAgainPosFade();
		else if(loadLevel == Constantes.NEXT_LEVEL)
			levelRef.loadNextGameScenePosFade();	
		else
			Debug.LogWarning("UM ERRO OCORREU");					
	}
	IEnumerator MotherFadeCoroutine(float delay,Level levelRef = null, bool redColor = false,string loadLeveL = "NONE",int steps = 10,float rate = 0.1f){
		for(int i = 0; i < steps;i++){
			yield return StartCoroutine(FadeScreen(delay,rate));
		}	
		if(levelRef){
			if(!redColor){
				if(loadLeveL == Constantes.NONE)
					levelRef.loadTryAgainPosFade();
				else
					SelectLevelToLoad(levelRef,loadLeveL);
			}	
			else
				levelRef.LoadGameOverPosFade();	
		}
		MouseVisibility.ShowCursor();
	}
	IEnumerator FadeScreen(float delay, float rate){
		var tempColor = imageRef.color;
        tempColor.a += rate;
        imageRef.color = tempColor;
		yield return new WaitForSeconds(delay);
	}
	/*usado em bombas */
	public void WhiteFate(){
		StartCoroutine(WhiteFadeCoroutine());
	}
	IEnumerator WhiteFadeCoroutine(int steps = 5, float fade = 0.2f){
		SetWhiteColor();
		for(int i = 0; i < steps;i++){
			yield return StartCoroutine(FadeScreen(0.1f,fade));
		}	
		Reset();
	}
	private void SetWhiteColor(){
		/*antes do game over hehe */
		var tempColor = Color.white;
		tempColor.a = 0f;
        imageRef.color = tempColor;
	}
	private void Reset(){
		var tempColor = Color.black;
		tempColor.a = 0f;
        imageRef.color = tempColor;
	}
	private void ShowCursor(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
