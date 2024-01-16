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
    private readonly IRouteService _routeService;

    public TrailService(ITrailRepository trailRepository, IMapper mapper, IAuthenticationUtility authenticationUtility,
        IRouteService routeService)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
        _authenticationUtility = authenticationUtility;
        _routeService = routeService;
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

    public async Task<TrailResponse> AddTrail(AddTrailRequest request)
    {
        if (request.Description?.Length > 255)
            return new TrailResponse(FailureType.User,
                "Description has too many characters. Only 255 characters allowed");

        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new TrailResponse(FailureType.User, "Authentication failure");
        }

        var entity = _mapper.Map<TrailEntity>(request);

        entity.OwnerUserId = userId;
        
        var lineString = await _routeService.GetRoute(request.Start, request.End);

        if (lineString == null)
            return new TrailResponse(FailureType.Server, "Route Api failure");

        entity.LineString = lineString;
        
        if (!await _trailRepository.AddTrail(entity))
            return new TrailResponse(FailureType.Server, "Database failure");

        return new TrailResponse();
    }

    public async Task<TrailResponse> AddTrailToFavorites(Guid id)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new TrailResponse(FailureType.User, "Authentication failure");
        }

        if (!await _trailRepository.AddTrailToFavorites(userId, id))
            return new TrailResponse(FailureType.Server, "Database failure");

        return new TrailResponse();
    }

    public async Task<TrailResponse> RemoveTrailFromFavorites(Guid id)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new TrailResponse(FailureType.User, "Authentication failure");
        }

        if (!await _trailRepository.RemoveTrailFromFavorites(userId, id))
            return new TrailResponse(FailureType.Server, "Database failure");

        return new TrailResponse();
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _trailRepository.DeleteTrail(id);
    }
}