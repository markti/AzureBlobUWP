using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class NewStorageAccountViewModel : ViewModelBase
    {
        private AzureStorageAccount currentAccount;

        public AzureStorageAccount CurrentAccount
        {
            get { return currentAccount; }
            set { currentAccount = value; }
        }
        private bool canSave;
        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; RaisePropertyChanged(); }
        }

        public NewStorageAccountViewModel()
        {
            this.CurrentAccount = new AzureStorageAccount();
            this.CurrentAccount.PropertyChanged += CurrentAccount_PropertyChanged;
        }

        private void CurrentAccount_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Validate();
        }

        private void Validate()
        {
            CanSave =
                !string.IsNullOrEmpty(this.CurrentAccount.Name) && this.CurrentAccount.Name.Length > 0 && 
                !string.IsNullOrEmpty(this.CurrentAccount.Key) && this.CurrentAccount.Key.Length > 0;
        }

        public async Task Save()
        {
            var storageService = new LocalStorageService();
            await storageService.AddStorageAccount(CurrentAccount);
        }
    }
}