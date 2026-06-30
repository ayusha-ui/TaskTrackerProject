using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.Models;

namespace TaskTrackerProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SignUp> SignUps { get; set; }
    }
}