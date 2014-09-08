using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey
{

	public class Property<T>
	{
		private T value;
		private string name;

		public T Value 
		{ 
			get 
			{ 
				return this.value; 
			}
			set 
			{
				this.value = value;
				OnPropertyChanged();
			}
		}

		public event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

		public Property(string name)
		{
			this.name = name;

			PublishEvents();
		}

		protected virtual void OnPropertyChanged()
		{
			if (PropertyChanged != null) 
			{
				Log.Debug("OnPropertyChanged {0}", Value);
				PropertyChanged(this, new PropertyChangedEventArgs<T>(Value));
			}
		}

		public void PublishEvents()
		{
			EventBroker.Publish<PropertyChangedEventArgs<T>>(
				String.Format("{0}_PropertyChanged", this.name), 
				ref PropertyChanged);
		}
	}

}