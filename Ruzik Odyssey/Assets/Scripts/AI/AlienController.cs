using UnityEngine;
using RuzikOdyssey.Weapons;
using RuzikOdyssey.Characters;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Ai
{
	public class AlienController : MonoBehaviour
	{
		public float warzoneSpeed = 5f;
		public int scoreForKill = 1;

		private bool isInWarzone;

		private float nonWarzoneSpeed = 100f;

		public int health;

		private WeaponController weaponController;
		private MovementStrategy movementStrategy;

		private float speed;


		public void ApplyDamage(int damage)
		{
			TakeDamage(damage);
		}

		private void Start()
		{
			weaponController = GetComponent<WeaponController>();
			movementStrategy = GetComponent<MovementStrategy>();

			isInWarzone = false;
			speed = nonWarzoneSpeed;
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.tag.Equals("WarzoneBoundary")) OnEnterWarzone();
		}

		private void OnWeaponHit(GameObject gameObject)
		{

		}

		private void OnEnterWarzone()
		{
			isInWarzone = true;
			speed = warzoneSpeed;
			rigidbody2D.velocity = Vector2.zero;
		}

		private void TakeDamage(int damage)
		{
			health -= damage;
			if (health < 0) Die();
		}

		private void Die()
		{
			Destroy(gameObject);
			Game.AddScore(scoreForKill);
		}

		private void Update()
		{
			if (isInWarzone && weaponController != null)
			{
				weaponController.AttackWithMainWeapon();
			}
		}

		private void FixedUpdate()
		{
			var movementDirection = movementStrategy.GetMovementDirection(transform.localPosition, isInWarzone);
			rigidbody2D.AddForce(movementDirection * speed);
		}
	}
}
