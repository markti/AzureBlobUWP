using BlobExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Navigation
{
    public class BlobListNavigationContext
    {
        public AzureStorageAccount Account { get; set; }
        public AzureStorageContainer Container { get; set; }
        public string BlobPrefix { get; set; }
    }
}