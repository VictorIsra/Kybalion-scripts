using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour {

	[SerializeField]
	float rotSpeed = 2f;
	[SerializeField]
	private LayerMask mask;
	private float AdjustFactor = 100f;/*rott speed é mt grande..isso corrige a escala pra parecermenor..em vez rotSpeed = 1000 fato rotSpeed = 10 xD */
	void Update () {
		Ray2D ray = new Ray2D(transform.position, Vector3.Normalize(GetComponent<Rigidbody2D>().velocity));
		 RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,1f,mask);
	//	gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed * AdjustFactor);
			if(hit.collider){
				//	Debug.LogWarning("colidi com " + hit.collider.name + " d" + hit.collider.gameObject.name);
			//	Debug.LogWarning(" ray dir " + ray.direction + " norma " + hit.normal);
				Vector2 reflectDir = Vector2.Reflect( ray.direction, hit.normal);
				/*pra eu debugar em graus: */
				//float rot = 90 - Mathf.Atan2(reflectDir.y,reflectDir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.LookRotation(Vector3.forward,reflectDir);
				gameObject.GetComponent<Rigidbody2D>().velocity = reflectDir * GetComponent<AmmoHandler>().GetAmmoSpeed();
			}		
	}
}
