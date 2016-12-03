using BlobExplorer.Model;

namespace BlobExplorer.Navigation
{
    public class ContainerEditorNavigationContext
    {
        public bool IsNew { get; set; }
        public AzureStorageAccount Account { get; set; }
        public AzureStorageContainer Container { get; set; }
    }
}