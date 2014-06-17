using UnityEngine;
using System.Collections;
using System;

namespace RuzikOdyssey.Player
{
	public class HealthController : MonoBehaviour
	{
		public float health = 10.0f;

		private float defaultHealth;
		private BarController healthBarController;
		private GameObject ui;

		private void Start()
		{
			ui = GameObject.Find("UI");
			if (ui == null) throw new Exception("Failed to find game object named 'ui' in the hierarchy");
			var healthBar = GameObject.Find("HealthBar");
			if (healthBar == null) throw new UnityException("Failed to find health bar in the hierarchy");

			healthBarController = healthBar.GetComponent<BarController>();
			if (healthBarController == null) throw new UnityException("Failed to find a health bar controller");

			defaultHealth = health;
		}

		public void AddHealth(float health)
		{
			this.health += health;
			if (this.health > defaultHealth) this.health = defaultHealth;
		}

		public float TakeDamage(float damage)
		{
			health -= damage;

			int healthLevel = (int)(100 * health / defaultHealth);
			healthBarController.ShowLevel(healthLevel);

			return health;
		}
	}
}