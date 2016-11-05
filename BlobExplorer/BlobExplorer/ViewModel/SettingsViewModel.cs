using BlobExplorer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private LocalStorageService localStorage;
        private UserSettings userSettings;

        public UserSettings UserSettings
        {
            get { return userSettings; }
            set { userSettings = value; RaisePropertyChanged(); }
        }

        public SettingsViewModel()
        {
            this.localStorage = new LocalStorageService();
        }

        public async Task OnNavigatedTo()
        {
            this.UserSettings = await localStorage.GetSettings();
        }

        public async Task Save()
        {
            await localStorage.SaveSettings(this.UserSettings);
        }
    }
}