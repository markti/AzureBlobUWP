using BlobExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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