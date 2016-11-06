using BlobExplorer.Common;
using GalaSoft.MvvmLight;
using System;

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
        public string Parent { get; set; }
        public string Container { get; set; }
        public string ETag { get; set; }
        public string ContentDisposition { get; internal set; }
        public string ContentEncoding { get; internal set; }
        public string ContentLanguage { get; internal set; }
        public string ContentMD5 { get; internal set; }
        public string ContentType { get; internal set; }
        public string CacheControl { get; internal set; }
        public int StreamMinimumReadSizeInBytes { get; internal set; }
        public int StreamWriteSizeInBytes { get; internal set; }
        public string LeaseStatus { get; internal set; }
        public string LeaseState { get; internal set; }
        public string LeaseDuration { get; internal set; }
        public Uri SnapshotQualifiedUri { get; internal set; }
        public Uri SnapshotQualifiedStorageUri { get; internal set; }
        public DateTime SnapshotTime { get; internal set; }
        public bool IsSnapshot { get; internal set; }
    }
}