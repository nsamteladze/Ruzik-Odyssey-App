using UnityEngine;
using System;

public class GlobalMapScreenUI : ScreenUIBase
{
	protected override void InitializeUI()
	{
		if (GUI.Button(new Rect(35 * scaleOffset.x, 555 * scaleOffset.y, 520 * scale, 270 * scale), 
		               "", GUIStyle.none))
		{
			Application.LoadLevel("chapter1_map_screen"); 
		}
	}
}

