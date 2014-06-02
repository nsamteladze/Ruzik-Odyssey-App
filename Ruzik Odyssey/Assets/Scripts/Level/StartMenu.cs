using UnityEngine;
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
		if (GUI.Button(new Rect(925 * scaleOffset.x, 510 * scaleOffset.y, 785 * scale, 345 * scale), 
		               "", GUIStyle.none))
		{
			Environment.StartMission();
			Application.LoadLevel("default_level"); 
		}
	}
}