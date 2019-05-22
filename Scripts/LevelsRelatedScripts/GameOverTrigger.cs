using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//codigo comum pra tela inicial do jogo e apos o game over

public class GameOverTrigger : MonoBehaviour {
	private float delay = 7f;
	// Use this for initialization
	void Start () {
		StartCoroutine(loadStartMenu(delay));
	}	
	IEnumerator loadStartMenu(float delay){
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(Constantes.START_MENU);
		NextLevelLoader.ResetLevelsIndex();
	}	
}
