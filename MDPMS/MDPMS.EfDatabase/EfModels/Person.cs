using MDPMS.EfDatabase.EfModels.Base;
using System;

namespace MDPMS.EfDatabase.EfModels
{
    /// <summary>
    /// Person, adult or child
    /// </summary>
    public class Person : EfBaseModel
    {
        /// <summary>
        /// First name (given name)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name (family name)
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Gender
        /// </summary>
        // ForeignKey to Gender        
        public Gender Gender { get; set; }

        /// <summary>
        /// Date of birth, DateTime
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Date of birth is approximate date?, true = yes, false = no
        /// </summary>
        public bool DateOfBirthIsApproximate { get; set; }

        /// <summary>
        /// Relationship to head of household
        /// </summary>
        public PersonRelationship RelationshipToHeadOfHousehold { get; set; }

        /// <summary>
        /// Optional, relationship to head of household if other
        /// </summary>
        public string RelationshipIfOther { get; set; }
    }
}
