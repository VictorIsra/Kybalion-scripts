using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class RobotManager : MonoBehaviour {
	[SerializeField] GameObject explosionPrefab;
	private float lifeTime = 10f;
	private bool moveTowardPlayer = false;
	private float speed = 10f;
	private Player player;
	private Animator animator;
	private bool animationStarted = false;
	private bool canFire = false;
	private RobotFireManager robotFireManager;
	private int myID;/*pra saber se existem outros robos*/
	private Vector2 target;
	private string seekPoint;/*pode ser A OU b DEPENDENDO se já tem algum robo sendo usado */
	private int As = 0;
	private int Bs = 0;
	private int Cs = 0;

	private void Awake() {
		SetReferences();	
		InitializeAttributes();
		SetShadow();
	}
	void FixedUpdate(){
		if(moveTowardPlayer)
			MoveTowardPlayer();
		if(canFire && player){
			robotFireManager.Fire();
		}
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag(Constantes.PLAYER)){
			player = other.gameObject.GetComponent<Player>();
			moveTowardPlayer = true;
			if(seekPoint == Constantes.NONE)
				CheckOthersSeekPoint();
		}
	}
	private void InitializeAttributes(){
		myID = gameObject.GetInstanceID();
		seekPoint = Constantes.NONE;
	}
	private void SetShadow(){
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}
	private void ResetSortLayer(){
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constantes.ROBOTS;
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		/*auxiliar as chamadas acima..garante que as animacoes vao ficar bonitas */
		if(gameObject.transform.parent)
			gameObject.transform.SetParent(null);
	}
	private void CheckOthersSeekPoint(){
		RobotManager[] robots = FindObjectsOfType<RobotManager>();
		foreach(RobotManager robot in robots){
			if(robot){
				if(robot.gameObject.GetInstanceID() != myID){
						if(robot.GetMySeekPoint() == Constantes.ROBOT_SEEK_POINT_A)
							As++;
						if(robot.GetMySeekPoint() == Constantes.ROBOT_SEEK_POINT_B)
							Bs++;
						if(robot.GetMySeekPoint() == Constantes.ROBOT_SEEK_POINT_C)
							Cs++;
				}	
			}	
		}
		CheckPositions();			
	}
	private void CheckPositions(){
		if(As == 0 && Bs == 0 & Cs == 0)
			seekPoint = Constantes.ROBOT_SEEK_POINT_A;
		else if (As == 1 && Bs == 0 && Cs == 0)
			seekPoint = Constantes.ROBOT_SEEK_POINT_B;
		else if ( As == 1 && Bs == 1 && Cs == 0)
			seekPoint = Constantes.ROBOT_SEEK_POINT_C;
		else if( As == 1 && Bs == 1 && Cs == 1)
			Suicide();
		As = 0;/* reseta pra sempre ser valido as cehcagens */
		Bs = 0;
		Cs = 0;			
		/*container muda as layers pra ficarem atrás da explosao,
			porem dps é necessário restaurar as layers originais, pra ficar legal */	
		if(seekPoint != Constantes.NONE)
			ResetSortLayer();
	
	}
	public string GetMySeekPoint(){
		return seekPoint;
	}
	
	private Transform FindChildWithTag(string tag){
		foreach(Transform child in player.transform){
			if(child.CompareTag(tag))
				return child;
		}
		return null;
	}
	public float GetSpeed(){
		return speed;
	}
	private void SetReferences(){
		animator = GetComponent<Animator>();
		robotFireManager = GetComponent<RobotFireManager>();
	}
	private void MoveTowardPlayer(){
		if(player && seekPoint != Constantes.NONE){
			float maxDistanceDelta = speed * Time.deltaTime;
			target = FindChildWithTag(seekPoint).position;
			transform.position = Vector2.MoveTowards(transform.position,target, maxDistanceDelta);
			HandleAnimation();
		}
	}
	private void HandleAnimation(){
		if(!animationStarted){
			animationStarted = true;
			RunNextAnimation();
		}
	}
	private void RunNextAnimation(){
		if(animator){
			animator.SetBool(Constantes.AWAKE, true);
		}	
	}
	/*se nao a animacao de idle( que pertence ao container) irá ficar aparecendo e sobrepondo a morphing */
	private void DisableChildSprite(){
		GameObject child = gameObject.transform.Find(Constantes.ROBOT_TRANSFORM_CONTAINER).gameObject;
		child.GetComponent<SpriteRenderer>().enabled = false;
	}
	/*animacao envocará isso: */
	private void SetFire(){
		canFire = true;
		DisableChildSprite();
		StartCoroutine(SelfDestruct(lifeTime));
	}
	public void Suicide(){
		InstantiateEffect();
		Destroy(gameObject);
	}
	IEnumerator SelfDestruct(float delay){
		yield return new WaitForSeconds(delay);
		seekPoint = Constantes.NONE;
		animator.SetTrigger(Constantes.UNMORPH);
	}
	private void InstantiateEffect(){
		Instantiate(explosionPrefab,transform.position,Quaternion.identity);
	}
}
