using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace BlobExplorer
{
    public class TransferManager : IDisposable
    {
        private CancellationTokenSource cts;

        private List<DownloadOperation> activeDownloads;
        public List<BlobTransfer> TransferHistory { get; set; }

        private static TransferManager instance;
        public static TransferManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new TransferManager();
                }
                return instance;
            }
        }
        private Dictionary<Guid, CancellationTokenSource> cancellationTokens;
        
        private TransferManager()
        {
            cts = new CancellationTokenSource();

            cancellationTokens = new Dictionary<Guid, CancellationTokenSource>();
            activeDownloads = new List<DownloadOperation>();

            TransferHistory = new List<BlobTransfer>();

            this.RegisterEvents();
        }

        public void Dispose()
        {
            // dispose of all cancellation tokens because the application is shutting down
            foreach(var cancelItem in cancellationTokens.Values)
            {
                if (cancelItem != null)
                {
                    cancelItem.Dispose();
                }
            }
            // remove all references to the cancellation tokens so that they may be garbage collected
            cancellationTokens.Clear();

            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }

            GC.SuppressFinalize(this);
        }

        private void RegisterEvents()
        {
            Messenger.Default.Register<BlobDownloadRequestedEvent>(this, OnBlobDownloadRequested);
            Messenger.Default.Register<BlobTransferCancelRequestedEvent>(this, OnTransferCancelRequested);
        }

        private void OnTransferCancelRequested(BlobTransferCancelRequestedEvent payload)
        {
            this.TransferHistory.Remove(payload.Transfer);

            var id = payload.Transfer.Identifier;
            var cts = cancellationTokens[id];
            if(cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                // remove all references to the cancellation token
                cancellationTokens.Remove(id);
            }
        }

        private void OnBlobDownloadRequested(BlobDownloadRequestedEvent payload)
        {
            var destinationFile = payload.Target;
            var source = payload.Source.Uri;

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, destinationFile);

            var transfer = new BlobTransfer();
            transfer.Identifier = download.Guid;
            transfer.PercentComplete = 0;
            transfer.FileName = payload.FileName;
            transfer.FullPath = payload.FullPath;

            this.TransferHistory.Add(transfer);

            HandleDownloadAsync(download, true);
        }

        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                // Store the download so we can pause/resume.
                activeDownloads.Add(download);

                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);

                //var cts = new CancellationTokenSource();
                //cancellationTokens.Add(download.Guid, cts);

                if (start)
                {
                    // Start the download and attach a progress handler.
                    await download.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await download.AttachAsync().AsTask(cts.Token, progressCallback);
                }

                ResponseInformation response = download.GetResponseInformation();

                // GetResponseInformation() returns null for non-HTTP transfers (e.g., FTP).
                string statusCode = response != null ? response.StatusCode.ToString() : String.Empty;
                
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                activeDownloads.Remove(download);
            }
        }


        // Note that this event is invoked on a background thread, so we cannot access the UI directly.
        private void DownloadProgress(DownloadOperation download)
        {
            // DownloadOperation.Progress is updated in real-time while the operation is ongoing. Therefore,
            // we must make a local copy so that we can have a consistent view of that ever-changing state
            // throughout this method's lifetime.
            BackgroundDownloadProgress currentProgress = download.Progress;
            
            double percent = 100;
            if (currentProgress.TotalBytesToReceive > 0)
            {
                percent = currentProgress.BytesReceived * 100 / currentProgress.TotalBytesToReceive;
            }

            var payload = new DownloadProgressChangedEvent();
            payload.Identifier = download.Guid;
            payload.Percent = percent;

            var transfer = this.TransferHistory.Where(f => f.Identifier == download.Guid).FirstOrDefault();
            if(transfer != null)
            {
                transfer.PercentComplete = percent;
            }

            Messenger.Default.Send<DownloadProgressChangedEvent>(payload);

            if (currentProgress.HasRestarted)
            {
            }

            if (currentProgress.HasResponseChanged)
            {
                // We have received new response headers from the server.
                // Be aware that GetResponseInformation() returns null for non-HTTP transfers (e.g., FTP).
                ResponseInformation response = download.GetResponseInformation();
                int headersCount = response != null ? response.Headers.Count : 0;
            }
        }

    }
}