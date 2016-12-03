using GalaSoft.MvvmLight;

namespace BlobExplorer.Model
{
    public class UserSettings : ViewModelBase
    {
        private bool shouldGroupDirectories;
        public bool ShouldGroupDirectories
        {
            get { return shouldGroupDirectories; }
            set { shouldGroupDirectories = value; RaisePropertyChanged(); }
        }
    }
}