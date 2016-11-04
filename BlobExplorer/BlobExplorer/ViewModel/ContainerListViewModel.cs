using BlobExplorer.Model;
using GalaSoft.MvvmLight;
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
        private AzureStorageAccount storageAccount;
        public ObservableCollection<AzureStorageContainer> Containers { get; private set; }

        public ContainerListViewModel()
        {
            this.localStorage = new LocalStorageService();
            this.Containers = new ObservableCollection<AzureStorageContainer>();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.storageAccount = e.Parameter as AzureStorageAccount;
            InitializeClient();
            RefreshContainers();
        }

        private void InitializeClient()
        {
            try
            {
                client = new AzureStorageClient(storageAccount);
            }
            catch (Exception ex)
            {
                // do something
            }
        }

        private async Task RefreshContainers()
        {
            try
            {
                var containers = await client.GetContainers();
                this.Containers.Clear();
                foreach (var item in containers)
                {
                    this.Containers.Add(item);
                }
            }
            catch (Exception ex)
            {
                // do something
            }
        }

        internal void DeleteStorageAccount()
        {
            this.localStorage.RemoveStorageAccount(this.storageAccount);
        }

        internal void Refresh()
        {
            this.RefreshContainers();
        }
    }
}