using Hike.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

// public class IntegrationTests 
//     : IClassFixture<WebApplicationFactory<HikeApiProgram>>
// {
//     private readonly WebApplicationFactory<HikeApiProgram> _factory;
//
//     public IntegrationTests(WebApplicationFactory<HikeApiProgram> factory)
//     {
//         _factory = factory;
//     }
//
//     [Theory]
//     [InlineData("/")]
//     [InlineData("/api/trail")]
//     [InlineData("/t")]
//     [InlineData("/t2")]
//     [InlineData("/Contact")]
//     public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
//     {
//         // Arrange
//         var client = _factory.CreateClient();
//
//         // Act
//         var response = await client.GetAsync(url);
//
//         // Assert
//         response.EnsureSuccessStatusCode(); // Status Code 200-299
//         Assert.Equal("application/json; charset=utf-8", 
//             response.Content.Headers.ContentType.ToString());
//     }
// }