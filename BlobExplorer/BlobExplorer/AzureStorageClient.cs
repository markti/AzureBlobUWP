﻿using BlobExplorer.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace BlobExplorer
{
    public class AzureStorageClient
    {
        private CloudBlobClient blobClient;
        private CloudStorageAccount storageAccount;

        public AzureStorageClient(AzureStorageAccount account)
        {
            var credentials = new StorageCredentials(account.Name, account.Key);
            this.storageAccount = new CloudStorageAccount(credentials, true);
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        public async Task<List<AzureStorageContainer>> GetContainers()
        {
            var list = new List<AzureStorageContainer>();

            var prefix = string.Empty;
            var continuationToken = new BlobContinuationToken();
            var requestOptions = new BlobRequestOptions();
            var cancelToken = new CancellationToken();
            var opContext = new OperationContext();

            var operationOutcome = await blobClient.ListContainersSegmentedAsync(prefix, ContainerListingDetails.All, 1000, continuationToken, requestOptions, opContext, cancelToken);

            foreach(var item in operationOutcome.Results)
            {
                var container = new AzureStorageContainer();
                container.Name = item.Name;

                var accessLevel = await GetAccessLevel(container);
                container.AccessLevel = new ContainerAccessLevel() { Code = accessLevel };

                list.Add(container);
            }

            return list;
        }

        public async Task<List<AzureStorageBlob>> GetBlobs(string containerName, string prefix)
        {
            var list = new List<AzureStorageBlob>();
            
            var flatBlobs = false;
            var continuationToken = new BlobContinuationToken();
            var requestOptions = new BlobRequestOptions();
            var cancelToken = new CancellationToken();
            var opContext = new OperationContext();

            var container = blobClient.GetContainerReference(containerName);

            var operationOutcome = await container.ListBlobsSegmentedAsync(prefix, flatBlobs, BlobListingDetails.Metadata, 1000, continuationToken, requestOptions, opContext);

            foreach (var item in operationOutcome.Results)
            {
                var newBlobItem = new AzureStorageBlob();

                if (item is CloudBlobDirectory)
                {
                    var blobDir = item as CloudBlobDirectory;
                    var folderName = blobDir.Prefix.Substring(prefix.Length, blobDir.Prefix.Length - prefix.Length);
                    newBlobItem.Name = folderName.Replace("/", "");
                    newBlobItem.Path = blobDir.Prefix;
                    newBlobItem.IsDirectory = true;
                }
                else if(item is ICloudBlob)
                {
                    
                    var blobItem = item as ICloudBlob;
                    newBlobItem.IsDirectory = false;
                    newBlobItem.Uri = blobItem.Uri;
                    newBlobItem = await GetBlobDetail(newBlobItem);
                    var fileName = blobItem.Name.Substring(prefix.Length, blobItem.Name.Length - prefix.Length);
                    newBlobItem.Name = fileName;
                    newBlobItem.Path = blobItem.Name;
                }

                list.Add(newBlobItem);
            }

            return list;
        }

        public async Task UploadBlob(StorageFile targetFile, string containerName, string prefix)
        {
            var container = blobClient.GetContainerReference(containerName);
            var fullUrl = container.Uri.AbsolutePath + prefix + targetFile.Name;
            var blobUri = new Uri(fullUrl);
            var blobRef = await blobClient.GetBlobReferenceFromServerAsync(blobUri);
            blobRef.UploadFromFileAsync(targetFile);
        }

        public async Task DownloadBlob(StorageFile targetFile, Uri blobUri)
        {
            var blobRef = await blobClient.GetBlobReferenceFromServerAsync(blobUri);
            blobRef.DownloadToFileAsync(targetFile);
        }

        public async Task<AzureStorageBlob> GetBlobDetail(AzureStorageBlob blob)
        {
            var blobRef = await blobClient.GetBlobReferenceFromServerAsync(blob.Uri);

            var fullDetailBlob = new AzureStorageBlob();

            fullDetailBlob.BlobType = blobRef.BlobType.ToString();
            fullDetailBlob.Name = blobRef.Name;
            fullDetailBlob.LastModified = blobRef.Properties.LastModified.Value.UtcDateTime;
            fullDetailBlob.LengthInBytes = blobRef.Properties.Length;
            fullDetailBlob.Parent = blobRef.Parent.Prefix;
            fullDetailBlob.Container = blobRef.Container.Name;
            fullDetailBlob.ETag = blobRef.Properties.ETag;
            fullDetailBlob.Uri = blobRef.Uri;
            // content
            fullDetailBlob.StreamMinimumReadSizeInBytes = blobRef.StreamMinimumReadSizeInBytes;
            fullDetailBlob.StreamWriteSizeInBytes = blobRef.StreamWriteSizeInBytes;
            fullDetailBlob.CacheControl = blobRef.Properties.CacheControl;
            fullDetailBlob.ContentDisposition = blobRef.Properties.ContentDisposition;
            fullDetailBlob.ContentEncoding = blobRef.Properties.ContentEncoding;
            fullDetailBlob.ContentLanguage = blobRef.Properties.ContentLanguage;
            fullDetailBlob.ContentMD5 = blobRef.Properties.ContentMD5;
            fullDetailBlob.ContentType = blobRef.Properties.ContentType;
            // lease
            fullDetailBlob.LeaseStatus = blobRef.Properties.LeaseStatus.ToString();
            fullDetailBlob.LeaseState = blobRef.Properties.LeaseState.ToString();
            fullDetailBlob.LeaseDuration = blobRef.Properties.LeaseDuration.ToString();
            // snapshot
            fullDetailBlob.IsSnapshot = blobRef.IsSnapshot;
            if(blobRef.SnapshotTime.HasValue)
            {
                fullDetailBlob.SnapshotTime = blobRef.SnapshotTime.Value.UtcDateTime;
            }
            fullDetailBlob.SnapshotQualifiedStorageUri = blobRef.SnapshotQualifiedStorageUri.PrimaryUri;
            fullDetailBlob.SnapshotQualifiedUri = blobRef.SnapshotQualifiedUri;

            return fullDetailBlob;
        }

        public async Task<bool> CreateContainer(AzureStorageContainer container)
        {
            var containerRef = blobClient.GetContainerReference(container.Name);
            var opResult = await containerRef.CreateIfNotExistsAsync();
            return opResult;
        }

        public async Task<bool> DeleteContainer(AzureStorageContainer container)
        {
            var containerRef = blobClient.GetContainerReference(container.Name);
            var opResult = await containerRef.DeleteIfExistsAsync();
            return opResult;
        }
        public async Task<BlobContainerPublicAccessType> GetAccessLevel(AzureStorageContainer container)
        {
            var containerRef = blobClient.GetContainerReference(container.Name);
            var perms = await containerRef.GetPermissionsAsync();
            return perms.PublicAccess;
        }

        public async Task ChangeAccessLevel(AzureStorageContainer container)
        {            
            var containerRef = blobClient.GetContainerReference(container.Name);
            var perms = await containerRef.GetPermissionsAsync();
            perms.PublicAccess = container.AccessLevel.Code;
            await containerRef.SetPermissionsAsync(perms);
        }
    }
}