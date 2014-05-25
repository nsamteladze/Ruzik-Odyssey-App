using UnityEngine;
using System.Collections;

public class EnemyLifespan : MonoBehaviour 
{
	private bool hasSpawn;
	private EnemyWeapon weapon;
	
	void Awake()
	{
		weapon = GetComponent<EnemyWeapon>();
	}

	void Start()
	{
		hasSpawn = false;
		collider2D.enabled = false;
		if (weapon != null) weapon.enabled = false;
	}
	
	void Update()
	{
		if (hasSpawn == false)
		{
			if (renderer.IsVisibleFrom(Camera.main))
			{
				Spawn();
			}
		}
		else
		{
//			if (weapon != null && weapon.enabled && weapon.CanAttack)
//			{
//				weapon.Attack(true);
//			}

			if (!renderer.IsVisibleFrom(Camera.main))
			{
				Destroy(gameObject);
			}
		}
	}

	private void Spawn()
	{
		hasSpawn = true;
		collider2D.enabled = true;
		if (weapon != null) weapon.enabled = true;
	}
}
