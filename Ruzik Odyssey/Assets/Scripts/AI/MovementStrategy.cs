using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public abstract class MovementStrategy : MonoBehaviour
	{
		public Vector2 GetMovementDirection(Vector2 currentPosition, bool isInWarzone)
		{
			if (!isInWarzone) return new Vector2(-1, 0);
			return CalculateMovementDirection(currentPosition);
		}

		protected abstract Vector2 CalculateMovementDirection(Vector2 currentPosition);
	}
}

