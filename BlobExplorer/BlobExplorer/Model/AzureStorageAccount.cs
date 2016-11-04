using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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