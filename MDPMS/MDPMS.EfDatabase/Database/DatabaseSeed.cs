using MDPMS.EfDatabase.EfModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDPMS.EfDatabase.Database
{
    public static class DatabaseSeed
    {
        public static void SeedDatabase(MDPMSDatabaseContext mDPMSDatabaseContext)
        {
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
            mDPMSDatabaseContext.SaveChanges();            
        }
    }
}
