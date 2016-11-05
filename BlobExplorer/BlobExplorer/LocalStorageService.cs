using BlobExplorer.Events;
using BlobExplorer.Model;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BlobExplorer
{
    public class LocalStorageService
    {
        public async Task<UserSettings> GetSettings()
        {
            var settings = new UserSettings();

            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("settings.json");

            if (item != null)
            {
                var storageFile = await StorageFile.GetFileFromPathAsync(item.Path);

                var rawJson = await FileIO.ReadTextAsync(storageFile);

                var accounts = JsonConvert.DeserializeObject<UserSettings>(rawJson);

                settings = accounts;
            }

            return settings;
        }

        public async Task SaveSettings(UserSettings userSettings)
        {
            try
            {
                var rawJson = JsonConvert.SerializeObject(userSettings);

                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("settings.json");

                var accountsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);

                await FileIO.WriteTextAsync(accountsFile, rawJson);
            }
            catch (Exception ex)
            {
                // do something
            }
        } 

        public async Task<List<AzureStorageAccount>> GetStorageAccounts()
        {
            var list = new List<AzureStorageAccount>();

            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("accounts.json");

            if(item != null)
            {
                var storageFile = await StorageFile.GetFileFromPathAsync(item.Path);

                var rawJson = await FileIO.ReadTextAsync(storageFile);

                var accounts = JsonConvert.DeserializeObject<List<AzureStorageAccount>>(rawJson);

                list = accounts;
            }

            return list;
        }

        public async Task AddStorageAccount(AzureStorageAccount account)
        {
            var list = await GetStorageAccounts();

            var match = list.Where(f => f.Name == account.Name).FirstOrDefault();

            if(match == null)
            {
                list.Add(account);

                await SaveList(list);

                Messenger.Default.Send<AccountCreatedEvent>(new AccountCreatedEvent());
            }
        }

        public async Task RemoveStorageAccount(AzureStorageAccount account)
        {
            var list = await GetStorageAccounts();

            var match = list.Where(f => f.Name == account.Name).FirstOrDefault();

            if (match != null)
            {
                list.Remove(match);

                await SaveList(list);

                Messenger.Default.Send<AccountDeletedEvent>(new AccountDeletedEvent());
            }
        }

        private async Task SaveList(List<AzureStorageAccount> list)
        {
            try
            {
                var rawJson = JsonConvert.SerializeObject(list);

                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("accounts.json");

                var accountsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("accounts.json", CreationCollisionOption.OpenIfExists);

                await FileIO.WriteTextAsync(accountsFile, rawJson);
            }
            catch (Exception ex)
            {
                // do something
            }
        }
    }
}