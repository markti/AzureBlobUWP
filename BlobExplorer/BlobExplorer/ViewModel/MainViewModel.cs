using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<AzureStorageAccount> Accounts { get; private set; }
        public ObservableCollection<MenuItemViewModel> Options { get; private set; }

        public MainViewModel()
        {
            this.Accounts = new ObservableCollection<AzureStorageAccount>();
            this.Options = new ObservableCollection<MenuItemViewModel>();

            this.InitializeOptions();
            this.InitializeEventHandlers();
        }

        public void OnNavigatedTo()
        {
            this.RefreshStorageAccounts();
        }

        private void InitializeEventHandlers()
        {
            Messenger.Default.Register<AccountCreatedEvent>(this, HandleAccountCreatedEvent);
            Messenger.Default.Register<AccountDeletedEvent>(this, HandleAccountDeletedEvent);
            Messenger.Default.Register<PageTitleChangedEvent>(this, HandlePageTitleChangedEvent);
        }

        private void HandlePageTitleChangedEvent(PageTitleChangedEvent payload)
        {
            this.Title = payload.Title;
        }

        private void HandleAccountDeletedEvent(AccountDeletedEvent obj)
        {
            this.RefreshStorageAccounts();
        }

        private void HandleAccountCreatedEvent(AccountCreatedEvent obj)
        {
            this.RefreshStorageAccounts();
        }

        private void InitializeOptions()
        {
            this.Options.Clear();
            var transferListItem = new MenuItemViewModel() { Label = "Transfers", SymbolAsChar = '\uE896', PageType = typeof(TransferListView) };
            var addAccountItem = new MenuItemViewModel() { Label = "Add Account", SymbolAsChar = '\uE1E2', PageType = typeof(NewStorageAccountView) };
            var settingsItem = new MenuItemViewModel() { Label = "Settings", SymbolAsChar = '\uE115', PageType = typeof(SettingsView) };

            this.Options.Add(transferListItem);
            this.Options.Add(addAccountItem);
            this.Options.Add(settingsItem);
        }

        private async Task RefreshStorageAccounts()
        {
            this.Accounts.Clear();
            var storageService = new LocalStorageService();
            var accounts = await storageService.GetStorageAccounts();
            foreach(var item in accounts)
            {
                this.Accounts.Add(item);
            }
        }
    }
}