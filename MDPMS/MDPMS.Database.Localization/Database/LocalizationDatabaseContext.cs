using MDPMS.Database.Localization.Models;
using Microsoft.EntityFrameworkCore;

namespace MDPMS.Database.Localization.Database
{
    public class LocalizationDatabaseContext : DbContext
    {
        public DbSet<Models.Localization> Localizations { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Value> Values { get; set; }

        private string DatabasePath { get; set; }

        public LocalizationDatabaseContext()
        {
            //
        }

        public LocalizationDatabaseContext(string databasePath)
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
