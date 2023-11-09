using Hike.Data.DbContext;
using Hike.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Hike.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hike.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddTransient<ITrailRepository, TrailRepository>();
        collection.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.UseMySql(configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(configuration.GetConnectionString("NoDatabase")),
                options =>
                {
                    options.UseNetTopologySuite();
                });
        });
        return collection;
    }
}