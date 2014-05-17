using UnityEngine;
using System.Collections;

public class PlayerWeaponsController : MonoBehaviour {
	
	public Transform mainWeaponPrefab;
	public Transform secondWeaponPrefab;
	
	public float shootingRate = 0.25f;

	public float mainWeaponsXAdjustment = 0.635f;
	public float mainWeaponsYAdjustment = -0.7f;
	public float secondWeaponsXAdjustment = 0.635f;
	public float secondWeaponsYAdjustment = -0.7f;
	private float shootCooldown = 0f;
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	public void AttackWithMainWeapon()
	{
		if (CanAttack())
		{
			shootCooldown = shootingRate;

			var shotTransform = (Transform) Instantiate(mainWeaponPrefab);
			shotTransform.position = new Vector2(transform.position.x + mainWeaponsXAdjustment,
			                                     transform.position.y + mainWeaponsYAdjustment);
		}
	}

	public void AttackWithSecondWeapon()
	{
		var shotTransform = (Transform) Instantiate(secondWeaponPrefab);
		shotTransform.position = new Vector2(transform.position.x + secondWeaponsXAdjustment,
		                                     transform.position.y + secondWeaponsYAdjustment);
	}

	public bool CanAttack()
	{
		return shootCooldown <= 0f;
	}
}
