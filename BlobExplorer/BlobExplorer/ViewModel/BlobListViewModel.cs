﻿using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace BlobExplorer.ViewModel
{
    public class BlobListViewModel : ViewModelBase
    {
        private AzureStorageClient client;
        public AzureStorageAccount StorageAccount { get; set; }
        public AzureStorageContainer Container { get; set; }
        private string prefix;

        public ObservableCollection<AzureStorageBlob> Blobs { get; private set; }
        public ObservableCollection<AzureStorageBlob> SelectedItems { get; set; }
        private bool canDownloadItem;

        public bool CanDownload
        {
            get { return canDownloadItem; }
            set { canDownloadItem = value; RaisePropertyChanged(); }
        }

        public BlobListViewModel()
        {
            this.Blobs = new ObservableCollection<AzureStorageBlob>();
            this.SelectedItems = new ObservableCollection<AzureStorageBlob>();
            this.SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var firstItem = SelectedItems.FirstOrDefault();
            CanDownload = SelectedItems.Count == 1 && firstItem != null && !firstItem.IsDirectory;
        }

        public void OnNavigatedTo(BlobListNavigationContext context)
        {
            this.StorageAccount = context.Account;
            this.Container = context.Container;
            this.prefix = context.BlobPrefix;
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.Container.Name });
            InitializeClient();
            RefreshBlobs();
        }

        private void InitializeClient()
        {
            client = new AzureStorageClient(StorageAccount);
        }
        private async Task RefreshBlobs()
        {
            var blobs = await client.GetBlobs(Container.Name, prefix);
            this.Blobs.Clear();
            foreach (var item in blobs)
            {
                this.Blobs.Add(item);
            }
        }

        public async Task DownloadFile(StorageFile targetFile)
        {
            var firstItem = this.SelectedItems.FirstOrDefault();
            // don't do this anymore!
            //client.DownloadBlob(targetFile, firstItem.Uri);

            var payload = new BlobDownloadRequestedEvent();
            payload.Target = targetFile;
            payload.Source = firstItem;
            payload.FileName = firstItem.Name;
            payload.FullPath = firstItem.Uri.AbsoluteUri;

            Messenger.Default.Send<BlobDownloadRequestedEvent>(payload);
        }

        public async Task UploadFile(StorageFile sourceFile)
        {
            client.UploadBlob(sourceFile, this.Container.Name, this.prefix);
        }
    }
}