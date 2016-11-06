using GalaSoft.MvvmLight;

namespace BlobExplorer.Model
{
    public class AzureStorageContainer : ViewModelBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }
        private ContainerAccessLevel accessLevel;
        public ContainerAccessLevel AccessLevel
        {
            get { return accessLevel; }
            set { accessLevel = value; this.RaisePropertyChanged(); }
        }
    }
}