using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using System;

namespace RuzikOdyssey.ViewModels
{
	public class StartSceneViewModel : ExtendedMonoBehaviour
	{
		public event EventHandler<ProgressUpdatedEventsArgs> LoadingProgressUpdated;
		public event EventHandler<EventArgs> LoadingFinished;

		private void Awake()
		{
			SubscribeToEvents();
		}

		private void Start()
		{
			StartCoroutine(GlobalModel.InitializeAsync());
		}

		private void OnDestroy()
		{
			UnsubscribeFromEvents();
		}

		private void SubscribeToEvents()
		{
			EventsBroker.Subscribe<ProgressUpdatedEventsArgs>(
				Events.Global.ModelLoadingProgressUpdated, 
				GameModel_LoadingProgressUpdate); 

			EventsBroker.Subscribe<EventArgs>(Events.Global.ModelLoadingFinished, GameModel_LoadingFinished);
		}

		private void UnsubscribeFromEvents()
		{
			EventsBroker.Unsubscribe<ProgressUpdatedEventsArgs>(
				Events.Global.ModelLoadingProgressUpdated, 
				GameModel_LoadingProgressUpdate); 
			
			EventsBroker.Unsubscribe<EventArgs>(Events.Global.ModelLoadingFinished, GameModel_LoadingFinished);
		}

		private void OnLoadingProgressUpdated(ProgressUpdatedEventsArgs e)
		{
			if (LoadingProgressUpdated != null) LoadingProgressUpdated(this, e);
		}

		private void OnLoadingFinished(EventArgs e)
		{
			if (LoadingFinished != null) LoadingFinished(this, EventArgs.Empty);
		}

		private void GameModel_LoadingProgressUpdate(object sender, ProgressUpdatedEventsArgs e)
		{
			OnLoadingProgressUpdated(e);
		}

		private void GameModel_LoadingFinished(object sender, EventArgs e)
		{
			OnLoadingFinished(e);
		}
	}
}
