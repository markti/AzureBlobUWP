using BlobExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Navigation
{
    public class BlobDetailNavigationContext
    {
        public AzureStorageAccount Account { get; set; }
        public AzureStorageContainer Container { get; set; }
        public AzureStorageBlob Blob { get; internal set; }
    }
}