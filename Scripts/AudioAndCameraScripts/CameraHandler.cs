using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
	[HideInInspector]
	public Rect rect;
    
    public Vector2 cameraBounds;

	private void AdjustScreen(){
		/*escolho o aspect ratio alvo, meu default aqui é 16:9 */
         //float targetaspect = 1280.0f / 800.0f;
         /*valores hardcored que se adequam a todos os casos...nao curto isso, mas só assim que deu */
		/*valores sem o asse da camera: xcorrection 15.6f tcorrenction10f */
         /*com o asset da camera: */
         float xCorrection = 17.5f;
         float yCorrection = 10f;
         //pego o aspect ratio da tela
         //float windowaspect = (float)Screen.width / (float)Screen.height;
         // viewport vai se ajustar a essa taxa ( manterá proporção)
         //float scaleheight = windowaspect / targetaspect;
         //referencia a camera pra mudar o viewport dela
         Camera camera = Camera.main;
     
         // se a altura for menor que a altura atual, completa com retangulo
         /* if (scaleheight < 1.0f){  
            rect = camera.rect;
         
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
         
            camera.rect = rect;
         }
         else{  
            float scalewidth = 1.0f / scaleheight;
         
            rect = camera.rect;
         
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
         }*/
        cameraBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
        cameraBounds.x = xCorrection;
        cameraBounds.y = yCorrection;

    }
    
}
