using System.Net;
using System.Net.Http.Json;
using Hike.API;
using Hike.API.DTO;
using Hike.Domain.Enum;
using Hike.UseCases.Requests.Trail;
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
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("IntegrationTest"));
    }

    [Fact]
    public async Task GetTrails_AddTrail_Success()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/trail",
            new AddTrailRequest(new Point(55,8), new Point(55,9), 3f,
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

    [Fact]
    public async Task AddTrail_DeleteTrail_GetById_NotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/trail",
            new AddTrailRequest(new Point(55,8), new Point(55,9), 3f,
                TrailDifficulty.Beginner, "", "", "", 1L),
            Default.JsonSerializerOptions);
        var getTrailsResponse = await client.GetAsync("/api/trail?Page=1&PageSize=15");
        var trail =
            await getTrailsResponse.Content.ReadFromJsonAsync<IEnumerable<TrailDTO>>(Default.JsonSerializerOptions);
        Guid id = trail!.First().Id;
        var deleteResponse = await client.DeleteAsync($"/api/trail/{id}");
        var getTrailResponse = await client.GetAsync($"/api/trail/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        getTrailsResponse.EnsureSuccessStatusCode();
        deleteResponse.EnsureSuccessStatusCode();
        Assert.True(getTrailResponse.StatusCode == HttpStatusCode.NotFound);
    }
    
    
}