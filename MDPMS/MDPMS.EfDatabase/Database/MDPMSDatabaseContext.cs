using MDPMS.EfDatabase.EfModels;
using Microsoft.EntityFrameworkCore;

namespace MDPMS.EfDatabase.Database
{
    public class MDPMSDatabaseContext : DbContext
    {
        // Look ups
        public DbSet<Gender> Genders { get; set; }
        public DbSet<PersonRelationship> PersonRelationships { get; set; }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }

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
