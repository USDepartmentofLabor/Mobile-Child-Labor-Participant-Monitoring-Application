using MDPMS.EfDatabase.EfModels.Base;
using System;

namespace MDPMS.EfDatabase.EfModels
{
    public class Person : EfBaseModel
    {
        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // ForeignKey to Gender        
        public Gender Gender { get; set; }
    }
}
