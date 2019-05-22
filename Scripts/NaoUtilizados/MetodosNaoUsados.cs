using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ESSA CLASSE NÃO SERVE PARA NADA NO JOGO, PORÉM GUARDEI AQUI MÉTODOS QUE CRIEI
PARA SOLUCIONAR UM PROBLEMA, MAS QUE DEPOIS NÃO PRECISEI UTILIZA-LOS POR
ACHAR UMA SOLUÇÃO MAIS EFICIENTE/PRÁTICA: */
public class MetodosNaoUsados : MonoBehaviour {

//método que converte grau pra radianos. Legal, porém vi que o unity já tem um método interno para isso!
	private float convertDegreeToRad(float degree){
		return (Mathf.PI * degree) / 180;
	}
	private string findRelativePosition(Vector2 playerPosition, Vector2 enemyCenter){
		/*Baseado na posicao do ciclo trigonométrico, defina a posicao do player relativa a nave */
		/*méotdo inverseTransformPoint é muito bom, pois transforma uma posicao em outra relativa ao objeto que a invoca */
		Vector2 enemyCenterRelativeToEnemy = transform.InverseTransformPoint(enemyCenter);
		Vector2 playerPostionRelativeToEnemy = transform.InverseTransformPoint(playerPosition);

		if(Mathf.Cos(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) > 0
		&& Mathf.Sin(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) > 0 ){
			
			return Constantes.BOTTON_LEFT;
		}	
		else if(Mathf.Cos(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) < 0
		&& Mathf.Sin(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) > 0 ){
			
			return Constantes.UPPER_LEFT;
		}	
		else if(Mathf.Cos(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) < 0
		&& Mathf.Sin(convertDegreeToRad(Vector2.SignedAngle(playerPostionRelativeToEnemy, enemyCenterRelativeToEnemy))) < 0 ){
	
			return Constantes.UPPER_RIGHT;	
		}	
		else 
			return Constantes.BOTTON_RIGHT;	

	}
}
