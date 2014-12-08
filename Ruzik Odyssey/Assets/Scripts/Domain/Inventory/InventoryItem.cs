using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItem
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
		public string ShortName { get; set; }
		public string Description { get; set; }

		public string SpriteName { get; set; }
		public string ThumbnailName { get; set; }

		public int Weight { get; set; }
		public int Power { get; set; }
		public int Energy { get; set; }

		public int Level { get; set; }
		public int Class { get; set; }
		public InventoryItemCategory Category { get; set; }

		public InventoryItemPrice Price { get; set; }
		public InventoryItemRarity Rarity { get; set; }
	}
}
