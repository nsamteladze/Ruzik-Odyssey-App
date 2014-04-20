using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------
	
	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;
	
	/// <summary>
	/// Cooldown in seconds between two shots
	/// </summary>
	public float shootingRate = 0.25f;

	/// <summary>
	/// The position X adjustment.
	/// </summary>
	public float positionXAdjustment = 0.635f;

	/// <summary>
	/// The position Y adjustment.
	/// </summary>
	public float positionYAdjustment = -0.7f;
	
	//--------------------------------
	// 2 - Cooldown
	//--------------------------------
	
	private float shootCooldown;
	
	void Start()
	{
		shootCooldown = 0f;
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}
	
	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------
	
	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack()
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			
			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;
			
			// Assign position
			var playerPosition = transform.position;
			shotTransform.position = new Vector2(playerPosition.x + positionXAdjustment,
			                                     playerPosition.y + positionYAdjustment);

			// The is enemy property
			PlayerWeaponHit shot = shotTransform.gameObject.GetComponent<PlayerWeaponHit>();
			if (shot != null) shot.isEnemyShot = false;
			
			// Make the weapon shot always towards it
			PlayerWeaponMovement move = shotTransform.gameObject.GetComponent<PlayerWeaponMovement>();
			if (move != null)
			{
				move.direction = this.transform.right; // towards in 2D space is the right of the sprite
			}
		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}
