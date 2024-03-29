﻿using AutoMapper;
using Hike.Domain.Entities;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Hike.UseCases.Utilities;

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
        var response = _mapper.Map<GetTrailResponse?>(await _trailRepository.GetTrailById(id));

        var userId = _authenticationUtility.GetUserId();
        if (userId == null) return _mapper.Map<GetTrailResponse?>(response);
        
        var favorites = await _trailRepository.SearchFavoriteTrailsByUser(userId, 1, 9999);

        if (response != null && favorites.Any(x => x.Id == response.Id))
        {
            response = response with { IsFavorite = true };
        }

        return _mapper.Map<GetTrailResponse?>(response);
    }

    public async Task<GetTrailsResponse> GetTrails(string? searchValue, int page, int pageSize)
    {
        var entities = await _trailRepository.SearchTrailByTitle(searchValue, page, pageSize);
        var responseModels = _mapper.Map<GetTrailResponse[]>(entities);

        var userId = _authenticationUtility.GetUserId();

        if (userId == null) return new GetTrailsResponse(responseModels);

        var favorites = await _trailRepository.SearchFavoriteTrailsByUser(userId, 1, 9999);

        return new GetTrailsResponse(responseModels
            .Select(responseModel => responseModel with
            {
                IsFavorite = favorites.Any(x => x.Id == responseModel.Id)
            })
            .ToArray());
    }

    public async Task<GetTrailsResponse?> GetUserTrails(int page, int pageSize)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return null;
        }

        var entities = await _trailRepository.SearchTrailByUser(userId, page, pageSize);
        var responseModels = _mapper.Map<GetTrailResponse[]>(entities);

        var favorites = await _trailRepository.SearchFavoriteTrailsByUser(userId, 1, 9999);

        return new GetTrailsResponse(responseModels
            .Select(responseModel => responseModel with
            {
                IsFavorite = favorites.Any(x => x.Id == responseModel.Id)
            })
            .ToArray());
    }

    public async Task<GetTrailsResponse?> GetUserFavoriteTrails(int page, int pageSize)
    {
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return null;
        }

        var entities = await _trailRepository.SearchFavoriteTrailsByUser(userId, page, pageSize);
        return new GetTrailsResponse(
            _mapper.Map<GetTrailResponse[]>(entities).Select(x => x with { IsFavorite = true }));
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

    public async Task<TrailResponse> UpdateTrail(UpdateTrailRequest request)
    {
        if (request.Description?.Length > 255)
            return new TrailResponse(FailureType.User,
                "Description has too many characters. Only 255 characters allowed");
        
        var userId = _authenticationUtility.GetUserId();

        if (userId == null)
        {
            return new TrailResponse(FailureType.User, "Authentication failure");
        }
        
        var trail = await GetTrailById(request.Id);

        if (trail == null || trail.OwnerUserId != userId)
        {
            return new TrailResponse(FailureType.User, "Not found or no access");
        }

        var entity = _mapper.Map<TrailEntity>(request);

        entity.OwnerUserId = userId;
        
        var lineString = await _routeService.GetRoute(request.Start, request.End);

        if (lineString == null)
            return new TrailResponse(FailureType.Server, "Route Api failure");

        entity.LineString = lineString;
        
        if (!await _trailRepository.UpdateTrail(entity))
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