using BlobExplorer.Model;

namespace BlobExplorer.Navigation
{
    public class BlobListNavigationContext
    {
        public AzureStorageAccount Account { get; set; }
        public AzureStorageContainer Container { get; set; }
        public string BlobPrefix { get; set; }
    }
}