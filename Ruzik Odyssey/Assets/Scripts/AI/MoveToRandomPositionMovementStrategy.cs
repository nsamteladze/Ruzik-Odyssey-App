using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public class MoveToRandomPositionMovementStrategy : MovementStrategy
	{
		private Vector2 targetPosition;

		private float allowedTargetPositionDistanceError = 0.1f;

		private void Start()
		{
			targetPosition = new Vector2(Random.Range(-1f, 6.5f), Random.Range(-3.5f, 3f));
		}

		protected override Vector2 CalculateMovementDirection(Vector2 currentPosition)
		{
			return (Vector2.Distance(currentPosition, targetPosition) > allowedTargetPositionDistanceError)
				? (targetPosition - currentPosition).normalized
				: Vector2.zero;
		}

		public override Vector2 GetTargetPosition (Vector2 currentPosition, bool isInWarzone)
		{
			return targetPosition;
		}
	}
}
