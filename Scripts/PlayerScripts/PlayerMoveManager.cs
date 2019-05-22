using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour{//}, ISetLimitsInterface {
	
	[SerializeField] public static float playerSpeed = 10f; 
	private Vector2 player_position; //irá determinar a posicao do Player
	private float deltaX;
	private float deltaY;
	private float newXpos;
	private float newYpos;
	private float objectWidth;
	private float objectHeight;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	private void Awake() {
		SetReferences();
		SetPlayerBounds();
	}
	private void SetReferences(){
		try{
			player_position = new Vector2();
		}
		catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
	private void SetPlayerBounds(){
		objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x/2; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y/2; //extents = size of height / 2
		
		minX =  Camera.main.GetComponent<CameraHandler>().cameraBounds.x * -1 + objectWidth;
		maxX = 	Camera.main.GetComponent<CameraHandler>().cameraBounds.x- objectWidth;
		
		minY = Camera.main.GetComponent<CameraHandler>().cameraBounds.y * -1 + objectHeight;
		maxY = Camera.main.GetComponent<CameraHandler>().cameraBounds.y - objectHeight;
	}
	public void move(){
		
		deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
		newXpos = Mathf.Clamp( transform.position.x + deltaX, minX, maxX ); 
		
		deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
		newYpos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);
		
		player_position.Set(newXpos, newYpos);
		transform.position =  player_position;

	}
	public void FlyAway(){
		transform.Translate( Vector2.up * playerSpeed * Time.deltaTime);
	}	
}
