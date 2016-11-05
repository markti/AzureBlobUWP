using BlobExplorer.Common;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Model
{
    public class AzureStorageBlob : ViewModelBase
    {
        public bool IsDirectory { get; internal set; }
        public string Name { get; set; }
        public string Path { get; internal set; }
        private long sizeInBytes;
        public long SizeInBytes
        {
            get { return sizeInBytes; }
            set
            {
                sizeInBytes = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => FileSize);
            }
        }
        public string FileSize
        {
            get { return string.Format(new FileSizeFormatProvider(), "{0:fs}", SizeInBytes); }
        }

        public Uri Uri { get; internal set; }
    }
}