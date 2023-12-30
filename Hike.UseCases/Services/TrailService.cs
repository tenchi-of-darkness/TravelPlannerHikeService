using AutoMapper;
using Hike.Domain.Entities;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Hike.UseCases.Utilities;

namespace Hike.UseCases.Services;

public class TrailService : ITrailService
{
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly IAuthenticationUtility _authenticationUtility;

    public TrailService(ITrailRepository trailRepository, IMapper mapper, IAuthenticationUtility authenticationUtility)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
        _authenticationUtility = authenticationUtility;
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
        
        if (!await _trailRepository.AddTrail(entity))
            return new AddTrailResponse(FailureType.Server, "Database failure");

        return new AddTrailResponse();
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _trailRepository.DeleteTrail(id);
    }
}