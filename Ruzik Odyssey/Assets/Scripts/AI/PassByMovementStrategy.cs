using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public class PassByMovementStrategy : MovementStrategy
	{
		protected override UnityEngine.Vector2 CalculateMovementDirection (Vector2 currentPosition)
		{
			return new Vector2(-1, 0);
		} 

		public override Vector2 GetTargetPosition (Vector2 currentPosition, bool isInWarzone)
		{
			return new Vector2(-10, 0);
		}
	}
}
