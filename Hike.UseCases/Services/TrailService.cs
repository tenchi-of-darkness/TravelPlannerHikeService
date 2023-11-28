using AutoMapper;
using Hike.Domain.Entities;
using Hike.Domain.Repositories.Interfaces;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;

namespace Hike.UseCases.Services;

public class TrailService : ITrailService
{
    private readonly ITrailRepository _trailRepository;
    private readonly IMapper _mapper;

    public TrailService(ITrailRepository trailRepository, IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
    }
    public async Task<TrailEntity?> GetTrailById(Guid id)
    {
        return await _trailRepository.GetTrailById(id);
    }

    public async Task<IEnumerable<TrailEntity>> GetTrails(string? searchValue, int page, int pageSize)
    {
        return await _trailRepository.SearchTrailByTitle(searchValue, page, pageSize);
    }

    public async Task<AddTrailResponse> AddTrail(AddTrailRequest request)
    {
        if (request.Description?.Length > 255)
        {
            return new AddTrailResponse(FailureType.User,"Description has too many characters. Only 255 characters allowed");
        }

        if (!await _trailRepository.AddTrail(_mapper.Map<TrailEntity>(request)))
        {
            return new AddTrailResponse(FailureType.Server,"Database failure");
        }

        return new AddTrailResponse();
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _trailRepository.DeleteTrail(id);
    }
}