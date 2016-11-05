using BlobExplorer.Common;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobExplorer.Model
{
    public class AzureStorageBlob : ViewModelBase
    {
        public bool IsDirectory { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        private long sizeInBytes;
        public long LengthInBytes
        {
            get { return sizeInBytes; }
            set
            {
                sizeInBytes = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => LengthDescription);
            }
        }
        public string LengthDescription
        {
            get { return string.Format(new FileSizeFormatProvider(), "{0:fs}", LengthInBytes); }
        }
        public Uri Uri { get; set; }
        public string BlobType { get; set; }
        public DateTime LastModified { get; set; }
        public string Parent { get; internal set; }
        public string Container { get; internal set; }
    }
}