using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class BlobListViewModel : ViewModelBase
    {
        private AzureStorageClient client;
        public AzureStorageAccount StorageAccount { get; set; }
        public AzureStorageContainer Container { get; set; }
        private string prefix;

        public ObservableCollection<AzureStorageBlob> Blobs { get; private set; }
        private AzureStorageBlob selectedBlob;

        public AzureStorageBlob SelectedBlob
        {
            get { return selectedBlob; }
            set { selectedBlob = value; this.RaisePropertyChanged(); }
        }


        public BlobListViewModel()
        {
            this.Blobs = new ObservableCollection<AzureStorageBlob>();
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
    }
}