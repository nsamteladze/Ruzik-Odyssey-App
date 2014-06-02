using System;
using UnityEngine;

namespace RuzikOdyssey
{
	public interface ITouchButton
	{
		void Touch();
		bool HitTest(Vector2 position);
	}
}

