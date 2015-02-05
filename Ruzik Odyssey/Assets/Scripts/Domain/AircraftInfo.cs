namespace RuzikOdyssey.Domain
{
	public class AircraftInfo
	{
		public AircraftUiInfo Ui { get; set; }

		public override string ToString ()
		{
			return string.Format ("[AircraftInfo: Ui={0}]", Ui);
		}

		public class AircraftUiInfo 
		{
			public string SceneSpriteName { get; set; }
			
			public override string ToString ()
			{
				return string.Format ("[AircraftUiInfo: SceneSpriteName={0}]", SceneSpriteName);
			}
		}
	}
}
