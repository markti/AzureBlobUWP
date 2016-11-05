using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Model
{
    public class ContainerAccessLevel
    {
        public BlobContainerPublicAccessType Code { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}