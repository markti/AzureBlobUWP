using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class NewStorageAccountViewModel
    {
        private AzureStorageAccount currentAccount;

        public AzureStorageAccount CurrentAccount
        {
            get { return currentAccount; }
            set { currentAccount = value; }
        }

        public NewStorageAccountViewModel()
        {
            this.CurrentAccount = new AzureStorageAccount();
        }

        public async Task Save()
        {
            var storageService = new LocalStorageService();
            await storageService.AddStorageAccount(CurrentAccount);
        }
    }
}