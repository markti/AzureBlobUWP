﻿using BlobExplorer.Model;
using BlobExplorer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<AzureStorageAccount> Accounts { get; private set; }
        public ObservableCollection<MenuItemViewModel> Options { get; private set; }

        public MainViewModel()
        {
            this.Accounts = new ObservableCollection<AzureStorageAccount>();
            this.Options = new ObservableCollection<MenuItemViewModel>();

            this.InitializeOptions();
        }

        public void OnNavigatedTo()
        {
            this.RefreshStorageAccounts();
        }

        private void InitializeOptions()
        {
            var addAccountItem = new MenuItemViewModel() { Label = "Add Account", SymbolAsChar = '\uE1E2', PageType = typeof(NewStorageAccountView) };
            var settingsItem = new MenuItemViewModel() { Label = "Settings", SymbolAsChar = '\uE115', PageType = typeof(SettingsView) };
            this.Options.Add(addAccountItem);
            this.Options.Add(settingsItem);
        }

        private async Task RefreshStorageAccounts()
        {
            var storageService = new LocalStorageService();
            var accounts = await storageService.GetStorageAccounts();
            foreach(var item in accounts)
            {
                this.Accounts.Add(item);
            }
        }
    }
}