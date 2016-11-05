using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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