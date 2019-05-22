using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobotManager))]

public class RobotFireManager : MonoBehaviour {
[SerializeField] RobotAmmoInfos robotAmmoInfos;
[SerializeField] int trackerProjectilesNumber = 5;
private bool canFire = true;

//private RobotManager robotManager;

	private void Awake() {
		SetReferences();
	}
	private void SetReferences(){
		//robotManager = GetComponent<RobotManager>();
		if(robotAmmoInfos.ammo)
			robotAmmoInfos.ammo.SetFirePower();
	}
	public void Fire(){
		if(canFire)
			StartCoroutine(FireCoroutine());
	}
	IEnumerator FireCoroutine(){
		canFire = false;
		if(robotAmmoInfos.ammo)
			SelectFireType(robotAmmoInfos.ammo);
		else
			Debug.LogWarning(Constantes.MISSING_REF);	
		yield return new WaitForSeconds(robotAmmoInfos.fireDelay);
		canFire = true;
	}
	private void SelectFireType(Ammo currentAmmo){
		if(robotAmmoInfos.circularFire)
			fireCircular(currentAmmo);
		else if(robotAmmoInfos.trackerFire)
			fireDirectionalTracker(currentAmmo);
		else
			Debug.LogWarning("NENHUM TIPO DE TIRO FOI SETADO...");		
	}
	public void fireDirectionalTracker(Ammo ammo){
		GameObject sortedEnemy = SortEnemy();
		if(sortedEnemy){
			RotateOnEnemyDirection(sortedEnemy);
			foreach(Transform fireSpawnPoint in transform){
				float x = 1f;
				Vector2 enemyPosition = sortedEnemy.transform.position;
				Vector2 playerCenter = GetComponent<Collider2D>().bounds.center;
				Vector2 vetorDirecaoProjetil =  ( playerCenter - enemyPosition).normalized * -1;
				for(int i = 0; i < trackerProjectilesNumber; i ++){
					Vector2 v = new Vector2(fireSpawnPoint.position.x + x, fireSpawnPoint.position.y + x);
					GameObject currentAmmo = Instantiate(ammo.getAmmoPrefab(), v, Quaternion.identity);
					currentAmmo.GetComponent<Rigidbody2D>().velocity =  vetorDirecaoProjetil * ammo.getAmmoShotSpeed() *2.5f;
            		currentAmmo.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
					x +=0.3f;
				}
			}
		}
		else{
			foreach(Transform fireSpawnPoint in transform){
				transform.rotation = Quaternion.LookRotation(Vector3.forward);
				GameObject currentAmmo = Instantiate(ammo.getAmmoPrefab(), fireSpawnPoint.position, Quaternion.identity);
				currentAmmo.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ammo.getAmmoShotSpeed() *2.5f);
			}
		}	
	}  
	private void RotateOnEnemyDirection(GameObject sortedEnemy){
		if(sortedEnemy){
			Vector2 playerPosition = sortedEnemy.transform.position;
			Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;
			Vector2 vetorDirecaoProjetil =  ( enemyCenter - playerPosition).normalized * -1;
			transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
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
	public void fireCircular(Ammo ammo, int numberSpawPoints = 8, float raio = 1f){
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
			//if( (angulo % 180) != 0){
				GameObject projetil = Instantiate(ammo.getAmmoPrefab(), new Vector2(x0,y0),Quaternion.identity);
				projetil.GetComponent<Rigidbody2D>().velocity =  vetorDirecaoProjetil;
				projetil.transform.rotation = Quaternion.LookRotation(Vector3.forward,vetorDirecaoProjetil);
			//}
			angulo += pedacos;	
		}
	}
	
}
