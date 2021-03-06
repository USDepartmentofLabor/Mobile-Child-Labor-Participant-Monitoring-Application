﻿using System;

namespace MDPMS.Shared.Models
{
    public class SerializedApplicationInstanceData
    {
        public DateTime? LastSync { get; set; } = null;
        public string Url { get; set; } = @"";
        public bool SyncError { get; set; } = false;
        public string ApiKey { get; set; } = @"";
        public string LastSuccessfulUsernameUsed { get; set; } = @"";
        public string Localization { get; set; } = @"";
    }
}
