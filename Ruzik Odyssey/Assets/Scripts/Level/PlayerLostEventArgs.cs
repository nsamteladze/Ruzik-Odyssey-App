using System;

namespace RuzikOdyssey.Level
{
	public sealed class PlayerLostEventArgs : EventArgs
	{
		public int LevelScore { get; set; }
	}
}
