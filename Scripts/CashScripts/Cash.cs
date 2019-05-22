using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour {
	[SerializeField] private float cashAmount = 0;
	[SerializeField] bool move = true;
	private float cashSpeed = 0;
	private UiManagement uiManagement;

	private void Awake() {
		uiManagement = FindObjectOfType<UiManagement>();
		SetSpeed();
		SetShadow();
	}
	private void Update() {
		if(move)
			MoveCash();
	}
	private void SetShadow(){
		if(!gameObject.GetComponent<Shadow>())
			gameObject.AddComponent<Shadow>();
	}	
	public void SetCash(float cashAmount){
		this.cashAmount = cashAmount;
	}
	public float GetCash(){
		return cashAmount;
	}
	/*velocidade que a grana vai percorrer o cenário até sumir */
	private void SetSpeed(){
		cashSpeed = Random.Range(0.5f, 2f);
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag(Constantes.PLAYER)){
			AddCash();
			/*cash fica num container por causa da animacao, destrua o parent que destruirá ela hehe */
			if(gameObject.transform.parent != null ){/*pq posso spawnar sem o container/parent, tipo dps de matar um dragao */
				if(gameObject.transform.parent.CompareTag(Constantes.CASH_))
					Destroy(gameObject.transform.parent.gameObject);
				else/*bau é filho do container q o spawnou, por isso acima a checagem de tag */
					Destroy(gameObject);
			}	
			else
				Destroy(gameObject);

		}	
	}
	private void AddCash(){
		if(uiManagement){
			PlayerGlobalStatus.addPlayerCash(GetCash());
			uiManagement.UpdateScoreText();
		}	
		else
			Debug.LogWarning(Constantes.MISSING_REF);
	}
	private void MoveCash(){
		//if(gameObject.transform.parent != null){
		//	if(gameObject.transform.parent.CompareTag(Constantes.CASH_))
		//		gameObject.transform.parent.transform.Translate( Vector2.down *  cashSpeed * Time.deltaTime);
		transform.Translate( Vector2.down *  cashSpeed * Time.deltaTime);
		//	}
	}
		
}
