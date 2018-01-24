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
            foreach (var gender in new Dictionary<int, Tuple<string, int>>
            {
                { 1, new Tuple<string, int>(@"Male", 1) },
                { 2, new Tuple<string, int>(@"Female", 2) }
            })
            {
                if (!mDPMSDatabaseContext.Genders.Any(a => a.GenderReadable.Equals(gender.Value.Item1)))
                {
                    if (mDPMSDatabaseContext.Genders.Any(a => a.GenderId.Equals(gender.Key))) { throw new Exception(@"Duplicate Id in seed data"); }
                    mDPMSDatabaseContext.Genders.Add(new Gender { GenderId = gender.Key, GenderReadable = gender.Value.Item1, DpmsGenderNumber = gender.Value.Item2 });
                }
            }
            
            // Save
            mDPMSDatabaseContext.SaveChanges();            
        }
    }
}
