using BlobExplorer.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading;

namespace BlobExplorer
{
    public class AzureStorageClient
    {
        private CloudBlobClient blobClient;
        private CloudStorageAccount storageAccount;

        public AzureStorageClient(AzureStorageAccount account)
        {
            var credentials = new StorageCredentials(account.Name, account.Key);
            this.storageAccount = new CloudStorageAccount(credentials, true);
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        public async Task<List<AzureStorageContainer>> GetContainers()
        {
            var list = new List<AzureStorageContainer>();

            var prefix = string.Empty;
            var continuationToken = new BlobContinuationToken();
            var requestOptions = new BlobRequestOptions();
            var cancelToken = new CancellationToken();
            var opContext = new OperationContext();

            var operationOutcome = await blobClient.ListContainersSegmentedAsync(prefix, ContainerListingDetails.All, 1000, continuationToken, requestOptions, opContext, cancelToken);

            foreach(var item in operationOutcome.Results)
            {
                var container = new AzureStorageContainer();
                container.Name = item.Name;

                list.Add(container);
            }

            return list;
        }

        public async Task<List<AzureStorageBlob>> GetBlobs(string containerName, string prefix)
        {
            var list = new List<AzureStorageBlob>();
            
            var flatBlobs = false;
            var continuationToken = new BlobContinuationToken();
            var requestOptions = new BlobRequestOptions();
            var cancelToken = new CancellationToken();
            var opContext = new OperationContext();

            var container = blobClient.GetContainerReference(containerName);

            var operationOutcome = await container.ListBlobsSegmentedAsync(prefix, flatBlobs, BlobListingDetails.Metadata, 1000, continuationToken, requestOptions, opContext);

            foreach (var item in operationOutcome.Results)
            {
                var newBlobItem = new AzureStorageBlob();

                if (item is CloudBlobDirectory)
                {
                    var blobDir = item as CloudBlobDirectory;
                    var folderName = blobDir.Prefix.Substring(prefix.Length, blobDir.Prefix.Length - prefix.Length);
                    newBlobItem.Name = folderName.Replace("/", "");
                    newBlobItem.Path = blobDir.Prefix;
                    newBlobItem.IsDirectory = true;
                }
                else if(item is ICloudBlob)
                {
                    var blobItem = item as ICloudBlob;
                    var fileName = blobItem.Name.Substring(prefix.Length, blobItem.Name.Length - prefix.Length);
                    newBlobItem.Name = fileName;
                    newBlobItem.Path = blobItem.Name;
                    newBlobItem.IsDirectory = false;
                }

                list.Add(newBlobItem);
            }

            return list;
        }
    }
}