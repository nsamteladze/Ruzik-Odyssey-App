using UnityEngine;
using System;
using RuzikOdyssey.Common;

public class LevelLoadingScreenUI : ScreenUIBase
{
	protected override void InitializeUI()
	{
		if (GUI.Button(new Rect(0 * scaleOffset.x, 0 * scaleOffset.y, 2048 * scale, 1152 * scale), 
		               "", GUIStyle.none))
		{
			GameEnvironment.StartMission();
			Application.LoadLevel("default_level"); 
		}
	}
}


