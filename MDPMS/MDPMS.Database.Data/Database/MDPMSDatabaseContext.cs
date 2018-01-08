using MDPMS.Database.Data.Models;
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
        }
    }
}
