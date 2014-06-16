using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RuzikOdyssey.Level
{
public static class EventBroker
{
	private static Dictionary<string, Delegate> subscriptions;

	static EventBroker()
	{
		subscriptions = new Dictionary<string, Delegate>();
	}

	public static void Publish<T>(string eventId, ref EventHandler<T> publishedEvent)
		where T : EventArgs
	{
		if (String.IsNullOrEmpty(eventId)) 
			throw new UnityException("Event can't be published with an empty event ID");

		publishedEvent += (sender, e) => 
		{
			if (sender == null) throw new UnityException("Event can't be published with no sender");
			if (e == null) throw new UnityException("Event can't be published with no event arguments");

			ProxyEventHandler<T>(eventId, sender, e);
		};
	}
	
	public static void Subscribe<T>(string eventId, EventHandler<T> eventHandler)
		where T : EventArgs
	{
		if (String.IsNullOrEmpty(eventId)) 
			throw new UnityException("Can't subscribe for an event with an empty event ID");
		if (eventHandler == null) throw new UnityException("Subscribing event handler can't be null");

		subscriptions.Add(eventId, eventHandler);
	}

	public static void ClearSubscribtions()
	{
		subscriptions.Clear();
	}

	private static void ProxyEventHandler<T>(string eventId, object sender, T e)
		where T : EventArgs
	{
		Delegate eventHandlerDelegate;
		if (!subscriptions.TryGetValue(eventId, out eventHandlerDelegate))
		{
			Debug.LogWarning(String.Format("Event {0} doesn't have any subscribers", eventId));
			return;
		}

		var eventHandler = eventHandlerDelegate as EventHandler<T>;
		if (eventHandler == null) 
		{
			Debug.LogError(String.Format("Event {0} subscriber has incorrect event arguments", eventId));
			return;
		}

		eventHandler.Invoke(sender, e);
	}
}
}
