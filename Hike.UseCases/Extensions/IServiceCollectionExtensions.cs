using Hike.UseCases.Mappings;
using Hike.UseCases.Services;
using Hike.UseCases.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hike.UseCases.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogic(this IServiceCollection collection)
    {
        collection.AddTransient<ITrailService, TrailService>();
        collection.AddAutoMapper(typeof(TrailMapping));
        return collection;
    }
}