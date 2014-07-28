using UnityEngine;
using RuzikOdyssey.Weapons;
using RuzikOdyssey.Player;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Ai
{
	public class AlienController : MonoBehaviour
	{
		public GameObject energyBall;

		public float warzoneSpeed = 50f;
		public int scoreForKill = 1;

		public float damageFromCollision = 1.0f;

		private bool isInWarzone;

		private float nonWarzoneSpeed = 100f;

		public float health;

		private WeaponController weaponController;
		private MovementStrategy movementStrategy;

		private float speed;


		public void ApplyDamage(float damage)
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

			if (otherCollider.CompareTag("Player"))
				otherCollider.gameObject.GetComponent<RuzikController>()
										.ApplyDamage(damageFromCollision);
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

		private void TakeDamage(float damage)
		{
			health -= damage;
			if (health <= 0) Die();
		}

		private void Die()
		{
			DropEnergy();

			SoundEffectsController.Instance.PlayPlayerTaunt();
			GameHelper.Instance.AddScore(scoreForKill);

			Destroy(gameObject);
		}

		private void DropEnergy()
		{
			var energyAmount = Random.Range(0, 7);

			Debug.Log("Dropped energy: " + energyAmount);

			for (int i = 0; i < energyAmount; i++)
			{
				energyBall.InstantiateAtPosition(
					this.transform.position,
					5.0f * Vector2.one.RandomNormilazed(), ForceMode2D.Impulse);
			}
		}

		private void Update()
		{
			if (isInWarzone && weaponController != null)
			{
				weaponController.AttackWithMainWeapon();
				weaponController.AttackWithSecondaryWeapon();
			}
		}

		private void FixedUpdate()
		{
			var movementDirection = movementStrategy.GetMovementDirection(transform.localPosition, isInWarzone);
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(movementDirection * speed);
		}
	}
}
