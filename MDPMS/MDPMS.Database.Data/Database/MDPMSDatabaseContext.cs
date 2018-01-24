using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Database.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace MDPMS.Database.Data.Database
{
    public class MDPMSDatabaseContext : DbContext
    {
        // Look ups
        public DbSet<Gender> Genders { get; set; }
        public DbSet<PersonRelationship> PersonRelationships { get; set; }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }

        // Status Customization Look Ups
        public DbSet<StatusCustomizationHazardousCondition> StatusCustomizationHazardousConditions { get; set; }
        public DbSet<StatusCustomizationHouseholdTask> StatusCustomizationHouseholdTasks { get; set; }
        public DbSet<StatusCustomizationWorkActivity> StatusCustomizationWorkActivities { get; set; }

        // Data
        public DbSet<Household> Households { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }

        private string DatabasePath { get; set; }
        
        public MDPMSDatabaseContext()
        {
            //
        }

        public MDPMSDatabaseContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add constraints etc. here
            base.OnModelCreating(modelBuilder);

            // ManyToMany(Person, HazardousConditions)
            modelBuilder.Entity<PersonHazardousCondition>()
                .HasKey(a => new { a.PersonInternalId, a.HazardousConditionInternalId });
            modelBuilder.Entity<PersonHazardousCondition>()
                .HasOne(phc => phc.HazardousCondition)
                .WithMany(hc => hc.PeopleHazardousConditions)
                .HasForeignKey(phc => phc.HazardousConditionInternalId);
            modelBuilder.Entity<PersonHazardousCondition>()
                .HasOne(phc => phc.Person)
                .WithMany(p => p.PeopleHazardousConditions)
                .HasForeignKey(phc => phc.PersonInternalId);

            // ManyToMany(Person, WorkActivity)
            modelBuilder.Entity<PersonWorkActivity>()
                .HasKey(a => new { a.PersonInternalId, a.WorkActivityInternalId });
            modelBuilder.Entity<PersonWorkActivity>()
                .HasOne(phc => phc.WorkActivity)
                .WithMany(hc => hc.PeopleWorkActivities)
                .HasForeignKey(phc => phc.WorkActivityInternalId);
            modelBuilder.Entity<PersonWorkActivity>()
                .HasOne(phc => phc.Person)
                .WithMany(p => p.PeopleWorkActivities)
                .HasForeignKey(phc => phc.PersonInternalId);

            // ManyToMany(Person, HouseholdTask)
            modelBuilder.Entity<PersonHouseholdTask>()
                .HasKey(a => new { a.PersonInternalId, a.HouseholdTaskInternalId });
            modelBuilder.Entity<PersonHouseholdTask>()
                .HasOne(phc => phc.HouseholdTask)
                .WithMany(hc => hc.PeopleHouseholdTasks)
                .HasForeignKey(phc => phc.HouseholdTaskInternalId);
            modelBuilder.Entity<PersonHouseholdTask>()
                .HasOne(phc => phc.Person)
                .WithMany(p => p.PeopleHouseholdTasks)
                .HasForeignKey(phc => phc.PersonInternalId);
        }

        public PersonRelationship FindPersonRelationship(int externalId)
        {
            return FindISyncableByExternalId(externalId, PersonRelationships);
        }

        public StatusCustomizationHazardousCondition FindStatusCustomizationHazardousCondition(int externalId)
        {            
            return FindISyncableByExternalId(externalId, StatusCustomizationHazardousConditions);
        }

        public StatusCustomizationWorkActivity FindStatusCustomizationWorkActivity(int externalId)
        {
            return FindISyncableByExternalId(externalId, StatusCustomizationWorkActivities);
        }

        public StatusCustomizationHouseholdTask FindStatusCustomizationHouseholdTask(int externalId)
        {
            return FindISyncableByExternalId(externalId, StatusCustomizationHouseholdTasks);
        }

        private static T FindISyncableByExternalId<T>(int externalId, IQueryable<T> data) where T : class, ISyncable<T>
        {
            var search = data.Where(a => a.GetExternalId().Equals(externalId));
            return !search.Count().Equals(1) ? null : search.First();
        }

        public Gender GetMaleGender() => GetGender(@"Male");

        public Gender GetFemaleGender() => GetGender(@"Female");

        private Gender GetGender(string description)
        {
            var search = Genders.Where(a => a.GenderReadable.Equals(description));
            return !search.Count().Equals(1) ? null : search.First();
        }        
    }
}
