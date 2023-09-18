using Hike.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Hike.Logic.Repositories.Interfaces;

namespace Hike.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection collection)
    {
        collection.AddTransient<IPlanRepository, PlanRepository>();
        return collection;
    }
}