using System.ComponentModel.DataAnnotations.Schema;
using Hike.Domain.Enum;
using NetTopologySuite.Geometries;

namespace Hike.Domain.Entities;

public class TrailEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public LineString LineString { get; set; } = LineString.Empty;

    public float Rating { get; set; }

    public TrailDifficulty Difficulty { get; set; }

    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public string LocationName { get; set; } = "";

    public long DistanceInMeters { get; set; }

    public string OwnerUserId { get; set; }
}