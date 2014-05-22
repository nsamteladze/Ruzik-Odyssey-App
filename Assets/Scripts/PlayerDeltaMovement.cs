using UnityEngine;
using System.Collections;
using System;

public class PlayerDeltaMovement : MonoBehaviour
{

	public float movementSpeed = 1;

	private PlayerWeaponsController weapon;
	Vector2 movementDirection;

	private float leftBoundary;
	private float rightBoundary;
	private float topBoundary;
	private float bottomBoudary;

	private void Start()
	{
		leftBoundary = -(Camera.main.aspect * Camera.main.orthographicSize - 1.0f);
		rightBoundary = Camera.main.aspect * Camera.main.orthographicSize - 2.0f;
		topBoundary = Camera.main.orthographicSize - 1.0f;
		bottomBoudary = -(Camera.main.orthographicSize - 0.5f);

		weapon = GetComponent<PlayerWeaponsController>();
		if (weapon == null) 
		{
			throw new UnityException("Failed to aquire player's weapon");
		}
	}

	private void Update () 
	{	
		movementDirection = new Vector2(0, 0);

		if (Input.touchCount == 0) return;

		if (Input.touchCount > 1) return;

		Touch movementTouch = Input.GetTouch(0);

		if ((movementTouch.phase == TouchPhase.Canceled) || 
		    (movementTouch.phase == TouchPhase.Ended))
		{
			return;
		}

		weapon.AttackWithMainWeapon();

		if (movementTouch.phase == TouchPhase.Moved)
		{
			movementDirection = movementTouch.deltaPosition;

			if ((transform.position.x <= leftBoundary && movementDirection.x < 0) || 
			    (transform.position.x >= rightBoundary && movementDirection.x > 0))
			{
				movementDirection.x = 0;
			}
			if ((transform.position.y <= bottomBoudary && movementDirection.y < 0) || 
			    (transform.position.y >= topBoundary && movementDirection.y > 0))
			{
				movementDirection.y = 0;
			}
		}
	}
	
	private void FixedUpdate()
	{
		rigidbody2D.velocity = new Vector2(movementSpeed * movementDirection.x, 
		                                   movementSpeed * movementDirection.y);
	}
}

