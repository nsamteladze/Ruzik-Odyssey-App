using UnityEngine;
using System;

namespace RuzikOdyssey.Common
{
	public static class GameObjectExtensions
	{
		public static Vector2 RendererSize(this GameObject gameObject)
		{
			return gameObject.renderer.bounds.size;
		}

		public static T GetComponentOrThrow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null) throw new Exception(String.Format("GameObject {0} is missing a mandatory component {1}",
			                                                         gameObject.name, typeof(T).Name));
			
			return component;
		}

		public static T GetComponentInChildrenOrThrow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponentInChildren<T>();
			if (component == null) throw new Exception(String.Format("GameObject {0}'s children are missing a mandatory component {1}",
			                                                         gameObject.name, typeof(T).Name));
			
			return component;
		}

		public static GameObject FindOrThrow(string name)
		{
			var gameObject = GameObject.Find(name);
			if (gameObject == null) 
				throw new Exception(String.Format("Failed to find game object named {0} in the hierarchy", name));

			return gameObject;
		}
	}
}
