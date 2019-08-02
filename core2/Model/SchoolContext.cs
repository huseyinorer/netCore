using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core2.Entities;
using core2.EntitiesF;

namespace core2.Model
{
    public class SchoolContext:DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)//dependency injection.
        {
           
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //       => optionsBuilder.UseNpgsql("Host=baasu.db.elephantsql.com;port=5432;Database=rhilzwzi;Username=rhilzwzi;Password=Vy9Z-iYDP235rYpywMIqkaeTnTn68Oim");
        public DbSet<Student> Students { get; set; }
        public DbSet<persons> persons { get; set; }
        public DbSet<Employee> Employees { get; set; }

        

    }
}
