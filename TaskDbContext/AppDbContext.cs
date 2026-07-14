using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.Models;

namespace TaskTrackerProject.TaskDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<SignUp> SignUps { get; set; }
        public DbSet<TaskTrackerProject.Models.Task> Tasks { get; set; }
    }
}
