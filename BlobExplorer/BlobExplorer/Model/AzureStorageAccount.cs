using GalaSoft.MvvmLight;

namespace BlobExplorer.Model
{
    public class AzureStorageAccount : ViewModelBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }
        private string key;
        public string Key
        {
            get { return key; }
            set { key = value;  RaisePropertyChanged(); }
        }
    }
}