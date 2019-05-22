using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public static bool gamePause = false;
	private GameObject pauseMenuUI;
	private AudioManager audioManager;
	void Start(){
		FindPauseMenuRef();
		audioManager = AudioManager.getInstance();

	}
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(gamePause)
				Resume();
			else
				Pause();	
		}		
	}
	private void FindPauseMenuRef(){
		foreach(Transform childs in transform){
			if(childs.CompareTag(Constantes.PAUSE_MENU)){
				pauseMenuUI = childs.gameObject;
				return;
			}
		}
	}
	public void Resume(){
		if(pauseMenuUI){
			if(audioManager)
				audioManager.ResumeAudio();
			pauseMenuUI.SetActive(false);
			Time.timeScale = 1f;
			gamePause = false;
			MouseVisibility.HideCursor();
		}
	}
	private void Pause(){
		if(pauseMenuUI){
			if(audioManager)
				audioManager.PauseAudio();
			pauseMenuUI.SetActive(true);
			Time.timeScale = 0f;
			gamePause = true;
			MouseVisibility.ShowCursor();
		}		
	}
}
