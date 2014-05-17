using UnityEngine;
using System.Collections;
using System;

public class ButtonsRenderer : MonoBehaviour {
	
	public Texture fireSecondWeaponTexture;
	private PlayerWeaponsController playerWeapon;

	void Start()
	{
		playerWeapon = GameObject.Find("player").GetComponent<PlayerWeaponsController>();
	}
	
	void OnGUI()
	{

		int buttonWidth = Screen.width / 6;
		int buttonHeight = Screen.height / 8;
		int buttonX = Screen.width - buttonWidth;
		int buttonY = Screen.height - buttonHeight;

		GUI.backgroundColor = Color.clear;

		// Draw a button to start the game
		if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), fireSecondWeaponTexture))
		{
			playerWeapon.AttackWithSecondWeapon();
		}
	}
}
