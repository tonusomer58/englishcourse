using Microsoft.EntityFrameworkCore;
using SozlukApp.Models;

namespace SozlukApp.Data
{
    public class SozlukContext : DbContext
    {
        public SozlukContext(DbContextOptions<SozlukContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "123", Role = "Admin" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
