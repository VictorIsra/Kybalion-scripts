using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {
	[SerializeField] private float health = 150f;
	private Player player;

	void Awake(){
		player = FindObjectOfType<Player>();
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag(Constantes.PLAYER)){
			AddLife();
			Destroy(gameObject);
		}	
	}
	private void AddLife(){
		if(player)
			player.GetComponent<PlayerHealthManager>().AddHealth(health);
	}
	
}
