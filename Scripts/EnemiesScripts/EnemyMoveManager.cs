using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour {
	/*objeto que guarda os transform a serem seguidos */
	[SerializeField] GameObject enemyPath;
	[SerializeField] float enemySpeed = 5f;
	[SerializeField] bool rotateOnPlayerDirection = false;
	private Player player;
	private int pointsToVisit;//qts pontos terao no trajeto, EXCLUINDO o ponto de origem
	private Vector2 targetPosition;
	private int currentIndex = 0;
	private Vector2 finalDirection;//direcao apos completar o trajeto definido pelo path
	[SerializeField] private bool moveOnWave = false;//falso para mover individualmente, true pra respeitar o que for definido em EnemyWave.cs
	/* teste */
	[SerializeField] private bool moveOnPathDirection = false;
	/*nao remova, as vezes usa-se esse método, ignore o warning do inspector que nao tá ligado nisso: */
	private bool checkDirectionFlag = false;
	[HideInInspector]
	public bool moving_left = false;
	[HideInInspector]
	public bool moving_right = false;
	float oldXpos;

	void Awake(){
		SetReferences();
		SetInitialPoint();
		SetPointsToVisit();
		finalDirection = sortEndDirection();
		oldXpos = transform.position.x;	
	}
	void Update(){
		/*inimigo irá se mover baseado no comportamente definido no path dessa classe e nao no definido em uma wave */
		Move();
	}
	private void SetReferences(){
		if(rotateOnPlayerDirection){
			player = FindObjectOfType<Player>();
		}
	}
	private void SetInitialPoint(){
		/*seta o targetPostion para posicao inicial, que é a propria posicao onde o inimigo será instanciado */
		targetPosition = transform.position;
	}
	public float GetSpeed(){
		return enemySpeed;
	}
	public void SetCheckDirectionFlag(bool flag){
		checkDirectionFlag = flag;
	}
	private void SetPointsToVisit(){
		if(enemyPath){
			currentIndex = 0;
			pointsToVisit = enemyPath.transform.childCount;
		}
	/* 	else
			Debug.LogWarning("ESQUECEU DE BOTAR UM PATH NO INIMIGO: " + gameObject.name);*/
	}
	public void MoveOnWave(bool moveOnWave){
		this.moveOnWave = moveOnWave;
	}
	private void Move(){
		if(rotateOnPlayerDirection)
			RotateOnPlayerDirection();
		if(!moveOnWave){
			FollowPath();
		}
		else if(moveOnWave){
			/*mudou as variaveis/comportamentos pois EnemyWaveSpawner invocou o método setWaveBehaviour ;) */
			FollowPath();
		}			
	}
	/*se utilizada, o inimigo sempre irá apontar o sprite na direcao do player: */
	private void RotateOnPlayerDirection(){
		if(player){
			Vector2 playerPosition = player.transform.position;
			Vector2 enemyCenter = GetComponent<Collider2D>().bounds.center;
			Vector2 vetorDirecaoProjetil =  ( enemyCenter - playerPosition).normalized * -1;
			transform.rotation = Quaternion.LookRotation(Vector3.forward,-vetorDirecaoProjetil);
		}
	}
	/* EnemyWaveSpawner chama esse método! */
	public void SetWaveBehaviour(EnemyWave wave){
		enemySpeed = wave.GetEnemySpeed();
		enemyPath = wave.GetWavePathPrefab();
		SetPointsToVisit();
		moveOnWave = true;
	}
	/*caso eu queira escolher um path via codigo por outro script e setar aqui:(ex escolher um path aleatorio pra um dragao) */
	public void SetPath(GameObject path){
		enemyPath = path;
		/*setei o path com a variável externa, agora mudo o path */
		SetPointsToVisit();
	}
	private void FollowPath(){
		if(enemyPath){
			/*for n adianta pq o funcao movetoward demora até chegar no alvo, logo um if é bom..um while se pa tb rolaria*/
			if(currentIndex < pointsToVisit){
				if(moveOnPathDirection)
					ChangeEnemyDirection(enemyPath.transform.GetChild(currentIndex).transform);
				
				targetPosition = enemyPath.transform.GetChild(currentIndex).transform.position;
				transform.position = Vector2.MoveTowards(transform.position,targetPosition, Time.deltaTime * enemySpeed);
				if(transform.position.Equals(targetPosition))
					currentIndex++;
			}
			else{//dps de completar o path
				transform.Translate( finalDirection *  enemySpeed * Time.deltaTime);
			}	
		}
		/* else
			Debug.LogWarning("BOTE UM PATH!");*/
	}
	private void ChangeEnemyDirection(Transform targetPoint){
		/* isso é diferente da animacao! aqui é pro sprite flipar em uma dada direcao,
		as animacoes nao necessariamente terao esse comportamento de flipar o sprite, mas só de mudar seu desenho mas manter a direcao do sprite xD */
		 Quaternion rotation = Quaternion.LookRotation(targetPoint.position - transform.position,Vector3.forward);
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);		
	}
	//sortea o vetor que definira a direcao do movimento dps que o inimigo terminar a trajetoria padrao:
	private Vector2 sortEndDirection(){
		int direcao = Random.Range(0, 4);

		switch(direcao){
			case 0:
				return Vector2.up;
			case 1 :
				return Vector2.left;
			case 2:
				return Vector2.down;
			case 3:
				return Vector2.right;
			default:
				//caso padrao:
				return Vector2.down;
		}
	} 
}
