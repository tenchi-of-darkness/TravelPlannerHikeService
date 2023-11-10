﻿using Hike.Logic.Entities;
using NetTopologySuite.Geometries;

namespace Hike.API.Models;

public class TrailModel
{
    public Guid Id { get; set; }
    
    public LineString LineString { get; set; }
    
    public float Rating { get; set; }
    
    public TrailDifficulty Difficulty { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public string LocationName { get; set; }
    
    public long DistanceInMeters { get; set; }
    
    
    public TrailEntity ToTrailEntity()
    {
        return new TrailEntity
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

    public TrailModel(TrailEntity entity)
    {
        Id = entity.Id;
        LineString = entity.LineString;
        Rating = entity.Rating;
        Difficulty = entity.Difficulty;
        Title = entity.Title;
        Description = entity.Description;
        LocationName = entity.LocationName;
        DistanceInMeters = entity.DistanceInMeters;
    }
}