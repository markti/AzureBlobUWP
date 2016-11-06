using BlobExplorer.Model;

namespace BlobExplorer.Navigation
{
    public class BlobDetailNavigationContext
    {
        public AzureStorageAccount Account { get; set; }
        public AzureStorageContainer Container { get; set; }
        public AzureStorageBlob Blob { get; internal set; }
    }
}