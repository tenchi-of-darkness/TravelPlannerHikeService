using Hike.Domain.Entities;
using Hike.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace Hike.UseCases.Requests.Trail;

public record AddTrailRequest(LineString LineString, float Rating, TrailDifficulty Difficulty, string Title,
    string? Description, string LocationName, long DistanceInMeters);