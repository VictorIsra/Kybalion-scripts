using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
/* cria um ray que pate do centro da camera e vai até o centro da regiao que ficará o tilemap seguinte,
 . se um ponto colide com esse rayCast, o ponto irá instanciar o tilemap correspondente*/
public class CameraRayCast : MonoBehaviour {

[SerializeField]
	private LayerMask mask;
	private GenerateGrid mapHandlerRef;
	
	void Awake(){
		mapHandlerRef = FindObjectOfType<GenerateGrid>();
	}
	void Update () {
		Ray2D ray = new Ray2D( new Vector3(0,-1f,0), new Vector3(0f, ( 2*Camera.main.orthographicSize) + 1f));
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,2*Camera.main.orthographicSize + 1f,mask);
		/*pra ver visualmente o ray criado: */
		 Debug.DrawRay( new Vector3(0,-1f,0), new Vector3(0f, (2* Camera.main.orthographicSize) + 1f,0f),Color.red, 10f);
		if(hit.collider){
			Debug.LogWarning("VAI GEIRA MAPA " + hit.collider.name);
			mapHandlerRef.GenerateMap(GetIndexOfName(hit.collider.name));
			Debug.LogWarning("colidiu com " +hit.collider.name);
			Destroy(hit.collider.GetComponent<BoxCollider2D>());
		}	
	}		
	private string GetIndexOfName(string name){
		string[] numbers = Regex.Split(name, @"\D+");
		string final = "";/*concatenou os digitos contidos na string em uma unica string ex "02" "1" etc  */
		foreach(string number in numbers)
			final += number;
		return final;
	}
}
