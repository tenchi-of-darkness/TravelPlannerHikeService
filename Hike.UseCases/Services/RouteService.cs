using System.Text.Json;
using System.Text.Json.Serialization;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

namespace Hike.UseCases.Services;

public class RouteService : IRouteService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly HttpClient _client = new();

    public RouteService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<LineString?> GetRoute(Point start, Point end)
    {
        if (_webHostEnvironment.IsEnvironment("IntegrationTest"))
        {
            return new LineString(new[] { new Coordinate(55, 8), new Coordinate(55, 9) });
        }

        var openRouteServiceApiKey = _configuration["OpenRouteService:ApiKey"] ?? throw new Exception();

        var startString =
            $"{start.Y.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},{start.X.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)}";
        var endString =
            $"{end.Y.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},{end.X.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)}";

        var url =
            $"https://api.openrouteservice.org/v2/directions/foot-hiking?api_key={openRouteServiceApiKey}&start={startString}&end={endString}";

        Console.WriteLine(url);

        HttpResponseMessage response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new GeoJsonConverterFactory());
        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(responseBody, options);
        var lineString = featureCollection?.Where(x => x.Geometry is LineString).Select(x => (LineString)x.Geometry)
            .FirstOrDefault();
        return lineString;
    }
}