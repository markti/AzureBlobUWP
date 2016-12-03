using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

        private bool canSave;
        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; RaisePropertyChanged(); }
        }
        private bool canEditName;
        public bool CanEditName
        {
            get { return canEditName; }
            set { canEditName = value; this.RaisePropertyChanged(); }
        }
        public bool IsNew { get; set; }
        public AzureStorageAccount StorageAccount { get; set; }


        public ContainerEditorViewModel()
        {
            AccessLevels = new ObservableCollection<ContainerAccessLevel>();
            InitializeAccessLevels();
        }

        private void InitializeAccessLevels()
        {
            publicContainerAccess = new ContainerAccessLevel() { Label = "Public Container", Description = "Anonymous clients can read blob and container content/metadata.", Code = BlobContainerPublicAccessType.Container };
            publicBlobAccess = new ContainerAccessLevel() { Label = "Public Blob", Description = "Anonymous clients can read blob content/metadata but no container listings are accessible.", Code = BlobContainerPublicAccessType.Blob };
            privateAccess = new ContainerAccessLevel() { Label = "Off", Description = "No anonymous access. Only the account owner can access resources in this container.", Code = BlobContainerPublicAccessType.Off };

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

            this.IsNew = context.IsNew;
            if (this.IsNew)
            {
                this.CurrentContainer = new AzureStorageContainer();
                Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Creating New Container" });
                // make sure we reset this on new containers
                this.CurrentContainer.AccessLevel = privateAccess;
                this.CanEditName = true;
            }
            else
            {
                this.CurrentContainer = context.Container;
                Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = this.CurrentContainer.Name });
                // TODO: find the right access level from the container's internal value
                var existingMatch = this.AccessLevels.Where(f => f.Code == this.CurrentContainer.AccessLevel.Code).FirstOrDefault();
                this.CurrentContainer.AccessLevel = existingMatch;
                this.CanEditName = false;
                Validate();
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
            this.CanSave = CurrentContainer.Name.Length > 0 && this.CurrentContainer.AccessLevel != null;
        }

        public async Task<bool> Save()
        {
            var result = false;
            if(this.IsNew)
            {
                result = await client.CreateContainer(this.CurrentContainer);
            }
            else
            {
                await client.ChangeAccessLevel(this.CurrentContainer);
                // if it returns then we're good
                result = true;
            }
            return result;
        }
    }
}