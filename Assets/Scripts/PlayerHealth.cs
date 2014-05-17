using UnityEngine;
using System.Collections;

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

	void Awake()
	{
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		healthBarBackground = GameObject.Find("HealthBarBackground").GetComponent<SpriteRenderer>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
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

		UpdateHealthBar ();

		if (health <= 0) Death();
	}

	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.1f);

		Debug.Log("Position X: " + healthBar.transform.localPosition.x);
		Debug.Log("Position Y: " + healthBar.transform.localPosition.y);
		Debug.Log("Scale X: " + healthBar.transform.localScale.x);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * (1 / defaultHealth), 1, 1);
		healthBar.transform.localPosition = 
			new Vector2 (healthBar.transform.localPosition.x - (healthBarWidth / defaultHealth), 
			             healthBar.transform.localPosition.y);
	}

	private void Death()
	{
		Destroy (gameObject);
		Destroy (healthBar.gameObject);
		Destroy (healthBarBackground.gameObject);
	}
}