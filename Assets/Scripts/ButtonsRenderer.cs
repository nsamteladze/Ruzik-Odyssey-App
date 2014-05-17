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

		int buttonWidth = Screen.width / 3;
		int buttonHeight = Screen.height / 4;
		int buttonX = (int) (Screen.width - 0.75 * buttonWidth);
		int buttonY = Screen.height - buttonHeight;

		GUI.backgroundColor = Color.clear;

		if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), fireSecondWeaponTexture))
		{
			playerWeapon.AttackWithSecondWeapon();
		}
	}
}
