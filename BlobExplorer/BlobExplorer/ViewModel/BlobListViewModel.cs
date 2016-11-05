﻿using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
            InitializeClient();
            RefreshBlobs();
        }

        private void InitializeClient()
        {
            try
            {
                client = new AzureStorageClient(StorageAccount);
            }
            catch (Exception ex)
            {
                // do something
            }
        }
        private async Task RefreshBlobs()
        {
            try
            {
                var blobs = await client.GetBlobs(Container.Name, prefix);
                this.Blobs.Clear();
                foreach (var item in blobs)
                {
                    this.Blobs.Add(item);
                }
            }
            catch (Exception ex)
            {
                // do something
            }
        }

        public async Task SaveFile(StorageFile targetFile)
        {
            var firstItem = this.SelectedItems.FirstOrDefault();
            client.DownloadBlob(targetFile, firstItem.Uri);
        }
    }
}