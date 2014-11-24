using System;

namespace RuzikOdyssey.Common
{
	public class ProgressUpdatedEventsArgs : EventArgs
	{
		public string ActionName { get; set; }
		public int ActionProgress { get; set; }
	}
}
