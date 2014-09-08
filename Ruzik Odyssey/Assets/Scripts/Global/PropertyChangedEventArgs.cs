using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey
{

	public class PropertyChangedEventArgs<T> : EventArgs
	{
		public T PropertyValue { get; set; }

		public PropertyChangedEventArgs(T value)
		{
			this.PropertyValue = value;
		}
	}

}