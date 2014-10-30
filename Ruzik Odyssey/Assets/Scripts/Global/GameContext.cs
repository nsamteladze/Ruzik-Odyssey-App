using UnityEngine;
using RuzikOdyssey;
using Newtonsoft.Json;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Infrastructure
{
	public sealed class Lazy<T>
	{
		private T backingField;
		private bool isInitialized;
		private readonly Func<T> valueFactory;

		public T Value
		{
			get
			{
				if (!isInitialized)
				{
					backingField = valueFactory();
					isInitialized = true;
				}
				return backingField;
			}
			private set 
			{ 
				backingField = value; 
			}
		}

		public Lazy(Func<T> valueFactory)
		{
			this.valueFactory = valueFactory;
		}
	}

	public sealed class GameContext
	{
		public const string GameModelKey = "GameModel";
		public const string GameProgressKey = "GameProgress";

		public const string GameProgressDefaultsFile = "GameProgressDefaults.json";

		public static Lazy<TextAsset> gameProgressDefaults = 
			new Lazy<TextAsset>(() => Resources.Load(GameProgressDefaultsFile) as TextAsset);

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public T LoadDefauts<T>()
		{
			var defaultsFile = GetDefaultsFile<T>();

			if (defaultsFile == null) throw new UnityException("Failed to load determine defaults file");

			var defaults = JsonConvert.DeserializeObject<T>(defaultsFile.text);
				
			if (defaults == null) throw new UnityException("Failed to deserialize defaults from the config file.");

			return defaults;
		}

		public TextAsset GetDefaultsFile<T>()
		{
			if (typeof(T) == typeof(GameProgress)) return gameProgressDefaults.Value;
			else return null;
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

		public void SaveWithKey<T>(string key, T entity)
		{
			var json = JsonConvert.SerializeObject(entity);
			PlayerPrefs.SetString(key, json);
		}

		public T LoadForKey<T>(string key) where T : class
		{
			var json = PlayerPrefs.GetString(key);
			
			if (String.IsNullOrEmpty(json))
			{
				Log.Warning("Failed to load {0} from persistence storage", key);
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
	}

	public sealed class GameModelEntity
	{
		public int Gold { get; set; }
		public int Corn { get; set; }
		public int Gas { get; set; }
	}
}
