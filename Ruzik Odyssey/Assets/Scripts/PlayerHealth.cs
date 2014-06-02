using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
	/// <summary>
	/// Total number of hitpoints
	/// </summary>
	public int health = 10;
	private float defaultHealth;
	private const float healthBarWidth = 195f / 200f;

	private SpriteRenderer healthBar;	
	private SpriteRenderer healthBarBackground;
	private Vector3 healthScale;

	private HealthBarController healthBarController;

	private GameObject ui;

	private void Start()
	{
		ui = GameObject.Find("UI");
		if (ui == null)
			throw new Exception("Failed to find game object named 'ui' in the hierarchy");

		healthBarController = ui.GetComponentInChildren<HealthBarController>();
		if (healthBarController == null) throw new UnityException("Failed to find a health bar controller");

		defaultHealth = health;
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// If collided with a weapon
		Weapon weapon = otherCollider.gameObject.GetComponent<Weapon>();
		if ((weapon != null) && (weapon.isEnemyShot == true))
		{
			TakeDamage(weapon.damage);
			Destroy(weapon.gameObject);
		}

		EnemyMovement enemyController = otherCollider.gameObject.GetComponent<EnemyMovement>();
		if (enemyController != null) 
		{
			TakeDamage(health);
			Destroy(enemyController.gameObject);
		}
	}

	private void TakeDamage(int damage)
	{
		health -= damage;

		int healthLevel = (int)(100 * health / defaultHealth);
		healthBarController.ShowHealthLevel(healthLevel);

		if (health <= 0) Death();
	}


	private void Death()
	{
		SoundEffectsController.Instance.PlayPlayerExplosion();
		Destroy (gameObject);

		if (!Environment.IsGameOver)
		{
			Environment.GameOver();
			ui.AddComponent<GameOverMenu>();
		}
	}
}