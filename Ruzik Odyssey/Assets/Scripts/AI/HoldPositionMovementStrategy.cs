using System;

namespace RuzikOdyssey.Ai
{
	public class HoldPositionMovementStrategy : IMovementStrategy
	{
		public float GetNextPosition (float currentPosition)
		{
			return currentPosition;
		}
	}
}

