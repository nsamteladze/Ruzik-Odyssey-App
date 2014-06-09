using UnityEngine;

namespace RuzikOdyssey.Level
{
	public class ForceField : MonoBehaviour
	{
		public float speedDecrease = 0.5f;
		public float speedDecreaseTime = 3f;

		private void Start()
		{
			this.gameObject.rigidbody2D.velocity = Environment.ForegroundSpeed;
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag("Player"))
			{
				var slowDownEffect = otherCollider.gameObject.AddComponent<SlowDownEffect>();
				slowDownEffect.speedDecrease = speedDecrease;
				slowDownEffect.speedDecreaseTime = speedDecreaseTime;
			}
		}
	}
}
