using AutoMapper;
using Hike.API.Mappings;
using Hike.Data.Mappings;
using Hike.Domain.Entities;
using Hike.Domain.Enum;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Mappings;
using Hike.UseCases.Services;
using Hike.UseCases.Services.Interfaces;
using Moq;
using NetTopologySuite.Geometries;

// ReSharper disable PossibleMultipleEnumeration

namespace Hike.Tests;

public class TrailServiceTests
{
    private readonly Mock<ITrailRepository> _trailRepositoryMock = new();
    private readonly ITrailService _instance;

    public TrailServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TrailMapping>();
            cfg.AddProfile<TrailDataMapping>();

            cfg.AddProfile<TrailApiMapping>();
        });
        IMapper mapper = new Mapper(config);
        _instance = new TrailService(_trailRepositoryMock.Object, mapper);
    }

    [Fact]
    public async Task SearchTest()
    {
        //Arrange
        string? searchValue = "";
        int page = 1;
        int pageSize = 5;
        var trailsList = new List<TrailEntity>
        {
            new()
            {
                Description = "Test",
                Difficulty = TrailDifficulty.Beginner,
                DistanceInMeters = 5,
                Id = Guid.NewGuid(),
                LineString = new LineString(new[] { new Coordinate(1, 1), new Coordinate(1, 2) }),
                Rating = 4,
                Title = "Test",
                LocationName = "Eindhoven"
            }
        };

        _trailRepositoryMock.Setup(x => x.SearchTrailByTitle(searchValue, page, pageSize)).ReturnsAsync(trailsList);

        //Act
        IEnumerable<TrailEntity> trails = await _instance.GetTrails(searchValue, page, pageSize);

        //Assert
        Assert.True(trails.Intersect(trailsList).Count() == trails.Count());
    }
}