using System.ComponentModel.DataAnnotations.Schema;
using Hike.Domain.Entities;
using Hike.Domain.Enum;
using NetTopologySuite.Geometries;

namespace Hike.Data.DBO;

public class TrailDBO
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
    
    public Guid OwnerUserId { get; set; }
}