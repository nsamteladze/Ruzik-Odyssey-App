using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public class PassByMovementStrategy : MovementStrategy
	{
		protected override UnityEngine.Vector2 CalculateMovementDirection (Vector2 currentPosition)
		{
			return new Vector2(-1, 0);
		} 
	}
}
