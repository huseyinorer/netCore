using Microsoft.EntityFrameworkCore;

namespace MainProject.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext()
        {
        }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Settings> Settings { get; set; }
    }
}