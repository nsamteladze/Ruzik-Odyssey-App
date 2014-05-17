using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public int damage = 1;
	public Vector2 speed = new Vector2(10, 10);
	public Vector2 direction = new Vector2(1, 0);
	public bool isEnemyShot = false;

	private Vector2 movement;
	
	void Start()
	{
		Destroy(gameObject, 10);
	}

	void Update()
	{
		movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
	}
	
	void FixedUpdate()
	{
		rigidbody2D.velocity = movement;
	}
}
