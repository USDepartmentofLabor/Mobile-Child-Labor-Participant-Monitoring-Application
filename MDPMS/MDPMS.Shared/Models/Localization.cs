using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MDPMS.Shared.Models
{
    public class Localization
    {
        /// <summary>
        /// ISO 639-1 code
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// ISO language name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(@"(");
                sb.Append(Abbreviation);
                sb.Append(@") - ");
                sb.Append(Name);
                return sb.ToString();
            }
        }

        // Values
        public Dictionary<string, string> Translations { get; set; }
    }
}
