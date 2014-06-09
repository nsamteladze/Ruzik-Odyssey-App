using UnityEngine;
using System.Collections;
using RuzikOdyssey.Player;

public class FireSecondaryWeaponTouchButton : TouchButton 
{	
	private WeaponController playerWeaponController;

	private new void Start()
	{
		base.Start();
 		
		playerWeaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
		if (playerWeaponController == null) 
			throw new UnityException("Failed to retrieve weapon controller from the player game object");
	}

	protected override void OnButtonTouch()
	{
		if (playerWeaponController.HasSecondaryWeapon())
		{
			playerWeaponController.AttackWithSecondaryWeapon();
		}
	}
}
