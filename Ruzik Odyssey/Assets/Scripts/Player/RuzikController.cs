using UnityEngine;
using RuzikOdyssey.Level;
using RuzikOdyssey.Ai;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Player
{
	public sealed class RuzikController : MonoBehaviour
	{
		public float damageFromCollision;

		public AudioClip explosionSfx;

		private MovementController movementController;
		private WeaponController weaponController;

		private HealthController healthController;
		private EnergyController energyController;
		private ShieldController shieldController;

		private bool shieldEnabled = false;

		private void Start()
		{
			movementController = gameObject.GetComponentOrThrow<MovementController>();
			weaponController = gameObject.GetComponentOrThrow<WeaponController>();
			healthController = gameObject.GetComponentOrThrow<HealthController>();
			energyController = gameObject.GetComponentOrThrow<EnergyController>();
			shieldController = gameObject.GetComponentOrThrow<ShieldController>();

			GameHelper.Instance.DisplayMissileAmmo(weaponController.missileAmmo);

			SubscribeToEvents();
		}

		private void SubscribeToEvents()
		{
			EventBroker.Subscribe<EventArgs>("FireSecondaryWeaponButton_Touch", FireSecondaryWeaponButton_Touch);
			EventBroker.Subscribe<ToggleStateChangedEventArgs>("ShieldToggle_StateChanged", ShieldToggle_StateChanged);
		}

		private void ShieldToggle_StateChanged(object sender, ToggleStateChangedEventArgs e)
		{
			shieldEnabled = e.ToggleIsOn;  
			shieldController.ChangeShieldVisibility(shieldEnabled);
		}

		private void FireSecondaryWeaponButton_Touch(object sender, EventArgs e)
		{
			weaponController.AttackWithSecondaryWeapon();
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag("Enemy"))
			{
				otherCollider.gameObject.GetComponent<AlienController>()
										.ApplyDamage(damageFromCollision);
			} 
			else if (otherCollider.CompareTag("PickUp"))
			{
				var pickUp = otherCollider.gameObject.GetComponentOrThrow<PickUp>();

				switch (pickUp.type)
				{
					case PickUpType.Health:
						healthController.Change(pickUp.amount);
						break;
					case PickUpType.Energy:
						energyController.Change(pickUp.amount);
						break;
					case PickUpType.Weapon:
						weaponController.ChangeMissileAmmo(10);
						break;
					default:
						Debug.LogWarning("Unrecognized pick up type");
						break;
				}
				Destroy(pickUp.gameObject);
			}
		}

		public void AttackWithMainWeapon()
		{
			weaponController.AttackWithMainWeapon();
		}

		public void Move(Vector2 position, Vector2 deltaPosition)
		{
			movementController.Move(position, deltaPosition);
		}

		public void Stop()
		{
			movementController.Stop();
			weaponController.FinishShootingMainWeapon();
		}

		public void ApplyDamage(float damage)
		{
			var remainingDamage = (energyController.amount > 0 && shieldEnabled)
				? shieldController.ShieldDamage(damage) 
				: damage;
			var energyLeft = energyController.Change(remainingDamage - damage);

			if (energyLeft <= 0) 
			{
				/* TODO
				 * Notify UI control that it must change state to off.
				 */

				shieldEnabled = false;
				shieldController.ChangeShieldVisibility(shieldEnabled);
			}

			var healthLeft = healthController.Change(-remainingDamage);

			if (healthLeft < 0.1) Die ();
		}

		private void Die()
		{
			Destroy (gameObject);
			MenuController.Instance.ShowGameOverMenu();
		}

		public void SlowDown(float speedDecrease)
		{
			movementController.ApplyAccelarationMultiplier(speedDecrease);
		}

		public void CancelSlowDown()
		{
			movementController.ResetAccelarationMultiplier();
		}

	}
}
