using UnityEngine;
using System.Collections;
using System;

public class ButtonsRenderer : MonoBehaviour 
{
	private PlayerWeaponsController playerWeapon;
	private GameObject ui;
	
	void Start()
	{
		GameObject player = GameObject.Find("player");
		if (player == null) 
			throw new Exception("Failed to find game object named 'player' in the hierarchy");

		playerWeapon = player.GetComponent<PlayerWeaponsController>();
		if (playerWeapon == null) 
			throw new Exception("Failed to retrieve PlayerWeaponsController from the player game object");

		ui = GameObject.Find("UI");
		if (ui == null)
			throw new Exception("Failed to find game object named 'ui' in the hierarchy");
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(1865 * Environment.ScaleOffset.x, 5 * Environment.ScaleOffset.y, 
		                        175 * Environment.Scale, 175 * Environment.Scale), 
		               "", GUIStyle.none) ){
			if (!Environment.IsPaused)
			{
				ui.AddComponent<PauseMenu>();
				Environment.Pause();
			}
		}

		if (GUI.Button(new Rect(1830 * Environment.ScaleOffset.x, 950 * Environment.ScaleOffset.y, 
		                        195 * Environment.Scale, 200 * Environment.Scale), 
		               "", GUIStyle.none) ){
			playerWeapon.AttackWithSecondWeapon();
		}
	}
}
