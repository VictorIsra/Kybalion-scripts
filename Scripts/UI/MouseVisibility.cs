using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseVisibility{
	public static void ShowCursor(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	public static void HideCursor(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
}
