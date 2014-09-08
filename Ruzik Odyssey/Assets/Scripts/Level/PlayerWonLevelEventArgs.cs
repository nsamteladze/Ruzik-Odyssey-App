using System;

namespace RuzikOdyssey.Level
{
	public sealed class PlayerWonLevelEventArgs : EventArgs
	{
		public int TotalLevelScore { get; set; }
	}
}
