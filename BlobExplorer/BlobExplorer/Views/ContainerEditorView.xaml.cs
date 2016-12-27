using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
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
    public sealed partial class ContainerEditorView : Page
    {
        ContainerEditorViewModel viewModel;

        public ContainerEditorView()
        {
            this.InitializeComponent();

            this.viewModel = new ContainerEditorViewModel();
            this.DataContext = viewModel;
        }

        DateTime startRequest;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HockeyClient.Current.TrackPageView("ContainerEditor");
            startRequest = DateTime.Now;

            viewModel.OnNavigatedTo(e.Parameter as ContainerEditorNavigationContext);
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "Container_Save";
            et.Properties.Add("Verb", viewModel.IsNew ? "CREATE" : "UPDATE");

            var result = await viewModel.Save();
            if (result)
            {
                et.Properties.Add("Completed", "TRUE");
                this.Frame.GoBack();
            }
            else
            {
                et.Properties.Add("Completed", "FALSE");
                // something broke
            }

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "Container_Save";
            et.Properties.Add("Verb", viewModel.IsNew ? "CREATE" : "UPDATE");
            et.Properties.Add("Completed", "FALSE");
            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);

            this.Frame.GoBack();
        }
    }
}