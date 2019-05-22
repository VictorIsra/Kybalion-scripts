using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*explosao associada a um boss. essa classe linkará o evento da explosao com uma instancia de boss */
public class BossFinalExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*sinaliza o boss do fim da explosao. é um trigger para ele. sempre
	existirá um boss, a nao ser que eu esteja istancia o prefab pra testar coisas, por isso a checagem xD */
	private void SignBoss(){
		if(transform.parent){/*checagens por paranoia...sempre vao ter */
			if(transform.parent.gameObject.GetComponent<Boss>()){
				transform.parent.gameObject.GetComponent<Boss>().DestroyBoss();
			}
		}
		Destroy(gameObject);	
	}
	private void MakeParentInvisible(){
		/*faco o boss sumir, pra ficar maneiro xD */
		if(transform.parent){/*checagens por paranoia...sempre vao ter */
			if(transform.parent.gameObject.GetComponent<Boss>()){
		        transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
}
