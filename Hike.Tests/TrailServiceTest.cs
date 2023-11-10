using Hike.Logic.Entities;
using Hike.Logic.Repositories.Interfaces;
using Hike.Logic.Services;
using Hike.Logic.Services.Interfaces;
using Moq;
using NetTopologySuite.Geometries;
// ReSharper disable PossibleMultipleEnumeration

namespace Hike.Tests;

public class TrailServiceTest
{
    private readonly Mock<ITrailRepository> _trailRepositoryMock = new();
    private readonly ITrailService _instance;

    public TrailServiceTest()
    {
        _instance = new TrailService(_trailRepositoryMock.Object);
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
                LineString = new LineString(new []{new Coordinate(1, 1), new Coordinate(1, 2)}),
                Rating = 4,
                Title = "Test",
                LocationName = "Eindhoven"
            }
        };

        _trailRepositoryMock.Setup(x => x.SearchTrailByTitle(searchValue, page, pageSize)).ReturnsAsync(trailsList);

        //Act
        IEnumerable<TrailEntity> trails = await _instance.GetTrails(searchValue, page, pageSize);

        //Assert
        Assert.True(trails.Intersect(trailsList).Count()==trails.Count());
    }
}