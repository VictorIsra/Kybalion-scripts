using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* essa classe gerenciará o uso das bombas e escudos pelo player  */
public class PlayerItensManager : MonoBehaviour {
	
	public static int currentItenIndex = 0; //zero é bomba, 1 é sheild
	private UiManagement uiManagement;
	private bool canUseShieldAgain = true;

	void Awake(){
		SetReference();
	}
	public void UseItem(){

		if(currentItenIndex == 0 ){
			if(PlayerGlobalStatus.IsThereBomb()){
				UseBomb();
			}
		}
		else if(currentItenIndex == 1){
			if(PlayerGlobalStatus.IsThereShield() && canUseShieldAgain){
				UseShield();
			}
		}	
	}
	private void UseBomb(){
		GameObject bomb = Instantiate(PlayerGlobalStatus.GetBomb(),transform.position,Quaternion.identity);
		bomb.GetComponent<Rigidbody2D>().velocity = Vector2.up * 5f;
		bomb.GetComponent<Bomb>().TriggerExplosionCoroutine();
		PlayerGlobalStatus.RemoveBomb();
		uiManagement.UpdateItenIcon(currentItenIndex);
	}
	private void UseShield(){
		canUseShieldAgain = false;
		GameObject shieldAura = Instantiate(PlayerGlobalStatus.GetShieldAura(),transform.position,Quaternion.identity);
		StartCoroutine(ShieldAuraDelay());
		shieldAura.GetComponent<ShieldAura>().StartAuraProtection(gameObject.GetComponent<Player>());
		shieldAura.transform.SetParent(GetComponent<Player>().transform);
		PlayerGlobalStatus.RemoveShield();
		uiManagement.UpdateItenIcon(currentItenIndex);
	}
	IEnumerator ShieldAuraDelay(){
		yield return new WaitForSeconds(PlayerGlobalStatus.GetShieldAuraCurrentDuration());
		canUseShieldAgain = true;
	}
	public void ChangeItem(){
		if(uiManagement){
			uiManagement.UpdateItenIcon(ChangeItenIndex());
			
		}
		else{
			SetReference();
			ChangeItem();
		}
	}
	private void SetReference(){
		uiManagement = FindObjectOfType<UiManagement>();
	}
	public void SetItenIndex(int index){
		/*pois qd pego do mapa, ele muda o icone e tem que mudar o indice tb!
		se nao fica indice de um escudo enqt o vetor ta na bomba por ex */
		currentItenIndex = index;
	}
	private int ChangeItenIndex(){
		currentItenIndex ++;
		/* mod pra ficar sempre oscilando entre 0 e 1 */
		currentItenIndex = currentItenIndex % 2;
	
		return currentItenIndex;	
	}
	
}
