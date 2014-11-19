using UnityEngine;
using RuzikOdyssey;
using Newtonsoft.Json;
using System;
using RuzikOdyssey.Common;
using System.Linq;
using System.Collections.Generic;

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
	}
}
