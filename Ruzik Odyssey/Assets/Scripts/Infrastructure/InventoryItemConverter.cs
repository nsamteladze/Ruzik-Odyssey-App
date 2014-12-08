using System;
using RuzikOdyssey.Domain.Inventory;
using Newtonsoft.Json.Linq;
using RuzikOdyssey.Common;
using System.Linq;

namespace RuzikOdyssey.Infrastructure
{
	public class InventoryItemConverter : JsonCreationConverter<InventoryItem>
	{
#if UNITY_EDITOR
		private const string InventoryItemCategoryKey = "category";
#elif UNITY_IOS
		private const string InventoryItemCategoryKey = "Category";
#else
		private const string InventoryItemCategoryKey = "category";
#endif


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
			if (jObject[InventoryItemCategoryKey] == null)
			{
				Log.Error("Serialized object misses '{0}' key. Object: \n{1}", 
				          InventoryItemCategoryKey, jObject.ToString());
				return default(InventoryItemCategory);
			}

			return jObject[InventoryItemCategoryKey].ToObject<InventoryItemCategory>();
		}

		private bool FieldExists(string fieldName, JObject jObject)
		{
			return jObject[fieldName] != null;
		}
	}
}
