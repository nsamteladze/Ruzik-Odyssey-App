using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerDeltaMovement : MonoBehaviour
{

	public Vector2 speed  = Vector2.one;

	private PlayerWeaponsController playerWeaponsController;
	private Vector2 movement = Vector2.zero;

	private float leftBoundary;
	private float rightBoundary;
	private float topBoundary;
	private float bottomBoudary;

	private int movementTouchFingerId = -1;

	private Animator animator;

	private void Start()
	{
		leftBoundary = -(Camera.main.aspect * Camera.main.orthographicSize - 1.0f);
		rightBoundary = Camera.main.aspect * Camera.main.orthographicSize - 2.0f;
		topBoundary = Camera.main.orthographicSize - 1.0f;
		bottomBoudary = -(Camera.main.orthographicSize - 0.5f);

		playerWeaponsController = GetComponent<PlayerWeaponsController>();
		if (playerWeaponsController == null) 
		{
			throw new UnityException("Failed to aquire player's weapon");
		}

		animator = GetComponent<Animator>();
	}

	private void Update () 
	{	
		var movedTouches = Input.touches.Where(x => x.phase == TouchPhase.Moved);
		if (movedTouches.Count() > 0)
		{
			animator.SetBool("IsShooting", true);

			playerWeaponsController.AttackWithMainWeapon();
			Move(movedTouches.First().deltaPosition);
			movementTouchFingerId = movedTouches.First().fingerId;
			return;
		}

		if (Input.touches.Where(x => (x.phase == TouchPhase.Ended || x.phase == TouchPhase.Canceled) &&
		                             x.fingerId == movementTouchFingerId)
		    			 .Count() > 0)
		{
			movement = Vector2.zero;
			movementTouchFingerId = -1;

			animator.SetBool("IsShooting", false);
		}

		if (Input.touches.Where(x => x.phase == TouchPhase.Stationary).Count() > 0)
		{
			animator.SetBool("IsShooting", true);
			playerWeaponsController.AttackWithMainWeapon();
			return;
		}

		animator.SetBool("IsShooting", false);
	}

	private void Move(Vector2 deltaMovement)
	{		
		movement += deltaMovement;

		if ((transform.position.x <= leftBoundary && movement.x < 0) || 
		    (transform.position.x >= rightBoundary && movement.x > 0))
		{
			movement.x = 0;
		}
		if ((transform.position.y <= bottomBoudary && movement.y < 0) || 
		    (transform.position.y >= topBoundary && movement.y > 0))
		{
			movement.y = 0;
		}
	}
	
	private void FixedUpdate()
	{
		rigidbody2D.velocity = new Vector2(Math.Abs(movement.x) > speed.x 
		                                   ? Math.Sign(movement.x) * speed.x 
		                                   : movement.x, 
		                                   Math.Abs(movement.y) > speed.y 
		                                   ? Math.Sign(movement.y) * speed.y 
		                                   : movement.y);

//		rigidbody2D.velocity = new Vector2(movement.x, movement.y);
		movement -= rigidbody2D.velocity;
	}
}

