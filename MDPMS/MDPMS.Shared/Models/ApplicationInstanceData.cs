using System;
using System.Collections.Generic;
using System.Text;

namespace MDPMS.Shared.Models
{
    public class ApplicationInstanceData
    {
        // PURPOSE: package instance data (cache, settings, etc.)     

        public DateTime? LastSync { get; set; }
        public string Url { get; set; }
        public bool SyncError { get; set; }
    }
}
