using BlobExplorer.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BlobExplorer.Selectors
{
    public class BlobItemPreviewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderPreviewTemplate { get; set; }
        public DataTemplate FilePreviewTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var blobItem = item as AzureStorageBlob;

            if (blobItem.IsDirectory)
            {
                return FolderPreviewTemplate;
            }
            else
            {
                return FilePreviewTemplate;
            }
        }
    }
}