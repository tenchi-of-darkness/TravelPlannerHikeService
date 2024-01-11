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
using Hike.UseCases.Services.Interfaces;
using Hike.UseCases.Utilities;
using Moq;
using NetTopologySuite.Geometries;

namespace Hike.Tests;

public class TrailServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ITrailRepository> _mockTrailRepo = new(MockBehavior.Strict);
    private readonly Mock<IAuthenticationUtility> _authenticationUtilityMock = new (MockBehavior.Strict);
    private const string UserId = "test";
    private readonly Mock<IRouteService> _routeServiceMock = new (MockBehavior.Strict);

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
        _authenticationUtilityMock.Setup(x => x.GetUserId()).Returns(UserId);
        return new TrailService(_mockTrailRepo.Object, _mapper, _authenticationUtilityMock.Object, _routeServiceMock.Object);
    }

    [Fact]
    public async Task CanGetTrailById()
    {
        // Arrange
        var trail = new TrailEntity()
        {
            Description = "Test",
            Difficulty = TrailDifficulty.Beginner,
            DistanceInMeters = 5,
            Id = Guid.NewGuid(),
            LineString = new LineString(new[] { new Coordinate(1, 1), new Coordinate(1, 2) }),
            Rating = 4,
            Title = "Test",
            LocationName = "Eindhoven"
        };
        _mockTrailRepo.Setup(repo => repo.GetTrailById(It.IsAny<Guid>()))
            .ReturnsAsync(trail);
        var trailService = CreateService();
        var id = Guid.NewGuid();
        var response = _mapper.Map<GetTrailResponse>(trail);

        // Act
        var result = await trailService.GetTrailById(id);

        // Assert
        Assert.Equal(result, response);
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
        _routeServiceMock.Setup(x => x.GetRoute(It.IsAny<Point>(), It.IsAny<Point>())).ReturnsAsync(LineString.Empty);
        _mockTrailRepo.Setup(repo => repo.AddTrail(It.IsAny<TrailEntity>())).Returns(Task.FromResult(true));
        var trailService = CreateService();
        var newTrailRequest = new AddTrailRequest(new Point(55,8), new Point(55,9), 4.5f, TrailDifficulty.Beginner, "Example Title",
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

        var newTrailRequest = new AddTrailRequest(new Point(55,8), new Point(55,9), 4.5f, TrailDifficulty.Beginner, "Example Title",
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
        _routeServiceMock.Setup(x => x.GetRoute(It.IsAny<Point>(), It.IsAny<Point>())).ReturnsAsync(LineString.Empty);
        var trailService = CreateService();
        var lineString = LineString.Empty;
        var newTrailRequest = new AddTrailRequest(new Point(55,8), new Point(55,9), 4.5f, TrailDifficulty.Beginner, "Example Title",
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