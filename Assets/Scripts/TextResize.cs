﻿using UnityEngine;
using System.Collections;

public class TextResize : MonoBehaviour {

	private const float originalWidth = 553f; 
	private const float originalHeight = 311f;

	private Vector3 scale;
	
	public void Start()
	{
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		GameObject ui = GameObject.Find ("UI");

		foreach (GUIText guiText in ui.GetComponentsInChildren<GUIText>())
		{
			guiText.fontSize = (int) (30 * Screen.width / originalWidth);
		}
	}
}