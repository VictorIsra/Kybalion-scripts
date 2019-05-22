using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterChildsDestroyed : MonoBehaviour {
	private bool checkChilds = false;
	/*me destruo qd meus filgos n existirem mais, assim os filhos n some subtamente da
	tela qd eu desaparecer!porisso usso esse script em vez de destroyOnBecomeInvisible
	lembre que nao adiciono a um prefab esse script, mas sim via script na hora de instancia-lo,
	assim, meus prefabs ficam amsi flexiveis xD */
	void OnBecameVisible(){
		checkChilds = true;

	}
	private void Update() {
		if(checkChilds)
			CheckChilds();
	}
	private void CheckChilds(){
		if(transform.childCount == 0)
			Destroy(gameObject);
	}
}
