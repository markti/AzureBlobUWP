using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Model
{
    public class AzureStorageBlob
    {
        public bool IsDirectory { get; internal set; }
        public string Name { get; set; }
        public string Path { get; internal set; }
    }
}