using UnityEngine;
using RuzikOdyssey;
using Newtonsoft.Json;
using System;
using RuzikOdyssey.Common;
using System.Linq;
using System.Collections.Generic;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Infrastructure
{
	public sealed class GameContext
	{
		public const string GameModelKey = "GameModel";

		private static Dictionary<string, Lazy<TextAsset>> defaultsAssets = new Dictionary<string, Lazy<TextAsset>>();

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public T LoadDefauts<T>()
		{
			var defaultsFile = GetDefaultsAsset<T>();

			if (defaultsFile == null) throw new UnityException("Failed to load defaults file");

			var defaults = JsonConvert.DeserializeObject<T>(defaultsFile.text);
				
			if (defaults == null) throw new UnityException("Failed to deserialize defaults from the config file.");

			return defaults;
		}

		private TextAsset GetDefaultsAsset<T>()
		{
			if (!defaultsAssets.ContainsKey(typeof(T).FullName))
			{
				defaultsAssets.Add(
					typeof(T).FullName, 
					new Lazy<TextAsset>(() => Resources.Load(typeof(T).Name + "Defaults") as TextAsset));
			}

			var defaultsAsset = defaultsAssets[typeof(T).FullName].Value; 

			return defaultsAsset;
		}

		public void SaveEntity<T>(T entity)
		{
			var json = JsonConvert.SerializeObject(entity);
			PlayerPrefs.SetString(typeof(T).FullName, json);
		}

		public T LoadEntity<T>() where T : class
		{
			var key = typeof(T).FullName;

			var json = PlayerPrefs.GetString(key);

			if (String.IsNullOrEmpty(json))
			{
				Log.Warning("Failed to load {0} from persistence storage. Loading defaults.", key);
				return LoadDefauts<T>();
			}
			
			var entity = JsonConvert.DeserializeObject<T>(json);
			
			if (entity == null)
			{
				Log.Error("Failed to parse {0}", key);
				return LoadDefauts<T>();
			}

			return entity;
		}

		public GameContent LoadGameContent()
		{
			var contentAsset = Resources.Load(GameConfig.GameContentFilePath) as TextAsset;

			if (contentAsset == null)
			{
				Log.Error("Failed to load game content from {0}", GameConfig.GameContentFilePath);
				return null;
			}

			GameContent content = null;
			try
			{
				content = JsonConvert.DeserializeObject<GameContent>(contentAsset.text);
			}
			catch (Exception ex)
			{
				Log.Error("An exeption occured while deserializing game content. Exception: {0}", ex.Message);
			}

			if (content == null) 
			{
				Log.Error("Failed to desirialize game content");
			}

			return content;
		}
	}
}
