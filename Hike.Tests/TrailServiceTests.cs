using AutoMapper;
using Hike.API.Mappings;
using Hike.Data.Mappings;
using Hike.Domain.Entities;
using Hike.Domain.Enum;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Mappings;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services;
using Moq;
using NetTopologySuite.Geometries;

namespace Hike.Tests;

public class TrailServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ITrailRepository> _mockTrailRepo = new(MockBehavior.Strict);

    public TrailServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TrailMapping>();
            cfg.AddProfile<TrailDataMapping>();

            cfg.AddProfile<TrailApiMapping>();
        });
        _mapper = new Mapper(config);
    }

    private TrailService CreateService()
    {
        return new TrailService(_mockTrailRepo.Object, _mapper);
    }

    [Fact]
    public async Task CanGetTrailById()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.GetTrailById(It.IsAny<Guid>()))
            .Returns(Task.FromResult((TrailEntity?)new TrailEntity
            {
                Description = "Test",
                Difficulty = TrailDifficulty.Beginner,
                DistanceInMeters = 5,
                Id = Guid.NewGuid(),
                LineString = new LineString(new[] { new Coordinate(1, 1), new Coordinate(1, 2) }),
                Rating = 4,
                Title = "Test",
                LocationName = "Eindhoven"
            }));
        var trailService = CreateService();
        var id = Guid.NewGuid();

        // Act
        var response = await trailService.GetTrailById(id);

        // Assert
        _mockTrailRepo.Verify(repo => repo.GetTrailById(id), Times.Once);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task CanGetTrails()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.SearchTrailByTitle(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<TrailEntity>
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
            });
        var trailService = CreateService();
        var trailTitle = "Test";
        var pageIndex = 1;
        var pageSize = 1;

        // Act
        var response = await trailService.GetTrails(trailTitle, pageIndex, pageSize);

        // Assert
        _mockTrailRepo.Verify(repo => repo.SearchTrailByTitle(trailTitle, pageIndex, pageSize), Times.Once);
        Assert.NotEmpty(response.Trails);
    }

    [Fact]
    public async Task CanAddTrail_Success()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.AddTrail(It.IsAny<TrailEntity>())).Returns(Task.FromResult(true));
        var trailService = CreateService();
        var lineString = LineString.Empty;
        var newTrailRequest = new AddTrailRequest(lineString, 4.5f, TrailDifficulty.Beginner, "Example Title",
            "Example Description", "location name", 20);
        // Act
        var result = await trailService.AddTrail(newTrailRequest);

        // Assert
        _mockTrailRepo.Verify(repo => repo.AddTrail(It.IsAny<TrailEntity>()), Times.Once);
        Assert.True(result.FailureReason == null);
    }
    
    [Fact]
    public async Task CanAddTrail_DescriptionTooLong()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.AddTrail(It.IsAny<TrailEntity>())).Returns(Task.FromResult(true));
        var trailService = CreateService();
        var lineString = LineString.Empty;
        var description = "";
        for (int i = 0; i < 256; i++)
        {
            description += 't';
        }
        
        var newTrailRequest = new AddTrailRequest(lineString, 4.5f, TrailDifficulty.Beginner, "Example Title",
            description, "location name", 20);
        // Act
        var result = await trailService.AddTrail(newTrailRequest);

        // Assert
        _mockTrailRepo.Verify(repo => repo.AddTrail(It.IsAny<TrailEntity>()), Times.Never);
        Assert.True(result.FailureType == FailureType.User);
    }
    
    [Fact]
    public async Task CanAddTrail_DatabaseFailure()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.AddTrail(It.IsAny<TrailEntity>())).Returns(Task.FromResult(false));
        var trailService = CreateService();
        var lineString = LineString.Empty;
        var newTrailRequest = new AddTrailRequest(lineString, 4.5f, TrailDifficulty.Beginner, "Example Title",
            "Example Description", "location name", 20);
        // Act
        var result = await trailService.AddTrail(newTrailRequest);

        // Assert
        _mockTrailRepo.Verify(repo => repo.AddTrail(It.IsAny<TrailEntity>()), Times.Once);
        Assert.True(result.FailureType == FailureType.Server);

    }

    [Fact]
    public async Task CanDeleteTrail()
    {
        // Arrange
        _mockTrailRepo.Setup(repo => repo.DeleteTrail(It.IsAny<Guid>())).Returns(Task.FromResult(true));
        var trailService = CreateService();
        var id = Guid.NewGuid();

        // Act
        var result = await trailService.DeleteTrail(id);

        // Assert
        _mockTrailRepo.Verify(repo => repo.DeleteTrail(id), Times.Once);
        Assert.True(result);
    }
}