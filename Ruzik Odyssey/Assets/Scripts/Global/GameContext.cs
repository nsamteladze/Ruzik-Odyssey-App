using UnityEngine;
using RuzikOdyssey;
using Newtonsoft.Json;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Infrastructure
{
	public sealed class GameContext
	{
		private const string GameModelKey = "GameModel";

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public GameModelEntity LoadGameModelEntity()
		{
			var json = PlayerPrefs.GetString(GameModelKey);

			if (String.IsNullOrEmpty(json))
			{
				Log.Error("Failed to load {0} from persistence storage", GameModelKey);
				return null;
			}

			var entity = JsonConvert.DeserializeObject<GameModelEntity>(json);

			if (entity == null)
			{
				Log.Error("Failed to parse {0}", GameModelKey);
				return null;
			}

			return entity;
		}

		public void SaveGameModelEntity(GameModelEntity entity)
		{
			var json = JsonConvert.SerializeObject(entity);
			PlayerPrefs.SetString(GameModelKey, json);
		}
	}

	public sealed class GameModelEntity
	{
		public int Gold { get; set; }
		public int Corn { get; set; }
		public int Gas { get; set; }
	}
}
