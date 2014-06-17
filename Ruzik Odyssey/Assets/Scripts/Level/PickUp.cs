using UnityEngine;

namespace RuzikOdyssey.Level
{
	public class PickUp : MonoBehaviour
	{
		public int amount = 10;
		public PickUpType type = PickUpType.Health;
		
		public void Start()
		{
			this.gameObject.rigidbody2D.velocity = Environment.ForegroundSpeed;
		}
	}

	public enum PickUpType
	{
		Weapon = 1,
		Health = 2,
	}
}

