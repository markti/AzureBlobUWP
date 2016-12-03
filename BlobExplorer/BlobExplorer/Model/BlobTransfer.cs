using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Model
{
    public class BlobTransfer : ViewModelBase
    {
        private Guid identifier;
        public Guid Identifier
        {
            get { return identifier; }
            set { identifier = value; RaisePropertyChanged(); }
        }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; RaisePropertyChanged(); }
        }
        private string fullPath;
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; RaisePropertyChanged(); }
        }
        private double percentComplete;
        public double PercentComplete
        {
            get { return percentComplete; }
            set { percentComplete = value; RaisePropertyChanged(); }
        }
    }
}