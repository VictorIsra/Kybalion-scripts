using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveManager : MonoBehaviour {
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;
	private Vector2 posicaoAlvo;
	private Player player;
	private bool following;
	private float bossWidth;
	private float bossHeight;
	private float timer;
	private float bossSpeed = 12f;
	[SerializeField] float minTempoAntesSeguirPlayer = 2.5f;
	[SerializeField] float maxTempoAntesSeguirPlayer = 4f;
	
	private void Awake() {
		SetReferences();
		DelimitBossMoveArea();
		InitializeValues();
	}
	private void SetReferences(){
		player = FindObjectOfType<Player>();
	}
	private void InitializeValues(){
		timer = minTempoAntesSeguirPlayer;
		posicaoAlvo = new Vector2();
		GenerateRandomTargetPosition();
	}
	private void DelimitBossMoveArea(){
		GetBossBounds();
		minX = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x + bossWidth;
		maxX = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x - bossWidth;
		minY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y + bossHeight;//metade da altura só rs
		maxY = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0)).y - bossHeight;
	}
	private void GetBossBounds(){
		bossWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x/2; //extents = size of width / 2
        bossHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y/2; //extents = size of height / 2
	}
	private void GenerateRandomTargetPosition(){
		float xAlvo = Random.Range(minX, maxX);
		float yAlvo = Random.Range(minY, maxY);
		posicaoAlvo.Set(xAlvo, yAlvo);		
	}
	public void MoveManager(){
		/*sao independentes uma da outra! */
		WaiTimer();
		move();
	}
	 void WaiTimer(){
		/*corotina nao seria viável nesse caso!! ela nunca iria chamar e tal..
		eventos sincronizados é dif de um timer! */
		timer -= Time.deltaTime;
		if(timer <= 0 ){
			following = true;
			timer = Random.Range(minTempoAntesSeguirPlayer,maxTempoAntesSeguirPlayer);
		}
	
	}
	private void move(){
		if(player != null){
			float maxDistanceDelta = bossSpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position,posicaoAlvo, maxDistanceDelta);
			if( transform.position.Equals(posicaoAlvo)){
				if(!following)
					GenerateRandomTargetPosition();
				else{//se estiver seguindo player
					posicaoAlvo.Set(player.transform.position.x , player.transform.position.y);	
					following = false;//reseta pra ele n seguir seguidamente.
				}
			}
		}
	}
}
