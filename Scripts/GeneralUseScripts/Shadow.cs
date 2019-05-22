using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shadow : MonoBehaviour {
	enum layer{shadows,backTerrainObjects, Player_projectil,Enemy_projectil,Boss,boss,colectable}
	/*funfou, generaliza-lo pra todos os objetos */
	// Use this for initialization
	[SerializeField] bool defaultMode = true;
	Material shadowMaterial;
	GameObject shadow;
	private float lastx;/*posicao x e y no frame anterior */
	private float lasty;
	private float currentx = 0f;/*posicao de x e y no frame atual */
	private float currenty = 0f;
	private bool staticParent = false;/*se for um obeto que fica parado */
	private float speed = 2f;
	private float factor;
	private SpriteRenderer shadowParentSpriteRenderer;

	void Awake(){
		factor = speed * Time.deltaTime;
		shadowMaterial = Resources.Load<Material>("materials/Shadow");
		GenerateShadow();
		LayerAdjust();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if(!staticParent){
			if(defaultMode)
				DefaultMode();
			else
				SimpleMode();
		}	
	
	}
	public void IsDefaultMode(bool status){
		defaultMode = status;
	}

	/*publica pois se adiciono esse script a um componente via codigo,
	o getshadow é shamado no awake, logo no script q eu o adiciono, o arake é
	chamado antes deu setar um parent, de modo q preciso chamar de novo */
	private void GenerateShadow(){
		Vector3 aux = transform.position;
		aux.y -=2f;
		shadow = new GameObject("shadow");
		shadow.tag = Constantes.SHADOW;
		shadow.transform.position = aux;
		shadow.AddComponent<SpriteRenderer>();
		int backGroundLayer;
		backGroundLayer = LayerMask.NameToLayer(layer.backTerrainObjects.ToString());
		
		shadow.transform.SetParent(transform);
		//shadow.transform.position = aux;
		/*se for algo no solo parado */
		if(gameObject.GetComponent<SpriteRenderer>().sortingLayerName.Equals(layer.backTerrainObjects.ToString())){
			shadow.GetComponent<SpriteRenderer>().sortingLayerName = layer.backTerrainObjects.ToString();
			shadow.GetComponent<SpriteRenderer>().sortingOrder = shadow.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder -1;
			aux.y -=4f;
			staticParent = true;
		}	
		/*se for municao */
		else if( gameObject.layer == LayerMask.NameToLayer(layer.Player_projectil.ToString())
		||  gameObject.layer == LayerMask.NameToLayer(layer.Enemy_projectil.ToString())){
			shadow.transform.localScale = new Vector3(0.75f,0.75f,0f);
			shadow.GetComponent<SpriteRenderer>().sortingLayerName = layer.shadows.ToString();
		}
		/*se for chefe */
		else if( gameObject.layer == LayerMask.NameToLayer(layer.Boss.ToString())){
			shadow.GetComponent<SpriteRenderer>().sortingLayerName = layer.boss.ToString();
			if(gameObject.GetComponent<SpriteRenderer>())/*checa ref */
				shadow.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder -1;
		}		
		else
			shadow.GetComponent<SpriteRenderer>().sortingLayerName = layer.shadows.ToString();

		if( gameObject.layer == LayerMask.NameToLayer(layer.colectable.ToString())){
			aux.y +=5.25f;/*ate aki tinha diminuido 6, adicionando 5.5, o delta de 0.5 é a distancia q quero */
			shadow.transform.position = aux;
		}		

		//shadow.transform.position = aux;
		GetShadowShape(shadow);

		if(!gameObject.CompareTag(Constantes.PLAYER))
			IsDefaultMode(false);
	
	}
	public void GetShadowShape(GameObject shadow){
		shadowParentSpriteRenderer = shadow.transform.parent.GetComponent<SpriteRenderer>();
		if(shadowParentSpriteRenderer){
			shadow.GetComponent<SpriteRenderer>().sprite = shadowParentSpriteRenderer.sprite;
			shadow.GetComponent<SpriteRenderer>().material = shadowMaterial;
		}
	}
	public void ChangeShadowShape(){
		shadow.GetComponent<SpriteRenderer>().sprite = shadowParentSpriteRenderer.sprite;
	}
	private void DefaultMode(){
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 newPost = new Vector3(transform.position.x - ( 4*horizontal), transform.position.y + (vertical),0);
		
		if(Input.GetAxisRaw("Horizontal") == 0 )
			newPost.x = shadow.transform.position.x;
		if(Input.GetAxisRaw("Vertical") == 0)	
			newPost.y = shadow.transform.position.y;
		
		shadow.transform.position = Vector3.MoveTowards(shadow.transform.position, newPost, factor/2);
	}
	private void SimpleMode(){
		/*qqr objeto com sombra que n seja o player.
		o comportamento da sombra aqui é analogo ao modo default, mas um pouco mais simples */
			float deltax = currentx - lastx;/*variacao da ppsicao x no intervalo de 1 frame */
			float deltay = currenty - lasty;

			/*anterior pega o valor atual antes do atual trocar, assim a diferenca dos valores sera congruente com o estado da posicao  */
			lastx = currentx;
			lasty = currenty; 
			
			currentx = shadow.transform.position.x;
			currenty = shadow.transform.position.y;
			
			Vector3 newPost = new Vector3(transform.position.x - ( 4*deltax), transform.position.y + (4*deltay),0);
			
			if(Mathf.Floor(deltax) == 0 )
				newPost.x = shadow.transform.position.x;
			if(Mathf.Floor(deltay) == 0)	
				newPost.y = shadow.transform.position.y;
			
			shadow.transform.position = Vector3.MoveTowards(shadow.transform.position, newPost, factor);
			/*como animacao do robo muda, isso fara a sombra mudar de acordo */
			
		if(gameObject.CompareTag(Constantes.ROBOT))
			ChangeShadowShape();
	}
	public void LayerAdjust(){
			shadow.GetComponent<SpriteRenderer>().sortingLayerName = shadow.transform.parent.GetComponent<SpriteRenderer>().sortingLayerName;		
			shadow.GetComponent<SpriteRenderer>().sortingOrder = shadow.transform.parent.GetComponent<SpriteRenderer>().sortingOrder-1;

	}
}
