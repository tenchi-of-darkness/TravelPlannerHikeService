using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Hike.Data.Entities;

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

    public TrailModel ToTrailModel()
    {
        return new TrailModel
        {
            Id = Id,
            LineString = LineString,
            Rating = Rating,
            Difficulty = Difficulty,
            Title = Title,
            Description = Description,
            LocationName = LocationName,
            DistanceInMeters = DistanceInMeters
        };
    }

    public TrailEntity(TrailModel model)
    {
        Id = model.Id;
        LineString = model.LineString;
        Rating = model.Rating;
        Difficulty = model.Difficulty;
        Title = model.Title;
        Description = model.Description;
        LocationName = model.LocationName;
        DistanceInMeters = model.DistanceInMeters;
    }

    public TrailEntity()
    {
        
    }
}