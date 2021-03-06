﻿using BlobExplorer.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BlobExplorer.Selectors
{
    public class BlobItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderItemTemplate { get; set; }
        public DataTemplate BlobItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var blobItem = item as AzureStorageBlob;

            if(blobItem.IsDirectory)
            {
                return FolderItemTemplate;
            }
            else
            {
                return BlobItemTemplate;
            }
        }
    }
}