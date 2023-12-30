using System.Net.Http.Json;
using Hike.API;
using Hike.API.DTO;
using Hike.Data.DbContext;
using Hike.Domain.Enum;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NetTopologySuite.Geometries;

namespace Hike.IntegrationTests;

public class HikeIntegrationTests
    : IClassFixture<WebApplicationFactory<HikeApiProgram>>
{
    private readonly WebApplicationFactory<HikeApiProgram> _factory;

    public HikeIntegrationTests(WebApplicationFactory<HikeApiProgram> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("IntegrationTest"));;
    }

    [Fact]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/trail",
            new AddTrailRequest(new LineString(new[] { new Coordinate(1, 1), new Coordinate(1, 2) }), 3f,
                TrailDifficulty.Beginner, "", "", "", 1L),
            Default.JsonSerializerOptions);


        var getTrailsResponse = await client.GetAsync("/api/trail?Page=1&PageSize=15");

        var test = await getTrailsResponse.Content.ReadAsStringAsync();

        var trails =
            await getTrailsResponse.Content.ReadFromJsonAsync<IEnumerable<TrailDTO>>(Default.JsonSerializerOptions);

        
        // Assert
        response.EnsureSuccessStatusCode();
        getTrailsResponse.EnsureSuccessStatusCode();
        Assert.NotNull(trails);
        Assert.True(trails.Any());
    }
}