﻿using BlobExplorer.Model;
using GalaSoft.MvvmLight;
using System.Text.RegularExpressions;
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
        private bool isStorageAccountNameInvalid;

        public bool IsStorageAccountNameValid
        {
            get { return isStorageAccountNameInvalid; }
            set { isStorageAccountNameInvalid = value; this.RaisePropertyChanged(); }
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
            var hasRequiredFields =
                !string.IsNullOrEmpty(this.CurrentAccount.Name) && this.CurrentAccount.Name.Length > 0 &&
                !string.IsNullOrEmpty(this.CurrentAccount.Key) && this.CurrentAccount.Key.Length > 0;

            var pattern = @"^([a-z0-9]){3,24}$";
            this.IsStorageAccountNameValid = Regex.Match(this.CurrentAccount.Name, pattern).Length > 0;
            this.CanSave = this.IsStorageAccountNameValid && hasRequiredFields;
        }

        public async Task Save()
        {
            var storageService = new LocalStorageService();
            await storageService.AddStorageAccount(CurrentAccount);
        }
    }
}