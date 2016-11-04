using BlobExplorer.Model;
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

        public void Save()
        {
            var storageService = new LocalStorageService();
            storageService.SaveStorageAccount(CurrentAccount);
        }
    }
}