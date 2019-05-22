using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNoChilds : MonoBehaviour {

	void OnBecameInvisible(){
		if(transform.childCount == 0)
			Destroy(gameObject);
	}
}
