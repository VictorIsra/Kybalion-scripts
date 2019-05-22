using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerFireManager : MonoBehaviour {
 	//ammos:
	private int numberOfAmmosOnCurrentWeapon;
	private List<bool> ammosFireFlags = new List<bool>();//cada municao terá sua unica flag de poder atirar
    private int counter = 0;
	private GameObject currentAmmo;
	private Player player;

	private void Awake() {
		SetPlayerReference();
	}
	private void SetPlayerReference(){
		player = gameObject.GetComponent<Player>();
	}
    /* playerWeaponsPartsManager que chama esse método! 
    necessário pra manter o sincronismo e tudo ocorrer bem */
	public void ManageAmmoVector(){
		numberOfAmmosOnCurrentWeapon = PlayerGlobalStatus.getWeaponAmmos().Count;
        SetFlags();
	}
	private void SetFlags(){
		for(int i = 0; i < numberOfAmmosOnCurrentWeapon; i++)
			ammosFireFlags.Add(true);
	}
	public void Fire(){
		foreach(Ammo ammo in PlayerGlobalStatus.getWeaponAmmos()){
			if( ammosFireFlags[counter] && ammo.GetAmmoStatus()){
				StartCoroutine(PlayerFireCoroutine(ammo, counter));
			}	
		    counter++;	
		}
		ResetCounter();
	}
	private void ResetCounter(){
		counter = 0;
	}
	IEnumerator PlayerFireCoroutine(Ammo currentAmmo, int currenFireFlagIndex){
		int currentFlagIndex = currenFireFlagIndex;
        try{
		    PlayerFire(currentAmmo);//origensTiro);	
			//audioManager.play_shot_audio(8);
			ammosFireFlags[currentFlagIndex] = false;
		}
		catch(System.Exception e){
				Debug.LogWarning(e);
		}
        
		yield return new WaitForSeconds(currentAmmo.GetFireDelay());
         
		ammosFireFlags[currentFlagIndex] = true;
	}
	public void PlayerFire(Ammo currentAmmo){
	    foreach(Transform weaponFireSpawPoint in player.findChildByTag(PlayerGlobalStatus.getCurrentWeaponUpgradeTag())){ 
            try{
                if(IsValidAmmoFireSpawnPoint(currentAmmo, weaponFireSpawPoint)){
                    if(PlayerGlobalStatus.isAmmoDiretionalFire(currentAmmo))
                        diretionalFire(currentAmmo, weaponFireSpawPoint);
                    else if(PlayerGlobalStatus.isAmmoTrackerFire(currentAmmo))
                        fireDirectionalTracker(currentAmmo, weaponFireSpawPoint);//diretionalFire(currentAmmo, WeaponFireSpawPoint);
                    else
                        linearFire(currentAmmo, weaponFireSpawPoint);   
                }     
            }catch(System.Exception e){
				Debug.LogWarning(e);
			}
		}
	}
	/* para que cada municao seja atirada apenas no ponto de origem desejado,
    cada ponto de origem DO OBJETO UPGRADEPART tera filhos indicando
    quais das municoes A,B OU C OU A,B E C poderão ser disparadas dela ;D */
    private bool IsValidAmmoFireSpawnPoint(Ammo currentAmmo, Transform WeaponFireSpawPoint){
        bool validSpawnPoint = false;
        foreach( Transform fireSpawPointChild in WeaponFireSpawPoint ){
            if(fireSpawPointChild.CompareTag(currentAmmo.GetAmmoChildTag())){
                 validSpawnPoint = true;
                return  validSpawnPoint;
            }    
        }
        return  validSpawnPoint;
    }
     /* poderia usar um angulo, senos e cossenos ( como no caso de Enemy.cs)
     mas como o player trabalha com pontos de origens do tiro nas partes upgradiáveis,
     é mais simples simplesmente apontar na direcao do ponto de origem. caso o ponto de origem
     seja no centro do player, o comportamento direcional se limitará a algo linear */
     private void diretionalFire(Ammo ammo, Transform WeaponShootSpawPoint){
         
		Vector2 playerCenter = player.GetComponent<Collider2D>().bounds.center;
		Vector2 vetorPosicaoOrigemTiro = new Vector2 (WeaponShootSpawPoint.position.x, WeaponShootSpawPoint.position.y);
        /*esse vetor que determinara a direcao/angulacao de origem! mas n confunda com a angulacao do sprite! */
        Vector2 vetorDirecaoProjetil;
        /*caso de lasers, ja que ficam colados ao player */
        if(ammo.ApendPositionToPlayer()){
            vetorDirecaoProjetil =  (playerCenter - vetorPosicaoOrigemTiro ).normalized * Vector2.zero;
            currentAmmo = Instantiate(ammo.getAmmoPrefab(),playerCenter, Quaternion.identity);
            currentAmmo.transform.parent = player.transform;
        }
        /* caso municoes comuns */
        else{
            vetorDirecaoProjetil =  (playerCenter - vetorPosicaoOrigemTiro ).normalized * - ammo.getAmmoShotSpeed();
            currentAmmo = Instantiate(ammo.getAmmoPrefab(),vetorPosicaoOrigemTiro, Quaternion.identity);
            currentAmmo.GetComponent<AmmoHandler>().SetAmmoSpeed(ammo.getAmmoShotSpeed());

        }
        currentAmmo.GetComponent<Rigidbody2D>().velocity = RadialAjust(vetorDirecaoProjetil);
        /* rotaciona a municao em si: */
        currentAmmo.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
    }
    private Vector2 RadialAjust(Vector2 vetorDiretor){
        /*unico jeito que encontrei de nao ficar escroto ao andar pra frente
        tentei refletixoes e outras coisas mas só isso que funcionou...nao
        é ideal porque altera minimamente a velocidade do tiro, mas tá valendo */
      if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0 ){
          vetorDiretor.y += PlayerMoveManager.playerSpeed/2;
          vetorDiretor.x += PlayerMoveManager.playerSpeed;
          return vetorDiretor;
      }
       if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0 ){
          vetorDiretor.y += PlayerMoveManager.playerSpeed/2;
          vetorDiretor.x -= PlayerMoveManager.playerSpeed;
          return vetorDiretor;
      }
      if(Input.GetAxis("Vertical") > 0 ){
          vetorDiretor.y += PlayerMoveManager.playerSpeed/2;
          return vetorDiretor;
      }
      if(Input.GetAxis("Horizontal") > 0 ){
          vetorDiretor.x += PlayerMoveManager.playerSpeed;
      }
      if(Input.GetAxis("Horizontal") < 0){
            vetorDiretor.x -= PlayerMoveManager.playerSpeed;
      }
        return vetorDiretor;
    }
    private void linearFire(Ammo ammo, Transform WeaponShootSpawPoint){
        if(ammo.ApendPositionToPlayer()){
            currentAmmo = Instantiate(ammo.getAmmoPrefab(),WeaponShootSpawPoint.transform.position, Quaternion.identity) as GameObject;
            currentAmmo.transform.parent = player.transform;
        }
        else{
            Vector2 vetorDir = WeaponShootSpawPoint.transform.position;
            currentAmmo = Instantiate(ammo.getAmmoPrefab(),WeaponShootSpawPoint.transform.position, Quaternion.identity) as GameObject;
            currentAmmo.GetComponent<Rigidbody2D>().velocity = new Vector2(0,ammo.getAmmoShotSpeed());
            currentAmmo.GetComponent<AmmoHandler>().SetAmmoSpeed(ammo.getAmmoShotSpeed());
        }
    } 
    public void fireDirectionalTracker(Ammo ammo,Transform weaponShootSpawPoint){
		GameObject sortedEnemy = SortEnemy();
		if(sortedEnemy){

			Vector2 enemyPosition = sortedEnemy.transform.position;
			Vector2 playerCenter = GetComponent<Collider2D>().bounds.center;
			Vector2 vetorDirecaoProjetil =  ( playerCenter - enemyPosition).normalized * -1;
			
			if(IsValidAmmoFireSpawnPoint(ammo,  weaponShootSpawPoint)){
			    GameObject currentAmmo = Instantiate(ammo.getAmmoPrefab(), weaponShootSpawPoint.position, Quaternion.identity);
				currentAmmo.GetComponent<Rigidbody2D>().velocity =  vetorDirecaoProjetil * ammo.getAmmoShotSpeed();
                currentAmmo.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
                currentAmmo.GetComponent<AmmoHandler>().SetAmmoSpeed(ammo.getAmmoShotSpeed());

            }
		}
	}  
    private GameObject SortEnemy(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Constantes.ENEMY);
        if(enemies.Length > 0){
            int ChosenEnemyIndex = Random.Range(0, enemies.Length);
            return enemies[ChosenEnemyIndex];
        }     
        else
            return null;    
    }
}
