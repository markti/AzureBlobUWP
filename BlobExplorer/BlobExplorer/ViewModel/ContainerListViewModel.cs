using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.ViewModel
{
    public class ContainerListViewModel : ViewModelBase
    {
        private LocalStorageService localStorage;
        private AzureStorageClient client;
        private AzureStorageAccount storageAccount;
        public AzureStorageAccount StorageAccount
        {
            get { return storageAccount; }
            set { storageAccount = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<AzureStorageContainer> Containers { get; private set; }
        public ObservableCollection<AzureStorageContainer> SelectedContainers { get; private set; }
        private bool canDeleteContainers;
        public bool CanDeleteContainers
        {
            get { return canDeleteContainers; }
            set { canDeleteContainers = value; this.RaisePropertyChanged(); }
        }
        private bool canEditContainer;
        public bool CanEditContainer
        {
            get { return canEditContainer; }
            set { canEditContainer = value; this.RaisePropertyChanged(); }
        }
        private bool canAddContainer;

        public bool CanAddContainer
        {
            get { return canAddContainer; }
            set { canAddContainer = value; this.RaisePropertyChanged(); }
        }
        private bool canRefreshList;

        public bool CanRefreshList
        {
            get { return canRefreshList; }
            set { canRefreshList = value; this.RaisePropertyChanged(); }
        }



        public ContainerListViewModel()
        {
            this.localStorage = new LocalStorageService();
            this.Containers = new ObservableCollection<AzureStorageContainer>();
            this.SelectedContainers = new ObservableCollection<AzureStorageContainer>();
            this.SelectedContainers.CollectionChanged += SelectedContainers_CollectionChanged;
        }

        private void SelectedContainers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.CanEditContainer = this.SelectedContainers.Count == 1;
            this.CanDeleteContainers = this.SelectedContainers.Count > 0;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.StorageAccount = e.Parameter as AzureStorageAccount;

            this.CanAddContainer = StorageAccount.IsConnected;
            this.CanRefreshList = StorageAccount.IsConnected;
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.StorageAccount.Name });
            InitializeClient();
            RefreshContainers();
        }

        private void InitializeClient()
        {
            try
            {
                client = new AzureStorageClient(StorageAccount);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async Task RefreshContainers()
        {
            var containers = await client.GetContainers();
            this.Containers.Clear();
            foreach (var item in containers)
            {
                this.Containers.Add(item);
            }
        }

        internal void RemoveStorageAccount()
        {
            this.localStorage.RemoveStorageAccount(this.StorageAccount);
        }

        internal void Refresh()
        {
            this.RefreshContainers();
        }

        public async Task DeleteSelectedContainers()
        {
            foreach(var item in this.SelectedContainers)
            {
                await client.DeleteContainer(item);
            }
            await this.RefreshContainers();
        }
    }
}