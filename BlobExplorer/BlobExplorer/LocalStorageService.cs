using BlobExplorer.Model;
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
        public async Task<List<AzureStorageAccount>> GetStorageAccounts()
        {
            var list = new List<AzureStorageAccount>();

            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("accounts.json");

            if(item != null)
            {
                var storageFile = await StorageFile.GetFileFromPathAsync(item.Path);

                var rawJson = string.Empty;

                JsonConvert.DeserializeObject<List<AzureStorageAccount>>(rawJson);
            }

            return list;
        }

        public async Task SaveStorageAccount(AzureStorageAccount account)
        {
            var list = await GetStorageAccounts();

            var match = list.Where(f => f.Name == account.Name).FirstOrDefault();

            if(match == null)
            {
                list.Add(account);
            }

            var rawJson = JsonConvert.SerializeObject(list);

            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("accounts.json");

            var accountsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("accounts.json", CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(accountsFile, rawJson);

            
        }
    }
}