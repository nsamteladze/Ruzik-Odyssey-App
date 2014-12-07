using System;
using RuzikOdyssey.Domain.Inventory;
using Newtonsoft.Json.Linq;
using RuzikOdyssey.Common;
using System.Linq;

namespace RuzikOdyssey.Infrastructure
{
	public class InventoryItemConverter : JsonCreationConverter<InventoryItem>
	{
		private const string InventoryItemCategoryKey = "category";

		protected override InventoryItem Create(Type objectType, JObject jObject)
		{
			switch (RetrieveCategory(jObject))
			{
				case InventoryItemCategory.Weapons: return new WeaponInventoryItem();
				case InventoryItemCategory.Ammo: return new AmmoInventoryItem();
				case InventoryItemCategory.Fuselage: return new ArmorInventoryItem();
				default: return new InventoryItem();
			}
		}

		private InventoryItemCategory RetrieveCategory(JObject jObject)
		{
			return jObject[InventoryItemCategoryKey].ToObject<InventoryItemCategory>();
		}

		private bool FieldExists(string fieldName, JObject jObject)
		{
			return jObject[fieldName] != null;
		}
	}
}
