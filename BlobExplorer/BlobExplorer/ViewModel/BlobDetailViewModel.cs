using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System;
using Windows.ApplicationModel.DataTransfer;

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
                client = new AzureStorageClient(StorageAccount);
        }

        public void CopyBlobUrlToClipboard()
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(this.blob.Uri.AbsoluteUri);
            // Set the DataPackage to clipboard.
            Clipboard.SetContent(dataPackage);
        }

        public async Task Refresh()
        {
            var fullDetail = await client.GetBlobDetail(this.Blob);
            this.Blob = fullDetail;
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.Blob.Name });
        }
    }
}