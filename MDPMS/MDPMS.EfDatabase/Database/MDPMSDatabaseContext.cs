using MDPMS.EfDatabase.EfModels;
using Microsoft.EntityFrameworkCore;

namespace MDPMS.EfDatabase.Database
{
    public class MDPMSDatabaseContext : DbContext
    {
        // Look ups
        public DbSet<Gender> Genders { get; set; }

        // Data
        public DbSet<Person> People { get; set; }

        private string DatabasePath { get; set; }
        
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
