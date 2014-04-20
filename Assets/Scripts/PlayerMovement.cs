using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement: MonoBehaviour {

//	public Vector2 speed = new Vector2(0.03f, 0.03f);
//	private Vector2 movement;
//	
//	void Update()
//	{
//		if (Input.GetButton ("Fire1")) 
//		{
//			float inputX = Input.GetAxis ("Horizontal");
//			float inputY = Input.GetAxis ("Vertical");
//
//			movement = new Vector2 (speed.x * inputX, speed.y * inputY);
//		} else 
//		{
//			movement = new Vector2(0, 0);
//		}
//	}
//
//	void FixedUpdate()
//	{
//		rigidbody2D.velocity = new Vector2(movement.x, movement.y);
//	}
	

	public float moveSpeed;
	private Vector3 moveDirection;
	private bool reachedTargetPostion;
	
	// Use this for initialization
	void Start () 
	{
//		moveDirection = Vector3.right;
	}

	void Update () 
	{	
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		float targetDistance = Vector3.Distance(targetPosition, currentPosition);
		reachedTargetPostion = (targetDistance < 10.01);

		if(Input.GetButton("Fire1")) 
		{
			Vector3 moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();

			PlayerWeapon weapon = GetComponent<PlayerWeapon>();
			if (weapon != null) weapon.Attack();
		}
		else 
		{
			moveDirection = new Vector3(0, 0, 0);
		}
		
//		Vector3 target = moveDirection * moveSpeed + currentPosition;
//		transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);

		// ...
		
//		// 5 - Shooting
//		bool shoot = Input.GetButtonDown("Fire1");
//		shoot |= Input.GetButtonDown("Fire2");
//		// Careful: For Mac users, ctrl + arrow is a bad idea
//		
//		if (shoot)
//		{
//			RuzikWeaponController weapon = GetComponent<RuzikWeaponController>();
//			if (weapon != null)
//			{
//				// false because the player is not an enemy
//				weapon.Attack(false);
//			}
//		}
	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = reachedTargetPostion ? 
			new Vector2(0, 0) : new Vector2(moveSpeed * moveDirection.x, moveSpeed * moveDirection.y);
	}

}