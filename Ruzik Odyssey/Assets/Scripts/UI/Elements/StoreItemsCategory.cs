using System.Collections.Generic;

namespace RuzikOdyssey.UI.Elements
{
	public sealed class StoreItemsCategory
	{
		public string Name { get; set; }
		public IList<StoreItem> Items { get; set; }
	}
}
