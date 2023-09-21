using Hike.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hike.Data.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=12345;database=hike-service", ServerVersion.AutoDetect("server=localhost;user=root;password=12345;"),
            options =>
            {
                options.UseNetTopologySuite();
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrailEntity>().HasKey(x => x.Id);
    }
    
    public DbSet<TrailEntity> Trails { get; set; }
}