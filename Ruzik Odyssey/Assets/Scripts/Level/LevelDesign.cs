using System.Collections.Generic;

namespace RuzikOdyssey.Level
{
	public sealed class LevelDesign
	{
		public IList<WaveTemplate> Enemies { get; set; }
		public IList<WaveTemplate> Obstacles { get; set; }
	}
}
