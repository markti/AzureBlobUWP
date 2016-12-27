using BlobExplorer.Events;
using BlobExplorer.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.HockeyApp;
using Microsoft.HockeyApp.DataContracts;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewStorageAccountView : Page
    {
        NewStorageAccountViewModel viewModel;

        public NewStorageAccountView()
        {
            this.InitializeComponent();

            viewModel = new NewStorageAccountViewModel();
            this.DataContext = viewModel;
        }

        DateTime startRequest;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            startRequest = DateTime.Now;

            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Add Storage Account" });
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "StorageAccount_Save";
            et.Properties.Add("Completed", "TRUE");

            await viewModel.Save();

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);

            this.Frame.GoBack();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "StorageAccount_Save";
            et.Properties.Add("Completed", "TRUE");

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);

            this.Frame.GoBack();
        }
    }
}