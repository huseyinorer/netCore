using Microsoft.EntityFrameworkCore;

namespace MainProject.Models
{
    public class ProjectDbContext : DbContext
    {       

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)  : base(options)
        {
        }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<HomeSliderPhotos> HomeSliderPhotos { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=raja.db.elephantsql.com;port=5432;Database=rouiqpyy;Username=rouiqpyy;Password=mqlhwqWK0rKJV0Mw14DyuPyCq5DQp7oq");
        //}
    }
}