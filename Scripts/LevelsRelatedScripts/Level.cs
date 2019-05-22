using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* ESSA CLASSE GERENCIA AS FASES E OS EVENTOS ASSOCIADOS A ELAS */
/*POR ALGUMA RAZAO MSM SETANDO a ref do fasecreenmanager no awake, as vezes
precisava re chamar a funcao pra ele achar...isso corre o risco de um stackoverflow,
pq qd n achava, eu chamava a propria funcao até ele achar a ref. td bem q testei e ele
sempre achava no max na segunda tetnativa..mas, por via das duvidas, vou setar a ref
diretamente antes de chama-la e só */
public class Level : MonoBehaviour {
	//variáveis:
	//private List<string> fases;
	private AudioManager audioManager;
	private float delayBeforeGameOver = 1f;
	private FadeScreenManager fadeScreenManager;

	void Awake(){
		SetScreenFaderRef();
	}
	void Start () {
	//	fases = new List<string>();
		try{
			checkAudioReference();
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
		//loadResources();
	}
	private void SetScreenFaderRef(){
		fadeScreenManager = FindObjectOfType<FadeScreenManager>();
	}
	public void loadNextGameScene(){
		SetScreenFaderRef();
		PlayerGlobalStatus.UpdateSpendedCash();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.NEXT_LEVEL);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void loadNextGameScenePosFade(){
		NextLevelLoader.LoadNextLevel();
	}
	public void loadLevelAgain(){//antes do game over vc vai recarregar a fase q vc tava antes de ir pro menu "tryagain"
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.SAME_LEVEL);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadLevelAgainPosFade(){
		PlayerGlobalStatus.ResetLevelCash();
		SceneManager.LoadScene(NextLevelLoader.GetCurrentLevel());
	}
	public void loadTryAgain(){
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this);
	}
	public void loadTryAgainPosFade(){
		SceneManager.LoadScene(Constantes.TRY_AGAIN);
	}
	public void loadFirstLevel(){		
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.LEVEL1);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadFirstLevelPosFade(){
		SceneManager.LoadScene(Constantes.LEVEL1);
	}
	public void loadGameOver(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,true);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadGameOverPosFade(){
		PlayerGlobalStatus.resetPlayerStatus();
		SceneManager.LoadScene(Constantes.GAME_OVER);
	}
	public void loadStartMenu(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.START_MENU);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadStartMenuPosFade(){
		SetTimeScale();
		PlayerGlobalStatus.resetPlayerStatus();
		NextLevelLoader.ResetLevelsIndex();
		SceneManager.LoadScene(Constantes.START_MENU);
	}
	private void SetTimeScale(){
		Time.timeScale = 1f;
	}
	public void loadStartMenuFromTryAgain(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.START_MENU);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadStarMenuFromTryAgainPosFade(){
		try{
			SetTimeScale();
			PlayerGlobalStatus.resetPlayerStatus();
			NextLevelLoader.ResetLevelsIndex();
			SceneManager.LoadScene(Constantes.START_MENU);
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}	
	}
	public void loadHowToMenu(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.HOW_TO_MENU);
		else
			Debug.LogWarning(Constantes.MISSING_REF);			
	}
	public void LoadHowToMenuPosFade(){
		SceneManager.LoadScene(Constantes.HOW_TO_MENU);
	}
	public void loadHowToMenu2(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.HOW_TO_MENU2);
		else
			Debug.LogWarning(Constantes.MISSING_REF);	
	}
	public void LoadHowToMenu2PosFade(){
		SceneManager.LoadScene(Constantes.HOW_TO_MENU2);
	}
	public void loadCreditsMenu(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.CREDITS_MENU);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadCredtisMenuPosFade(){
		SceneManager.LoadScene(Constantes.CREDITS_MENU);
	}
	public void loadShopMenu(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.SHOP_MENU);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void LoadShopMenuPosFade(){
		/*salva cash acumulado */
		PlayerGlobalStatus.SavePlayerCashAux();
			SceneManager.LoadScene(Constantes.SHOP_MENU);
	}
	public void quit(){
		SetScreenFaderRef();
		if(fadeScreenManager)
			fadeScreenManager.MotherFade(0.1f,this,false,Constantes.QUIT);
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	public void QuitPosFade(){
		Application.Quit();
	}
	/* private void loadResources(){
		Object[] scenes = Resources.LoadAll("Scenes");
	
		foreach (var item in scenes){
			fases.Add(item.name);
		}
	}	*/
	//IMPORTANTE: A COROTINA FOI BOTADA AQUI PQ NO PLAYER ELA NAO RETORNA DO YIELD SE O OBJETO PRINCIPAL
	//Q ELA PERTENCE FOR DESTRUIDO
	public void waitBeforeTryAgain(float delay){//n é o game over total é a tela de menu inciial
		StartCoroutine(delayTryAgain(delay));
	}
	public void waitBeforeLoadShop(float delay){
		StartCoroutine(delayBeforeShop(delay));
	}
	IEnumerator delayBeforeShop(float delay){
		yield return new WaitForSeconds(delay);
		loadShopMenu();
	}			
	IEnumerator delayTryAgain(float delay){
		yield return new WaitForSeconds(delay);
		loadTryAgain();
	}	
	public void triggerGameOverScene(){
		StartCoroutine(triggerGameOverSceneCoroutine(delayBeforeGameOver));
	}
	IEnumerator triggerGameOverSceneCoroutine(float delayBeforeGameOver){
		yield return new WaitForSeconds(delayBeforeGameOver);
		loadGameOver();
	}	
	public void checkAudioReference(){
		audioManager = AudioManager.getInstance();
		audioManager.ChooseTrackToPlay();
	}
	
}
