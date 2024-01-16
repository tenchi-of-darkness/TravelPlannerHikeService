using System.Net;
using System.Net.Http.Json;
using Hike.API;
using Hike.API.DTO;
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
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("IntegrationTest"));
    }

    [Fact]
    public async Task GetTrails_AddTrail_Success()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/trail",
            new AddTrailRequest(new Point(55, 8), new Point(55, 9), 3f,
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
            new AddTrailRequest(new Point(55, 8), new Point(55, 9), 3f,
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

    [Fact]
    public async Task AddToFavorites_RemoveFromFavorites_GetFavorites_Success()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var addTrailResponse = await client.PostAsJsonAsync("/api/trail",
            new AddTrailRequest(new Point(55, 8), new Point(55, 9), 3f, TrailDifficulty.Beginner, "", "", "", 1L),
            Default.JsonSerializerOptions);
        var getTrailsResponse = await client.GetAsync("/api/trail/user?Page=1&PageSize=15");
        var trail =
            await getTrailsResponse.Content.ReadFromJsonAsync<IEnumerable<TrailDTO>>(Default.JsonSerializerOptions);
        Guid trailId = trail!.First().Id;
        var addToFavoritesResponse = await client.PostAsync($"/api/trail/{trailId}/favorite", null);
        var getFavoritesResponse = await client.GetAsync("/api/trail/favorite");
        var favorites =
            await getFavoritesResponse.Content.ReadFromJsonAsync<IEnumerable<TrailDTO>>(Default.JsonSerializerOptions);
        var removeFromFavoritesResponse = await client.DeleteAsync($"/api/trail/{trailId}/favorite");
        // Assert
        addTrailResponse.EnsureSuccessStatusCode();
        addToFavoritesResponse.EnsureSuccessStatusCode();
        removeFromFavoritesResponse.EnsureSuccessStatusCode();
        getFavoritesResponse.EnsureSuccessStatusCode();
        Assert.Contains(trailId, favorites?.Select(x => x.Id) ?? Array.Empty<Guid>());
    }
}