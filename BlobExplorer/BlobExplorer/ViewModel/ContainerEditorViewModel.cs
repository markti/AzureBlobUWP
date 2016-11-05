using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobExplorer.Model;
using BlobExplorer.Events;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace BlobExplorer.ViewModel
{
    public class ContainerEditorViewModel : ViewModelBase
    {
        private AzureStorageClient client;
        ContainerAccessLevel publicContainerAccess;
        ContainerAccessLevel publicBlobAccess;
        ContainerAccessLevel privateAccess;
        private AzureStorageContainer currentContainer;
        public AzureStorageContainer CurrentContainer
        {
            get { return currentContainer; }
            set { currentContainer = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<ContainerAccessLevel> AccessLevels { get; private set; }
        private ContainerAccessLevel selectedAccessLevel;
        public ContainerAccessLevel SelectedAccessLevel
        {
            get { return selectedAccessLevel; }
            set { selectedAccessLevel = value; this.RaisePropertyChanged(); }
        }
        private bool canSave;
        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; RaisePropertyChanged(); }
        }

        public AzureStorageAccount StorageAccount { get; set; }


        public ContainerEditorViewModel()
        {
            AccessLevels = new ObservableCollection<ContainerAccessLevel>();
            InitializeAccessLevels();
        }

        private void InitializeAccessLevels()
        {
            publicContainerAccess = new ContainerAccessLevel() { Label = "Public Container", Description = "Anonymous clients can read blob and container content/metadata." };
            publicBlobAccess = new ContainerAccessLevel() { Label = "Public Blob", Description = "Anonymous clients can read blob content/metadata but no container listings are accessible." };
            privateAccess = new ContainerAccessLevel() { Label = "Off", Description = "No anonymous access. Only the account owner can access resources in this container." };

            this.AccessLevels.Add(publicContainerAccess);
            this.AccessLevels.Add(publicBlobAccess);
            this.AccessLevels.Add(privateAccess);
        }

        public void OnNavigatedTo(ContainerEditorNavigationContext context)
        {
            // first initialize the storage account meta data
            this.StorageAccount = context.Account;
            // then initialize the client
            InitializeClient();

            if (context.IsNew)
            {
                this.CurrentContainer = new AzureStorageContainer();
                Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Creating New Container" });
                // make sure we reset this on new containers
                this.SelectedAccessLevel = privateAccess;
            }
            else
            {
                this.CurrentContainer = context.Container;
                Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.CurrentContainer.Name });
                // TODO: find the right access level from the container's internal value
            }
            // turn on validation monitoring
            this.CurrentContainer.PropertyChanged += (s, e) => { Validate(); };
        }

        private void InitializeClient()
        {
            client = new AzureStorageClient(StorageAccount);
        }

        private void Validate()
        {
            this.CanSave = CurrentContainer.Name.Length > 0 && this.SelectedAccessLevel != null;
        }

        public async Task<bool> Save()
        {
            var result = await client.CreateContainer(this.CurrentContainer);
            return result;
        }
    }
}