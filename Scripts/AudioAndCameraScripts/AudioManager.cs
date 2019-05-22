using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
	private static AudioManager instance = null;
	private  AudioClip[] resources;
	private  AudioClip[] musics;
	private  AudioSource audioSource;
	[Range(0f,1.0f)] public float volume = 0.25f;
	/*SINGLETON PATTER, MUITO IMPORTANTE! */
	public static AudioManager getInstance(){
		if(instance == null){
			GameObject go = new GameObject();
			go.name = "AudioManager";
			instance = go.AddComponent<AudioManager>();
			DontDestroyOnLoad(go);
		}
		return instance;
	}
	void Awake(){
		if(instance == null){
			instance = this;
			audioSource = gameObject.AddComponent<AudioSource>();
			resources = Resources.LoadAll<AudioClip>("Audio");	
			musics = Resources.LoadAll<AudioClip>("Music");
			DontDestroyOnLoad(this.gameObject);
		}
		else
			Destroy(gameObject);
	}
	
	public void ChooseTrackToPlay(){
		
		try{
				

				if (SceneManager.GetActiveScene().name == Constantes.LEVEL1) {
					audioSource.loop = true;
					audioSource.Stop();
					audioSource.clip = musics[2];
					audioSource.Play();
				}
				if (SceneManager.GetActiveScene().name == Constantes.LEVEL2){
					audioSource.loop = true;
					audioSource.Stop();
					audioSource.clip = musics[2];
					audioSource.Play();
				}
				if (SceneManager.GetActiveScene().name == Constantes.SHOP_LEVEL){
					audioSource.loop = true;
					audioSource.Stop();
					audioSource.clip = musics[4];
					audioSource.Play();
				}
				if (SceneManager.GetActiveScene().name == Constantes.TRY_AGAIN){
					audioSource.loop = false;
					audioSource.Stop();
					//audioSource.clip = musics[4];
					audioSource.PlayOneShot(musics[6]);
				}
				if (SceneManager.GetActiveScene().name == Constantes.GAME_OVER){
					audioSource.loop = false;
					audioSource.Stop();
					audioSource.clip = musics[1];
					audioSource.Play();
				}
				if (SceneManager.GetActiveScene().name == Constantes.CREDITS_MENU){
					if(audioSource != null){  
						if(audioSource.clip != musics[3]){
							audioSource.loop = true;
							audioSource.Stop();
							audioSource.clip = musics[3];
							audioSource.Play();
						}
					}	  
				}
				if (SceneManager.GetActiveScene().name == Constantes.START_MENU ){
					if(audioSource != null){  
						if(audioSource.clip != musics[3]){
							audioSource.loop = true;
							audioSource.Stop();
							audioSource.clip = musics[3];
							audioSource.Play();
						}
					}	  
				}
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
    }
	public void play_shot_audio(int layer){
		switch(layer){
			case 8:
		AudioSource.PlayClipAtPoint((AudioClip)resources[1],Camera.main.transform.position,volume);
			break;
			case 9:
		AudioSource.PlayClipAtPoint((AudioClip)resources[2],Camera.main.transform.position,volume);
			break;
		}
	}
	public void play_die_audio(){
		AudioSource.PlayClipAtPoint((AudioClip)resources[0],Camera.main.transform.position,volume);
	}
	public void play_itemColect_audio(){
		AudioSource.PlayClipAtPoint((AudioClip)resources[3],Camera.main.transform.position,volume);
	}
	public void CheckVolume(){//Player.cs chama esse método.
			if(AudioListener.volume > 0)
				AudioListener.volume = 0;
			else 
				AudioListener.volume = 1;
	}	
	public void PauseAudio(){
		audioSource.Pause();
	}
	public void ResumeAudio(){
		audioSource.Play();
	}
}
