using Hike.Data.DBO;
using Microsoft.EntityFrameworkCore;

namespace Hike.Data.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public required DbSet<TrailDBO> Trails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrailDBO>().HasKey(x => x.Id);
        modelBuilder.Entity<TrailDBO>().Property(x => x.LineString).HasSrid(4326);
    }
}