using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement: MonoBehaviour {

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

		if (Input.GetButton("Fire1") && !InControlsArea(Input.mousePosition)) 
		{
			Vector3 moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();

			PlayerWeaponsController weapon = GetComponent<PlayerWeaponsController>();
			if (weapon != null) weapon.AttackWithMainWeapon();
		}
		else 
		{
			moveDirection = new Vector3(0, 0, 0);
		}
	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = reachedTargetPostion ? 
			new Vector2(0, 0) : new Vector2(moveSpeed * moveDirection.x, moveSpeed * moveDirection.y);
	}

	bool InControlsArea(Vector3 coordinates)
	{
		return ((coordinates.x > Screen.width * 0.75) && (coordinates.y < Screen.height* 0.25));
	}

}