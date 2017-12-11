using System;

namespace MDPMS.Shared.Models
{
    public class SerializedApplicationInstanceData
    {
        public DateTime? LastSync { get; set; } = null;
        public string Url { get; set; } = @"";
        public bool SyncError { get; set; } = false;
    }
}
