﻿using UnityEngine;
using System;

public class StartMenu : MonoBehaviour
{
	private Vector2 scaleOffset = Vector2.one;
	private float scale = 1.0f;
	
	void Start()
	{
		scaleOffset.x = Screen.width / Environment.DesignWidth;
		scaleOffset.y = Screen.height / Environment.DesignHeight;
		scale = Mathf.Max(scaleOffset.x, scaleOffset.y); 
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(938 * scaleOffset.x, 458 * scaleOffset.y, 580 * scale, 270 * scale), 
		               "", GUIStyle.none))
		{
			Application.LoadLevel("main_screen"); 
		}
	}
}