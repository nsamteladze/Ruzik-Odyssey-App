using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int health = 1;
	
	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;
	
	/// <summary>
	/// Inflicts damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount)
	{
		health -= damageCount;
		
		if (health <= 0)
		{
			Death();
		}
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		PlayerWeaponHit playerWeaponShot = otherCollider.gameObject.GetComponent<PlayerWeaponHit>();
		if (playerWeaponShot != null)
		{
			Damage(playerWeaponShot.damage);
			Destroy(playerWeaponShot.gameObject);
		}
		
		PlayerMovement playerController = otherCollider.gameObject.GetComponent<PlayerMovement>();
		if (playerController != null) 
		{
			Damage(health);
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
