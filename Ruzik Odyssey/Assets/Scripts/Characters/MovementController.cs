using UnityEngine;

namespace RuzikOdyssey.Characters
{
	public abstract class MovementController : MonoBehaviour
	{
		public abstract void Move(Vector2 position, Vector2 deltaPosition);
		public abstract void Stop();
	}
}
