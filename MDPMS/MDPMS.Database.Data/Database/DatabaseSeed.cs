using MDPMS.Database.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDPMS.Database.Data.Database
{
    public static class DatabaseSeed
    {
        public static void SeedDatabase(MDPMSDatabaseContext mDPMSDatabaseContext)
        {
            // Add a project details here for now
            if (mDPMSDatabaseContext.ProjectDetails.Count().Equals(0))
            {
                mDPMSDatabaseContext.ProjectDetails.Add(new ProjectDetails
                {
                    InternalId = 1,
                    ExternalId = null,
                    CreatedAt = null,
                    LastUpdatedAt = null,
                    SoftDeleted = false,
                    ShortName = @"Test Project",
                    FullName = @"This is a fake test project",
                    CooperativeAgreementNumber = @"AB-1234-CDE-567-89-FGH",
                    StartDate = new DateTime(2017, 12, 01),
                    EndDate = new DateTime(2018, 12, 01),
                    GranteeName = @"IMPAQ International",
                    FundingAmount = 1000000,
                    Region = @"North America"
                });
            }

            // Genders            
            foreach (var gender in new Dictionary<int, string>
            {
                { 1, @"Male" },
                { 2, @"Female" }
            })
            {
                if (!mDPMSDatabaseContext.Genders.Any(a => a.GenderReadable.Equals(gender.Value)))
                {
                    if (mDPMSDatabaseContext.Genders.Any(a => a.GenderId.Equals(gender.Key))) { throw new Exception(@"Duplicate Id in seed data"); }
                    mDPMSDatabaseContext.Genders.Add(new Gender { GenderId = gender.Key, GenderReadable = gender.Value });
                }
            }

            // Person Relationships
            // Dictionary<int, Tuple<string, string, string, bool>> = <Id, <Code, Display Name, Canonical Name, IsOther>>
            foreach (var relationship in new Dictionary<int, Tuple<string, string, string, bool>>
            {
                { 1, new Tuple<string, string, string, bool>(@"01", @"Head of household", @"HEAD", false)},
                { 2, new Tuple<string, string, string, bool>(@"02", @"Spouse/partner", @"SPOUSE", false)},
                { 3, new Tuple<string, string, string, bool>(@"03", @"Son/daughter", @"CHILD", false)},
                { 4, new Tuple<string, string, string, bool>(@"04", @"Step child", @"STEP_CHILD", false)},
                { 5, new Tuple<string, string, string, bool>(@"05", @"Adopted/fostered child", @"ADOPTED_FOSTERED_CHILD", false)},
                { 6, new Tuple<string, string, string, bool>(@"06", @"Son-in-law/daughter-in-law", @"CHILD_IN_LAW", false)},
                { 7, new Tuple<string, string, string, bool>(@"07", @"Grandchild", @"GRANDCHILD", false)},
                { 8, new Tuple<string, string, string, bool>(@"08", @"Parent", @"PARENT", false)},
                { 9, new Tuple<string, string, string, bool>(@"09", @"Parent-in-law", @"PARENT_IN_LAW", false)},
                { 10, new Tuple<string, string, string, bool>(@"10", @"Grandparent", @"GRANDPARENT", false)},
                { 11, new Tuple<string, string, string, bool>(@"11", @"Brother/sister", @"SIBLING", false)},
                { 12, new Tuple<string, string, string, bool>(@"12", @"Brother-in-law/sister-in-law", @"SIBLING_IN_LAW", false)},
                { 13, new Tuple<string, string, string, bool>(@"13", @"Aunt/uncle", @"AUNT_UNCLE", false)},
                { 14, new Tuple<string, string, string, bool>(@"14", @"Niece/nephew", @"NIECE_NEPHEW", false)},
                { 15, new Tuple<string, string, string, bool>(@"15", @"Cousin", @"COUSIN", false)},
                { 16, new Tuple<string, string, string, bool>(@"16", @"Servant", @"SERVANT", false)},
                { 17, new Tuple<string, string, string, bool>(@"17", @"Non-relative", @"NON_RELATIVE", false)},
                { 18, new Tuple<string, string, string, bool>(@"18", @"Other (specify)", @"OTHER", true)}
            })
            {
                if (!mDPMSDatabaseContext.PersonRelationships.Any(a => a.CanonicalName.Equals(relationship.Value.Item3)))
                {
                    if (mDPMSDatabaseContext.PersonRelationships.Any(a => a.InternalId.Equals(relationship.Key))) { throw new Exception(@"Duplicate Id in seed data"); }
                    mDPMSDatabaseContext.PersonRelationships.Add(new PersonRelationship
                    {
                        InternalId = relationship.Key,
                        ExternalId = null,
                        CreatedAt = null,
                        LastUpdatedAt = null,
                        Code = relationship.Value.Item1,
                        DisplayName = relationship.Value.Item2,
                        CanonicalName = relationship.Value.Item3,
                        IsOther = relationship.Value.Item4,
                        SoftDeleted = false
                    });
                }
            }

            // Save
            mDPMSDatabaseContext.SaveChanges();            
        }
    }
}
