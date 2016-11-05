using BlobExplorer.Model;
using BlobExplorer.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class BlobDetailViewModel
    {
        private AzureStorageClient client;
        public AzureStorageAccount StorageAccount { get; set; }
        public AzureStorageContainer Container { get; set; }
        public AzureStorageBlob Blob { get; set; }


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
            var fullDetail = client.GetBlobDetail(this.Blob);
        }
    }
}