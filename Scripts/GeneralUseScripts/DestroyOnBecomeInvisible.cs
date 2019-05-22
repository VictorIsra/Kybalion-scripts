using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBecomeInvisible : MonoBehaviour {
	/*O QUE FAZER QUANDO UM INIMIGO/ITEM/PLATAFORMA APARECE OU SOME DE CENA.
	COMO NO GERAL É SEMPRE DESTRUIR, PREFERI TRATAR A EXCEÇÃO NA CLASSES EM QUESTÃO
	( <TerrainMovementManager.cs> ) COMO SÓ TEM ESSA CLASSE NO MEU JOGO QUE É EXCEÇÃO AO
	COMPORTAMENTO PADRÃO, NÃO ACHEI ÚTIL IMPLEMENTAR UMA INTERFACE, MAS SE NO FUTURO ISSO MUDAR,
	SERIA UMA OPÇÃO LEGAL */
	private void OnBecameInvisible() {
		Destroy(gameObject);			
	}
	private void OnBecameVisible() {
	}
}
