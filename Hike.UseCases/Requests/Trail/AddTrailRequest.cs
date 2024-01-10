using Hike.Domain.Enum;
using NetTopologySuite.Geometries;

namespace Hike.UseCases.Requests.Trail;

public record AddTrailRequest(
    Point Start,
    Point End,
    float Rating,
    TrailDifficulty Difficulty,
    string Title,
    string? Description,
    string LocationName,
    long DistanceInMeters);