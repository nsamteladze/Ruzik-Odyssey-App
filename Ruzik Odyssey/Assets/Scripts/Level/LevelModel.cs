namespace RuzikOdyssey.Level
{
	internal sealed class LevelModel
	{
		public Property<int> Gold { get; set; }

		public LevelModel()
		{
			Gold = new Property<int>("Level_Gold");

			Gold.PublishEvents();
		}
	}
}
