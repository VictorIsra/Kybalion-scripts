using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*move o grid que contém os tilemaps.
esse movimento fara pontos eventualmente
colidirem com o raycast, instanciando assim um tilemap dinamicamente e de forma leve/eficiente */
public class MoveGrid: MonoBehaviour {
	private bool move = true;
	
	public bool GetStatus(){
		return move;
	}
	public void SetStatus(bool move){
		this.move = move;
	}
	// Update is called once per frame
	private void FixedUpdate() {	 
		if(move)
			transform.Translate(0,-0.05f,0);		
	}
}
