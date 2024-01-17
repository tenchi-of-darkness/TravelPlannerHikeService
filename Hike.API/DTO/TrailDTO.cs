using Hike.Domain.Enum;
using NetTopologySuite.Geometries;

namespace Hike.API.DTO;

public record TrailDTO(
    LineString LineString,
    float Rating,
    TrailDifficulty Difficulty,
    string Title,
    string? Description,
    string LocationName,
    long DistanceInMeters,
    Guid Id,
    string OwnerUserId,
    bool IsFavorite);