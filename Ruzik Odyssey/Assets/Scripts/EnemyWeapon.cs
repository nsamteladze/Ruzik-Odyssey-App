using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour 
{
	public Transform shotPrefab;
	public float shootingRate = 0.25f;
	public float positionXAdjustment = 0.635f;
	public float positionYAdjustment = -0.7f;
	private float shootCooldown;

	private Animator animator;

	private void Start()
	{
		shootCooldown = Random.Range(0f, 2f);

		animator = GetComponent<Animator>();

		if (animator == null) throw new UnityException(string.Format(
			"Failed to instantiate animator for {0}", this.name));
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
		if (CanAttack() && animator != null)
		{
			animator.SetBool("IsShooting", true);
		}
	}

	private void Shoot()
	{
		shootCooldown = shootingRate;
		
		var shotTransform = Instantiate(shotPrefab) as Transform;
		var playerPosition = transform.position;
		shotTransform.position = new Vector2(playerPosition.x + positionXAdjustment,
		                                     playerPosition.y + positionYAdjustment);
		
		SoundEffectsController.Instance.PlayLaserShot();
		animator.SetBool("IsShooting", false);
	}

	public bool CanAttack()
	{
		var cooldownOff = (shootCooldown <= 0f);
		if (cooldownOff && (Random.Range(0, 11) > 9)) return true;

		return false;
	}
}
