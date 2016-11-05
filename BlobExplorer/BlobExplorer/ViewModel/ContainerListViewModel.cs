using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.ViewModel
{
    public class ContainerListViewModel : ViewModelBase
    {
        private LocalStorageService localStorage;
        private AzureStorageClient client;
        public AzureStorageAccount StorageAccount { get; set; }
        public ObservableCollection<AzureStorageContainer> Containers { get; private set; }
        public ObservableCollection<AzureStorageContainer> SelectedContainers { get; private set; }
        private bool canDeleteContainers;
        public bool CanDeleteContainers
        {
            get { return canDeleteContainers; }
            set { canDeleteContainers = value; this.RaisePropertyChanged(); }
        }

        public ContainerListViewModel()
        {
            this.localStorage = new LocalStorageService();
            this.Containers = new ObservableCollection<AzureStorageContainer>();
            this.SelectedContainers = new ObservableCollection<AzureStorageContainer>();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.StorageAccount = e.Parameter as AzureStorageAccount;

            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.StorageAccount.Name });
            InitializeClient();
            RefreshContainers();
        }

        private void InitializeClient()
        {
            client = new AzureStorageClient(StorageAccount);
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