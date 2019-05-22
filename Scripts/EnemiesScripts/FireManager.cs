using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* use isso pro vilao rodar na direcao do player:
	transform.rotation = Quaternion.LookRotation(Vector3.forward,-vetorDirecaoProjetil);
 */
public class FireManager : MonoBehaviour {
	/* lembre o tamanho de ammoInfoContainer deve bater com o ammosFireFlag..atencao no editor..dps vou tratar isso via codigo xD */
	private Player player;
	private AudioManager audioManager;
	[SerializeField] private List<Ammo> ammos;
	[SerializeField] private AmmoInfosContainer[] ammoInfoContainer = new AmmoInfosContainer[3];
	private List<bool> ammosFireFlags = new List<bool>();//cada municao terá sua unica flag de poder atirar
    private int fireFlagcounter = 0;
	private EnemyMoveManager enemyMoveManager;

	private void Awake() {
		SetFlags();
		SetAmmosFirePower();
		setReferencesAndInitializeValues();	
	}
	/* caso eu queira adicionar uma municao pra ser usada apenas dps ( no chefao isso vai ser legal po ex) */
	private void RefreshInfo(){
		SetFlags();
		SetAmmosFirePower();
	}
	private void setReferencesAndInitializeValues(){
		try{
			audioManager = AudioManager.getInstance();
			player = FindObjectOfType<Player>();
			enemyMoveManager = GetComponent<EnemyMoveManager>();
		}catch(System.Exception e){
			Debug.LogWarning("IREI RETORNAR, POIS UMA EXCEÇÃO FOI ENCONTRADA: " + e);
			return;
		}	
	}
	private void EnemyMoveRefHandler(){
		/*lembre, nem sempre vai ter...inimigos do solo parado não terao por exemplo */
		if(enemyMoveManager)
			enemyMoveManager.SetCheckDirectionFlag(true);
	}
	private void SetAmmosFirePower(){
		foreach(Ammo ammo in ammos)
			ammo.SetFirePower();
	}
	private void SetFlags(){
		for(int i = 0; i < ammos.Count; i++)
			ammosFireFlags.Add(true);
	}
	public void Fire(){
        
		foreach(Ammo ammo in ammos){
			if( ammosFireFlags[fireFlagcounter]){
				StartCoroutine(PlayerFireCoroutine(ammo,fireFlagcounter));
			}	
		fireFlagcounter++;	
		}
		ResetFireFlagCounter();
	}
	private void ResetFireFlagCounter(){
		fireFlagcounter = 0;
	}
	IEnumerator PlayerFireCoroutine(Ammo currentAmmo, int currentFireFlagIndex){
		int currentFlagIndex = currentFireFlagIndex;
        try{
			/*pra garantir que a arma na struct baterá em ordem com a
			arma em questao, passo o indice do vetor de booleanos, pois este
			implicitamente define qual municao está sendo usada ( primeira, segunda etc...) */
			SelectFireType(currentAmmo, currentFireFlagIndex);	
			//audioManager.play_shot_audio(8);
			ammosFireFlags[currentFlagIndex] = false;
		}
		catch(System.Exception e){
				Debug.LogWarning(e);
		}
		if(WaitWaveDelay(currentFlagIndex))//esperará um tempo especificado no container
			yield return new WaitForSeconds(ammoInfoContainer[currentFlagIndex].fireWaveDelay);
		else//esperará o delay da municao normalmente
			yield return new WaitForSeconds(currentAmmo.GetFireDelay());
		ammosFireFlags[currentFlagIndex] = true;
	}
	public bool WaitWaveDelay(int currentFlagIndex){
		if(ammoInfoContainer[currentFlagIndex].fireCounter >= ammoInfoContainer[currentFlagIndex].maxFireNumberBeforeNewFireWaveDelay){
			ammoInfoContainer[currentFlagIndex].fireCounter = 0;
			return true;//esperará o tempo especificado no container
		}
		else
			return false;//esperará o próprio delay da ammo 
	}
	private void SelectFireType(Ammo currentAmmo, int currentFireFlagIndex){
		if(ammoInfoContainer[currentFireFlagIndex].defaultFire)
			fireDefault(currentAmmo, currentFireFlagIndex);
		else if(ammoInfoContainer[currentFireFlagIndex].circularFire)
			fireCircular(currentAmmo, currentFireFlagIndex);
		else if(ammoInfoContainer[currentFireFlagIndex].directionalFire)
			fireDirectionalTracker(currentAmmo, currentFireFlagIndex);
		else if(ammoInfoContainer[currentFireFlagIndex].radialFire)
			fireRadial(currentAmmo, currentFireFlagIndex);
	//	else
	//		Debug.LogWarning("NENHUM TIPO DE TIRO FOI SETADO...");		
	}
	private bool IsValidAmmoFireSpawnPoint(Ammo currentAmmo, Transform enemyFireSpawPoint){
        bool validSpawnPoint = false;
       	if(enemyFireSpawPoint.CompareTag(currentAmmo.GetAmmoChildTag())){
        	validSpawnPoint = true;
            return  validSpawnPoint;
        }    
		else
        	return  validSpawnPoint;
    }
	public void fireDefault(Ammo ammo, int currentFireFlag){
		try{
			foreach(Transform enemyChilds in  transform){
				if(IsValidAmmoFireSpawnPoint(ammo, enemyChilds)){
					GameObject tiro = Instantiate(ammo.getAmmoPrefab(),enemyChilds.transform.position, Quaternion.identity) as GameObject;
					tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-ammo.getAmmoShotSpeed());
					audioManager.play_shot_audio(9);
					tiro.transform.Rotate(0, 0, Constantes.MIRROR_ANGLE);

					ammoInfoContainer[currentFireFlag].fireCounter ++;
				}
			}
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}	
	}
	public void fireDirectionalTracker(Ammo ammo, int currentFireFlag){
		
		if(player){
			
			Vector2 playerPosition = player.transform.position;
			Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;
			Vector2 vetorDirecaoProjetil =  ( enemyCenter - playerPosition).normalized * -1;

			foreach(Transform fireSpawnPoint in transform){
				if(IsValidAmmoFireSpawnPoint(ammo, fireSpawnPoint)){
					GameObject currentAmmo = Instantiate(ammo.getAmmoPrefab(),fireSpawnPoint.position, Quaternion.identity);
					currentAmmo.GetComponent<Rigidbody2D>().velocity =  vetorDirecaoProjetil * ammo.getAmmoShotSpeed();
					currentAmmo.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
					ammoInfoContainer[currentFireFlag].fireCounter ++;
				}
			}
		}
	}
	public void fireCircular(Ammo ammo,int currentFireFlag, int numberSpawPoints = 8, float raio = 1f){
		Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;
		float enemyX0 = enemyCenter.x;
		float enemyY0 = enemyCenter.y;
		float pedacos = 360f / numberSpawPoints; 
		float angulo = 0f;

		for(int i = 0; i < numberSpawPoints; i++){
				//angulo * PI / 180 é pra converter de graus pra radianos
			float x0 = enemyX0 + Mathf.Cos( (angulo * Mathf.PI)/ 180) * raio;
			float y0 = enemyY0 + Mathf.Sin( (angulo * Mathf.PI)/ 180) * raio;
			
			Vector2 vetorProjetil = new Vector2 (x0, y0);//p poder controlar a direcao dele
			Vector2 vetorDirecaoProjetil =  (enemyCenter - vetorProjetil ).normalized * -ammo.getAmmoShotSpeed();
			
			GameObject projetil = Instantiate(ammo.getAmmoPrefab(), new Vector2(x0,y0),Quaternion.identity);
			projetil.GetComponent<Rigidbody2D>().velocity =  vetorDirecaoProjetil;
			projetil.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);

			ammoInfoContainer[currentFireFlag].fireCounter ++;
			angulo += pedacos;	
		}
	}
	public void fireRadial(Ammo ammo,int currentFireFlag){
		
		foreach(Transform fireSpawnPoint in transform){
			if(IsValidAmmoFireSpawnPoint(ammo, fireSpawnPoint)){
				Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;
				Vector2 vetorPosicaoOrigemTiro = new Vector2 (fireSpawnPoint.position.x,fireSpawnPoint.position.y);
				Vector2 vetorDirecaoProjetil =  (enemyCenter - vetorPosicaoOrigemTiro ).normalized * - ammo.getAmmoShotSpeed();
			
				GameObject currentAmmo = Instantiate(ammo.getAmmoPrefab(),vetorPosicaoOrigemTiro, Quaternion.identity);
				currentAmmo.GetComponent<Rigidbody2D>().velocity =  RadialAjust(vetorDirecaoProjetil);
				currentAmmo.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
				ammoInfoContainer[currentFireFlag].fireCounter ++;
			}
		}
	}
	private Vector2 RadialAjust(Vector2 vetorDiretor){
		if(enemyMoveManager){
			vetorDiretor.y *= enemyMoveManager.GetSpeed()/2;//*= Constantes.ENEMY_RADIAL_AJUST;
			
			if(enemyMoveManager.moving_right ){
				//vetorDiretor.y += enemyMoveManager.GetSpeed()/2;
				vetorDiretor.x += enemyMoveManager.GetSpeed();
				return vetorDiretor;
			}
			else if(enemyMoveManager.moving_left){
			// vetorDiretor.y += enemyMoveManager.GetSpeed()/2;
			vetorDiretor.x -= enemyMoveManager.GetSpeed();
			return vetorDiretor;
			}
			return vetorDiretor;
		}	
		return vetorDiretor;
	}   
}
