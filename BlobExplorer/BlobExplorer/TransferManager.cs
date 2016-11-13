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
        private List<DownloadOperation> activeDownloads;
        private CancellationTokenSource cts;

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

        private TransferManager()
        {
            TransferHistory = new List<BlobTransfer>();
            cts = new CancellationTokenSource();
            activeDownloads = new List<DownloadOperation>();

            this.RegisterEvents();
        }

        public void Dispose()
        {
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
        }

        private void OnBlobDownloadRequested(BlobDownloadRequestedEvent obj)
        {
            var destinationFile = obj.Target;
            var source = obj.Source.Uri;

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, destinationFile);

            var transfer = new BlobTransfer();
            transfer.Identifier = download.Guid;
            transfer.PercentComplete = 0;
            transfer.FileName = obj.FileName;
            transfer.FullPath = obj.FullPath;

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