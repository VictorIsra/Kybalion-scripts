using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NextLevelLoader{

	/*lembre: essa classe pressupoe que os niveis comecam em 1,2...e o str menu é o zero! 
	n usei cts pois o shop que chamará esse script, e ele é uma cena comum a todos os lvls*/
	private static int nextLevelIndex;/*indice da fase que quero ir ao passar de fas */

	static NextLevelLoader(){
		/*inicializa no lvl 1 */
		nextLevelIndex = 1;
	}
	public static  void LoadNextLevel(){
		IncrementLevel();/*loja ocorre dps do lvl1, logo carregara lvl 2,,,3,,etc */
		Debug.LogWarning("carregará lvl " + nextLevelIndex);
		SceneManager.LoadScene(nextLevelIndex);
		
	}
	public static void IncrementLevel(){
		nextLevelIndex ++;
	}
	public static int GetCurrentLevel(){
		return nextLevelIndex;
	}
	public static void ResetLevelsIndex(){
		nextLevelIndex = 1;
	}
}	
