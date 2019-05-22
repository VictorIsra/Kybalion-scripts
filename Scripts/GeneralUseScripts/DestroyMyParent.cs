using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMyParent : MonoBehaviour {

	private void OnBecameInvisible() {
		if(gameObject.transform.parent){
			Destroy(gameObject.transform.parent.gameObject) ;
		}
		else/*esse else nunca deveria acontecer se minha intencao é usar esse script e nao o onbecomeinvisible! */
			Destroy(gameObject);
	}
}
