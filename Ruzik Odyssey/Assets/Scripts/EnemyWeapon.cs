using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour 
{
	public Transform shotPrefab;
	public float shootingRate = 0.25f;
	public float positionXAdjustment = 0.635f;
	public float positionYAdjustment = -0.7f;
	private float shootCooldown;
	
	void Start()
	{
		shootCooldown = Random.Range(0f, 2f);
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	public void Attack()
	{
		if (CanAttack())
		{
			shootCooldown = shootingRate;

			var shotTransform = Instantiate(shotPrefab) as Transform;
			var playerPosition = transform.position;
			shotTransform.position = new Vector2(playerPosition.x + positionXAdjustment,
			                                     playerPosition.y + positionYAdjustment);

			SoundEffectsController.Instance.PlayLaserShot();
		}
	}

	public bool CanAttack()
	{
		var cooldownOff = (shootCooldown <= 0f);
		if (cooldownOff && (Random.Range(0, 11) > 9)) return true;

		return false;
	}
}
