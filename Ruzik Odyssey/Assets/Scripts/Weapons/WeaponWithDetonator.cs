using UnityEngine;
using System.Collections;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Player;
using System;
using System.Linq;

namespace RuzikOdyssey.Weapons
{
	public class WeaponWithDetonator : MonoBehaviour 
	{
		public int damage = 1;
		public Vector2 speed = new Vector2(10, 10);
		public Vector2 direction = new Vector2(1, 0);
		public bool isEnemyShot = false;
		public float detonationDelay = 1.0f;

		public AudioClip explosionSound;

		private Vector2 movement;
		private string hitTag;

		private Animator animator;
		
		private void Start()
		{
			rigidbody2D.velocity = new Vector2(speed.x * direction.x, speed.y * direction.y);

			hitTag = isEnemyShot ? "Player" : "Enemy";

			animator = this.gameObject.GetComponent<Animator>();
			if (animator == null) Debug.LogWarning(String.Format(
				"Failed to initialize animator for game object {0}", this.gameObject.name));
		}
		
		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag(hitTag)) Invoke("Detonate", detonationDelay);
		}

		private void Detonate()
		{
			if (animator != null) animator.SetBool("IsDetonating", true);
			else Explode();
		}

		private void Explode()
		{
			var radiusCollider = (CircleCollider2D) this.collider2D;
			var hitColliders = Physics2D.OverlapCircleAll(radiusCollider.center, radiusCollider.radius)
										.Where(x => x.CompareTag(hitTag));

			foreach (var hitCollider in hitColliders)
			{
				if (isEnemyShot)
				{
					var ruzikController = hitCollider.gameObject.GetComponent<RuzikController>();
					ruzikController.ApplyDamage(damage);
				}
				else
				{
					var alienController = hitCollider.gameObject.GetComponent<AlienController>();
					alienController.ApplyDamage(damage);
				}
			}

			SoundEffectsController.Instance.Play(explosionSound);

			Destroy(this.gameObject);
		}
	}
}
