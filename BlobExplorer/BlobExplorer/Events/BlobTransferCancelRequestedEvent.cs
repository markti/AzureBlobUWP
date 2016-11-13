using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobExplorer.Model;

namespace BlobExplorer.Events
{
    public class BlobTransferCancelRequestedEvent
    {
        public BlobTransfer Transfer { get; internal set; }
    }
}