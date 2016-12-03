using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobExplorer.Model
{
    public class ContainerAccessLevel
    {
        public BlobContainerPublicAccessType Code { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}