using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*script que lida com objetos que poderia comprar na loja
(bomba,shield armas) mas que posso coletar na fase*/
public class SpecialItems : MonoBehaviour {
	[SerializeField] GameObject weapon;/*só pra caso queira pegar/adicionar uma arma,se nao, será null */
	[SerializeField] GameObject explosionPrefab;
	private UiManagement uiManagement;
	private Player player;
	private bool collected = false;/*só pra n ter duplicatas...pra mim o ontrigger jamais deveria ter esser isco, mas tem  */
	void Awake(){
		SetRefs();
		SetShadow();
	}
	private void SetRefs(){
		uiManagement = FindObjectOfType<UiManagement>();
		player = FindObjectOfType<Player>();
	}
	void OnTriggerEnter2D(Collider2D other){	
		if(other.gameObject.CompareTag(Constantes.PLAYER) && !collected){
			collected = true;
			TryAddItem();
			Destroy(gameObject);
			if(explosionPrefab)
				Instantiate(explosionPrefab,transform.position,Quaternion.identity);		
		}
	}
	private void SetShadow(){
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}	
	/*lembre, uso as tags de bomba shield e arma mas o objeto
	que usa essa classe nao tem/carrega nada disso! só serve
	como indicativo pro metodo que chamo no plauerglobal status
	saber o que adicionar e pra qual lista! */
	private void TryAddItem(){
		if(gameObject.CompareTag(Constantes.BOMB)){
			/*se retornou true é pq n ultrapassou a qtdade limite e adicionou com sucesso */
			if(PlayerGlobalStatus.AddCollectedItem(Constantes.BOMB)){
				uiManagement.UpdateItenIcon(0);/*lembre zero é bomb 1 é shield */
				if(player)
					player.GetComponent<PlayerItensManager>().SetItenIndex(0);
				else
					Debug.LogWarning(Constantes.MISSING_REF);	

			}

		}
		else if(gameObject.CompareTag(Constantes.SHIELD)){
			/*se retornou true é pq n ultrapassou a qtdade limite e adicionou com sucesso */
			if(PlayerGlobalStatus.AddCollectedItem(Constantes.SHIELD)){/*1 é shield lembre */
				uiManagement.UpdateItenIcon(1);
				if(player)
					player.GetComponent<PlayerItensManager>().SetItenIndex(1);
				else
					Debug.LogWarning(Constantes.MISSING_REF);	
			}
		}
		/*pra ter ctz, msm que por engano alguem marque algo que nao devia como weapon, ele checará se tem o script da arma associado...isso garante que só tentara adicionar uma arma de fato */
		else if(gameObject.CompareTag(Constantes.WEAPON) && weapon.GetComponent<Weapon>()){
			if(weapon){
				if(PlayerGlobalStatus.AddCollectedItem(weapon)){
					/*fundamental..se n checar as partes e só adicionar a arma..cracrah tudo! */
				player.GetComponent<PlayerWeaponsPartsManager>().checkplayerUpgradePart(weapon);
				uiManagement.UpdateWeaponIcon();
				}
			}
			else
				Debug.LogWarning(Constantes.MISSING_REF);
		}
		else
			Debug.LogWarning(Constantes.NO_TAG_MATCHED);
	}
}
