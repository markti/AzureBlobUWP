using System;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using BlobExplorer.Model;
using BlobExplorer.Events;
using GalaSoft.MvvmLight.Messaging;

namespace BlobExplorer.ViewModel
{
    public class TransferListViewModel : ViewModelBase
    {
        TransferManager transferManager = null;

        public ObservableCollection<BlobTransfer> Transfers { get; private set; }

        public TransferListViewModel()
        {
            transferManager = TransferManager.Instance;
            this.Transfers = new ObservableCollection<BlobTransfer>();

        }

        internal void OnNavigatedTo()
        {
            foreach(var item in transferManager.TransferHistory)
            {
                this.Transfers.Add(item);
            }
        }

        internal void CancelTransfer(BlobTransfer transferItem)
        {
            var payload = new BlobTransferCancelRequestedEvent();
            payload.Transfer = transferItem;

            Messenger.Default.Send<BlobTransferCancelRequestedEvent>(payload);

            this.Transfers.Remove(transferItem);
        }
    }
}