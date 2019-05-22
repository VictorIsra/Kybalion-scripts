using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]

public class ShieldAura : MonoBehaviour {
    [SerializeField] GameObject auraEffect;//qd algum inimigo encosta nela
	public void StartAuraProtection(Player playerRef){
        StartCoroutine(StarAuraProtectionCoroutine(playerRef));
    }
    IEnumerator StarAuraProtectionCoroutine(Player playerRef){	
        if(playerRef)
            playerRef.IsPlayerImortal(true);
		yield return new WaitForSeconds(PlayerGlobalStatus.GetShieldAuraCurrentDuration());
        if(playerRef)
            playerRef.IsPlayerImortal(false);
        Destroy(gameObject);
	}	
    private void OnTriggerEnter2D(Collider2D other) {
        /*se for uma municao que nao seja do player... */
        if(other.gameObject.GetComponent<AmmoHandler>()){
            Destroy(other.gameObject);
            GenerateAuraEffect(other.gameObject);
        }
       
        /* n rola pq a colisao ta baseda em layers...sem saco p acertar 
        if( other.gameObject.layer == Constantes.PLAYER_PROJECTIL_LAYER){
            GameObject auraEffect = Instantiate(this.auraEffect,other.gameObject.transform.position,Quaternion.identity);
            auraEffect.transform.SetParent(gameObject.transform);
            Destroy(auraEffect,0.5f);
        }*/

    }
    private void GenerateAuraEffect(GameObject other){
        GameObject auraEffect = Instantiate(this.auraEffect,other.transform.position,Quaternion.identity);
        auraEffect.transform.SetParent(gameObject.transform);
        Destroy(auraEffect,0.5f);
    }
}
