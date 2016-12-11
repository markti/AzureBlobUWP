using BlobExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobExplorer.Events
{
    public class BlobDownloadRequestedEvent
    {
        public BlobContainerPublicAccessType AccessLevel { get; internal set; }
        public string FileName { get; internal set; }
        public string FullPath { get; internal set; }
        public AzureStorageBlob Source { get; set; }
        public AzureStorageAccount StorageAccount { get; internal set; }
        public StorageFile Target { get; set; }
    }
}