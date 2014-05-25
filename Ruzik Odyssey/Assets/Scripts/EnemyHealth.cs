using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
	public int health = 1;
	public bool isEnemy = true;

	public void Damage(int damageCount)
	{
		health -= damageCount;
		
		if (health <= 0) Death();
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// If collided with a weapon
		Weapon weapon = otherCollider.gameObject.GetComponent<Weapon>();
		if ((weapon != null) && (weapon.isEnemyShot != isEnemy))
		{
			Damage(weapon.damage);
			Destroy(weapon.gameObject);
		}

		// If collided with player
		PlayerMovement playerController = otherCollider.gameObject.GetComponent<PlayerMovement>();
		if (playerController != null) 
		{
			Death();
			Destroy(playerController.gameObject);
		}
	}

	void Death()
	{
		Destroy(gameObject);

		Score score = GameObject.Find("Score").GetComponent<Score>();
		score.score += 1;
	}
}
