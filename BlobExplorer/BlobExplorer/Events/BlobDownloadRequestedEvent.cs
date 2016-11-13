using BlobExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BlobExplorer.Events
{
    public class BlobDownloadRequestedEvent
    {
        public string FileName { get; internal set; }
        public string FullPath { get; internal set; }
        public AzureStorageBlob Source { get; set; }
        public StorageFile Target { get; set; }
    }
}