using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*todsas as clases que lidam com cascade spawner ( cascadespawner e cascadeenemyspawner) deverao ter uma funcao que dá um trigger pra eles instanciarem */
public interface ICascade  {
	/*trigger */
	void TriggerCascade();
	GameObject GetParentPrefab();
	GameObject GetPathToFollow();
}
