using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Hike.Domain.Entities;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Hike.UseCases.Utilities;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

namespace Hike.UseCases.Services;

public class TrailService : ITrailService
{
    private readonly HttpClient _client = new();
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly IAuthenticationUtility _authenticationUtility;
    private readonly string _openRouteServiceApiKey;

    public TrailService(ITrailRepository trailRepository, IMapper mapper, IAuthenticationUtility authenticationUtility,
        IConfiguration configuration)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
        _authenticationUtility = authenticationUtility;
        _openRouteServiceApiKey = configuration["OpenRouteService:ApiKey"] ?? throw new Exception();
    }

    public async Task<GetTrailResponse?> GetTrailById(Guid id)
    {
        var entities = await _trailRepository.GetTrailById(id);
        return _mapper.Map<GetTrailResponse?>(entities);
    }

    public async Task<GetTrailsResponse> GetTrails(string? searchValue, int page, int pageSize)
    {
        var entities = await _trailRepository.SearchTrailByTitle(searchValue, page, pageSize);
        return new GetTrailsResponse(_mapper.Map<GetTrailResponse[]>(entities));
    }

    public async Task<GetTrailsResponse?> GetUserTrails(int page, int pageSize)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return null;
        }

        var entities = await _trailRepository.SearchTrailByUser(userId, page, pageSize);
        return new GetTrailsResponse(_mapper.Map<GetTrailResponse[]>(entities));
    }

    public async Task<GetTrailsResponse?> GetUserFavoriteTrails(int page, int pageSize)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return null;
        }

        var entities = await _trailRepository.SearchFavoriteTrailsByUser(userId, page, pageSize);
        return new GetTrailsResponse(_mapper.Map<GetTrailResponse[]>(entities));
    }

    public async Task<AddTrailResponse> AddTrail(AddTrailRequest request)
    {
        if (request.Description?.Length > 255)
            return new AddTrailResponse(FailureType.User,
                "Description has too many characters. Only 255 characters allowed");

        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new AddTrailResponse(FailureType.User, "Authentication failure");
        }

        var entity = _mapper.Map<TrailEntity>(request);

        entity.OwnerUserId = userId;

        var start = $"{request.Start.Y},{request.Start.X}";
        var end = $"{request.End.Y},{request.End.X}";

        var url =
            $"https://api.openrouteservice.org/v2/directions/foot-hiking?api_key={_openRouteServiceApiKey}&start={start}&end={end}";

        HttpResponseMessage response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new AddTrailResponse(FailureType.Server, "Routing api failure status code: " + response.StatusCode);
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
        if (lineString == null)
        {
            return new AddTrailResponse(FailureType.Server, "Routing api failure status code: " + response.StatusCode);
        }

        entity.LineString = lineString;

        if (!await _trailRepository.AddTrail(entity))
            return new AddTrailResponse(FailureType.Server, "Database failure");

        return new AddTrailResponse();
    }

    public async Task<AddTrailResponse> AddTrailToFavorites(Guid id)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new AddTrailResponse(FailureType.User, "Authentication failure");
        }

        if (!await _trailRepository.AddTrailToFavorites(userId, id))
            return new AddTrailResponse(FailureType.Server, "Database failure");

        return new AddTrailResponse();
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _trailRepository.DeleteTrail(id);
    }
}