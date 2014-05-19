using UnityEngine;
using System.Collections;

public class PlayerDeltaMovement : MonoBehaviour
{
	public float movementSpeed = 1;

	private PlayerWeaponsController weapon;
	Vector2 movementDirection;

	void Start()
	{
		weapon = GetComponent<PlayerWeaponsController>();
		if (weapon == null) 
		{
			throw new UnityException("Failed to aquire player's weapon");
		}
	}

	void Update () 
	{	
		movementDirection = new Vector2(0, 0);

		if (Input.touchCount == 0) return;

		if (Input.touchCount > 1) 
		{
			Debug.Log("More than 1 touch detected");
			return;
		}

		Debug.Log(string.Format("Detected {0} touches", Input.touchCount));

		Touch movementTouch = Input.GetTouch(0);

		if ((movementTouch.phase == TouchPhase.Canceled) || 
		    (movementTouch.phase == TouchPhase.Ended))
		{
			Debug.Log("Touch ended or was calceled");
			return;
		}

		weapon.AttackWithMainWeapon();

		if (movementTouch.phase == TouchPhase.Moved)
		{
			movementDirection = movementTouch.deltaPosition;
		}
	}
	
	void FixedUpdate()
	{
		rigidbody2D.velocity = new Vector2(movementSpeed * movementDirection.x, 
		                                   movementSpeed * movementDirection.y);
	}
	
	bool InControlsArea(Vector3 coordinates)
	{
		return ((coordinates.x > Screen.width * 0.75) && (coordinates.y < Screen.height* 0.25));
	}
}

