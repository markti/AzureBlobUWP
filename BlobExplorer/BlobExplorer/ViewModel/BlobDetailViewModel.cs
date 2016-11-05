using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class BlobDetailViewModel : ViewModelBase
    {
        private AzureStorageClient client;
        public AzureStorageAccount StorageAccount { get; set; }
        public AzureStorageContainer Container { get; set; }
        AzureStorageBlob blob;
        public AzureStorageBlob Blob
        {
            get { return blob; }
            set { blob = value;  RaisePropertyChanged(); }
        }

        internal void OnNavigatedTo(BlobDetailNavigationContext context)
        {
            this.StorageAccount = context.Account;
            this.Container = context.Container;
            this.Blob = context.Blob;
            InitializeClient();
            Refresh();
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
        public async Task Refresh()
        {
            var fullDetail = await client.GetBlobDetail(this.Blob);
            this.Blob = fullDetail;
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.Blob.Name });
        }
    }
}