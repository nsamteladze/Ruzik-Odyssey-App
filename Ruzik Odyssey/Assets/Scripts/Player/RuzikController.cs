using UnityEngine;
using RuzikOdyssey.Level;
using RuzikOdyssey.Ai;
using System;

namespace RuzikOdyssey.Player
{
	public sealed class RuzikController : MonoBehaviour
	{
		public float damageFromCollision;

		private MovementController movementController;
		private WeaponController weaponController;

		private HealthController healthController;
		private ShieldController shieldController;

		private bool shieldEnabled = false;

		private void Start()
		{
			movementController = this.gameObject.GetComponent<MovementController>();
			if (movementController == null) throw new UnityException("Failed to initialize movement controller");

			weaponController = this.GetComponent<WeaponController>();
			if (weaponController == null) throw new UnityException("Failed to initialize weapon controller");

			healthController = this.GetComponent<HealthController>();
			if (healthController == null) throw new UnityException("Failed to initialize health controller");

			shieldController = this.GetComponent<ShieldController>();
			if (shieldController == null) throw new UnityException("Failed to initialize shield controller");

			SubscribeToEvents();
		}

		private void SubscribeToEvents()
		{
			EventBroker.Subscribe<EventArgs>("FireSecondaryWeaponButton_Touch", FireSecondaryWeaponButton_Touch);
			EventBroker.Subscribe<ToggleStateChangedEventArgs>("ShieldToggle_StateChanged", ShieldToggle_StateChanged);
		}

		private void ShieldToggle_StateChanged(object sender, ToggleStateChangedEventArgs e)
		{
			Debug.Log("Shield is " + e.ToggleIsOn);

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
				var pickUp = otherCollider.gameObject.GetComponent<PickUp>();
				if (pickUp == null) throw new UnityException("Pick Up lacks PickUp component");

				switch (pickUp.type)
				{
					case PickUpType.Health:
						healthController.AddHealth(pickUp.amount);
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
			var remainingDamage = shieldEnabled ? shieldController.ShieldDamage(damage) : damage;
			var healthLeft = healthController.TakeDamage(remainingDamage);

			Debug.Log(string.Format("Shielded: {0}, Taken: {1}, Left: {2}", damage - remainingDamage, remainingDamage, healthLeft));

			if (healthLeft < 0) Die ();
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
