using UnityEngine;

namespace RuzikOdyssey.Characters
{
	public class WeaponController : MonoBehaviour
	{
		public Transform mainWeaponPrefab;
		public Transform secondaryWeaponPrefab;
		
		public float mainWeaponShootingRate = 0.25f;
		public float secondaryWeaponShootingRate = 0.25f;
		
		public Vector2 mainWeaponPosition = Vector2.zero;
		public Vector2 secondaryWeaponPosition = Vector2.zero;
	
		private float mainWeaponCooldown = 0f;
		private float secondaryWeaponCooldown = 0f;
		
		private void Update()
		{
			if (mainWeaponCooldown > 0)
			{
				mainWeaponCooldown -= Time.deltaTime;
			}
			if (secondaryWeaponCooldown > 0)
			{
				secondaryWeaponCooldown -= Time.deltaTime;
			}
		}
		
		public void AttackWithMainWeapon()
		{
			if (!CanAttackWithMainWeapon()) return;

			mainWeaponCooldown = mainWeaponShootingRate;
			
			var shot = (Transform)Instantiate(mainWeaponPrefab);
			shot.position = new Vector2(transform.position.x + mainWeaponPosition.x,
	                                    transform.position.y + mainWeaponPosition.y);
		}
		
		public void AttackWithSecondaryWeapon()
		{
			if (!CanAttackWithSecondaryWeapon()) return;

			secondaryWeaponCooldown = secondaryWeaponShootingRate;
			
			var shot = (Transform)Instantiate(secondaryWeaponPrefab);
			shot.position = new Vector2(transform.position.x + secondaryWeaponPosition.x,
			                            transform.position.y + secondaryWeaponPosition.y);
			
			SoundEffectsController.Instance.PlayMissileShot();
		}
		
		private bool CanAttackWithMainWeapon()
		{
			return mainWeaponCooldown <= 0f;
		}

		private bool CanAttackWithSecondaryWeapon()
		{
			return secondaryWeaponCooldown <= 0f;
		}

		public bool HasSecondaryWeapon()
		{
			return secondaryWeaponPrefab != null;
		}
	}
}
