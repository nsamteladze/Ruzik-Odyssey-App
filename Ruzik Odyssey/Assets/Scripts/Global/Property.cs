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
			get { return this.value; }
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
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs<T>(Value));
		}

		public void PublishEvents()
		{
			EventBroker.Publish<PropertyChangedEventArgs<T>>(
				String.Format("{0}_PropertyChanged", this.name), 
				ref PropertyChanged);
		}
	}

	public static class Properties
	{
		public static class Global
		{
			public const string Gold = "Global_Gold";
			public const string Corn = "Global_Corn";
			public const string Gas = "Global_Gas";
			public const string CurrentLevelIndex = "Global_CurrentLevelIndex";
			public const string CurrentLevelDifficulty = "Global_CurrentLevelDifficulty";
		}
	}

}