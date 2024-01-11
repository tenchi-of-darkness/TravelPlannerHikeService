using NetTopologySuite.Geometries;

namespace Hike.UseCases.Services.Interfaces;

public interface IRouteService
{
    Task<LineString?> GetRoute(Point start, Point end);
}