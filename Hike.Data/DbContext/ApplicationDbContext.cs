using Hike.Data.DBO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hike.Data.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration, DbSet<TrailDBO> trails)
    {
        _configuration = configuration;
        Trails = trails;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(_configuration.GetConnectionString("NoDatabase")),
            options =>
            {
                options.UseNetTopologySuite();
            });
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrailDBO>().HasKey(x => x.Id);
    }
    
    public DbSet<TrailDBO> Trails { get; set; }
}