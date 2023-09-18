using Hike.Logic.Services;
using Hike.Logic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hike.Logic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogic(this IServiceCollection collection)
    {
        collection.AddTransient<IPlanService, PlanService>();
        return collection;
    }
}