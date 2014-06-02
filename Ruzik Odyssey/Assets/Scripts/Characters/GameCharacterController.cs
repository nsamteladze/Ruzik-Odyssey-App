using UnityEngine;

namespace RuzikOdyssey.Characters
{
	public sealed class GameCharacterController : MonoBehaviour
	{
		private MovementController movementController;
		private WeaponController weaponController;

		private void Start()
		{
			movementController = this.gameObject.GetComponent<MovementController>();
			if (movementController == null) throw new UnityException("Failed to initialize movement controller");

			weaponController = this.GetComponent<WeaponController>();
			if (weaponController == null) throw new UnityException("Failed to initialize weapon controller");
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
		}

	}
}
