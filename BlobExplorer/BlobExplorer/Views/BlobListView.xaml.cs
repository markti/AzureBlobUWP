using BlobExplorer.Common;
using BlobExplorer.Model;
using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
using Microsoft.HockeyApp;
using Microsoft.HockeyApp.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlobListView : Page
    {
        BlobListViewModel viewModel;

        public BlobListView()
        {
            this.InitializeComponent();

            viewModel = new BlobListViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel.OnNavigatedTo(e.Parameter as BlobListNavigationContext);
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AzureStorageBlob;

            if (item.IsDirectory)
            {
                var context = new BlobListNavigationContext();
                context.Account = viewModel.StorageAccount;
                context.BlobPrefix = item.Path;
                context.Container = viewModel.Container;

                this.Frame.Navigate(typeof(BlobListView), context);
            }
            else
            {
            }
        }

        private void GridView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            var item = element.DataContext as AzureStorageBlob;

            if (item.IsDirectory)
            {
                var context = new BlobListNavigationContext();
                context.Account = viewModel.StorageAccount;
                context.BlobPrefix = item.Path;
                context.Container = viewModel.Container;

                this.Frame.Navigate(typeof(BlobListView), context);
            }
            else
            {
                var context = new BlobDetailNavigationContext();
                context.Account = viewModel.StorageAccount;
                context.Blob = item;
                context.Container = viewModel.Container;

                this.Frame.Navigate(typeof(BlobDetailView), context);
            }
        }

        private async void DownloadBlobClicked(object sender, RoutedEventArgs e)
        {
            await DoDownload();
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedItems.Clear();
            foreach(var item in GridView.SelectedItems)
            {
                var typedItem = item as AzureStorageBlob;
                viewModel.SelectedItems.Add(typedItem);
            }
        }

        private async Task DoDownload()
        {
            var firstItem = viewModel.SelectedItems.FirstOrDefault();

            var savePicker = new FileSavePicker();
            savePicker.SuggestedFileName = firstItem.Name;
            savePicker.FileTypeChoices.Add("All Files", new List<string> { "." });

            var targetFile = await savePicker.PickSaveFileAsync();
            if(targetFile != null)
            {
                await viewModel.DownloadFile(targetFile);
            }
        }

        private void UploadBlobClicked(object sender, RoutedEventArgs e)
        {
            DoUpload();
        }

        private async Task DoUpload()
        {
            var et = new EventTelemetry();
            et.Name = "Blob_Upload";
            var startRequest = DateTime.Now;

            var openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add("*");

            var sourceFile = await openPicker.PickSingleFileAsync();
            if(sourceFile != null)
            {
                var fileInfo = await sourceFile.GetBasicPropertiesAsync();
                et.Properties.Add("FileSize", string.Format(new FileSizeFormatProvider(), "{0:fs}", fileInfo.Size));
                et.Properties.Add("Confirmed", "TRUE");
                await viewModel.UploadFile(sourceFile);
                et.Properties.Add("Completed", "TRUE");
            }
            else
            {
                et.Properties.Add("Confirmed", "FALSE");
                et.Properties.Add("Completed", "FALSE");
            }

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);
        }

        private async void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "BlobList_Refresh";
            var startRequest = DateTime.Now;

            await viewModel.Refresh();

            et.Properties.Add("ItemCount", viewModel.Blobs.Count.ToString());

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);
        }

        private async void OnDeleteBlobsClick(object sender, RoutedEventArgs e)
        {
            var et = new EventTelemetry();
            et.Name = "Blob_Delete";
            et.Properties.Add("ItemCount", viewModel.SelectedItems.Count.ToString());
            var startRequest = DateTime.Now;

            var affirmateev = new UICommand("Yes") { Id = 1 };
            var nein = new UICommand("No") { Id = 0 };
            var dialog = new Windows.UI.Popups.MessageDialog("You are about to delete a blob from your Azure Storage Account. This action cannot be undone.", "Are you sure?");
            dialog.Commands.Add(affirmateev);
            dialog.Commands.Add(nein);
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var result = await dialog.ShowAsync();

            if (result.Equals(affirmateev))
            {
                et.Properties.Add("Confirmed", "TRUE");

                // DELETE!
                await viewModel.DeleteSelectedBlobs();

                et.Properties.Add("Completed", "TRUE");
            }
            else
            {
                // ABORT!
                et.Properties.Add("Confirmed", "FALSE");
                et.Properties.Add("Completed", "FALSE");
            }

            var finishRequest = DateTime.Now;
            var diffInSeconds = (finishRequest - startRequest).TotalSeconds;
            et.Metrics.Add("Duration", diffInSeconds);
            HockeyClient.Current.TrackEvent(et);
        }
    }
}