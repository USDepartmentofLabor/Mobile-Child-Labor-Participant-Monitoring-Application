using System;

namespace MDPMS.Shared.Models
{
    /// <summary>
    /// Person, adult or child
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Last name (family name)
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// First name (given name)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// DateTime date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Date of birth is approximate
        /// </summary>
        public bool DateOfBirthIsApproximate { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Family or social relationship to head of household, can be head of household
        /// </summary>
        public PersonRelationship RelationshipToHouseholdHead { get; set; }

        // TODO: Intake

        #region "Child objects"

        // Follow ups

        // Services

        #endregion
        
    }
}
