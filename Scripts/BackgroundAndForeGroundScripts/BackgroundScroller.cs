using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	
	[SerializeField] float backgroundScroolSpeed = 0.25f;
	Material myMaterial;
	public float scroolSpeed;
	Vector2 offset; //velocidade q o cenario corre

	void Start () {
		myMaterial = GetComponent<Renderer>().material;
		offset = new Vector2(0,backgroundScroolSpeed);	
	}
	void Update () {
		myMaterial.mainTextureOffset += offset * Time.deltaTime;
	}
}
