using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Hike.Logic.Entities;

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
    
    public Guid OwnerUserId { get; set; }


    public TrailEntity()
    {
        
    }
}

public enum TrailDifficulty
{
    Beginner=0,
    Intermediate=10,
    Hard=20
}