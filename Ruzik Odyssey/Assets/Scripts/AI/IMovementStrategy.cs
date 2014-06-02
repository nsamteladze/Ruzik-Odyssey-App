using System;

namespace RuzikOdyssey.Ai
{
	public interface IMovementStrategy
	{
		float GetNextPosition(float currentPosition);
	}
}

