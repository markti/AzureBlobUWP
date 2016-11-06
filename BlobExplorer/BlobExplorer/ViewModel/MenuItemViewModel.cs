using GalaSoft.MvvmLight;
using System;

namespace BlobExplorer.ViewModel
{
    public class MenuItemViewModel : ViewModelBase
    {
        public string Label { get; set; }

        private char symbolAsChar;
        public char SymbolAsChar
        {
            get
            {
                return symbolAsChar;
            }
            set
            {
                symbolAsChar = value;
                RaisePropertyChanged();
            }
        }
        public Type PageType { get; set; }
    }
}