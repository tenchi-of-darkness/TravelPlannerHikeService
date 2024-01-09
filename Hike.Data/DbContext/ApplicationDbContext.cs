using Hike.Data.DBO;
using Hike.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hike.Data.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public required DbSet<TrailDBO> Trails { get; init; }
    

    public required DbSet<UserDBO> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrailDBO>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.LineString);
        });
        
        modelBuilder.Entity<UserDBO>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasMany<TrailDBO>(x=>x.FavoriteTrails).WithMany();
        });
    }
}