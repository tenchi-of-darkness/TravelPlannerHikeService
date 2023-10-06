using Hike.Data.Entities;
using Hike.Logic.Repositories.Interfaces;
using Hike.Logic.Services.Interfaces;

namespace Hike.Logic.Services;

public class TrailService : ITrailService
{
    private readonly ITrailRepository _trailRepository;

    public TrailService(ITrailRepository trailRepository)
    {
        _trailRepository = trailRepository;
    }
    public async Task<TrailModel?> GetTrailById(Guid id)
    {
        return await _trailRepository.GetTrailById(id);
    }

    public async Task<IEnumerable<TrailModel>> GetTrails(string? searchValue, int page, int pageSize)
    {
        return await _trailRepository.SearchTrailByTitle(searchValue, page, pageSize);
    }

    public async Task<bool> AddTrail(TrailModel model)
    {
        return await _trailRepository.AddTrail(model);
    }
}