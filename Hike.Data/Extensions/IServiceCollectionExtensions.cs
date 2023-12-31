﻿using Hike.Data.DbContext;
using Hike.Data.Mappings;
using Hike.Data.Repositories;
using Hike.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hike.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddTransient<ITrailRepository, TrailRepository>();
        collection.AddAutoMapper(typeof(TrailDataMapping));
        collection.AddDbContext<ApplicationDbContext>(builder =>
        {
            var connectionString = configuration.GetConnectionString("Default");
            
            builder.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString),
                options => { options.UseNetTopologySuite(); });
        });
        return collection;
    }
}