using UnityEngine;
using System;

public class Chapter1MapScreenUI : ScreenUIBase
{	
	protected override void InitializeUI()
	{
		if (GUI.Button(new Rect(0 * scaleOffset.x, 0 * scaleOffset.y, 2048 * scale, 1152 * scale), 
		               "", GUIStyle.none))
		{
			Application.LoadLevel("main_screen"); 
		}
	}
}

