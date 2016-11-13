using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobExplorer.Events
{
    public class DownloadProgressChangedEvent
    {
        public Guid Identifier { get; internal set; }
        public double Percent { get; internal set; }
    }
}