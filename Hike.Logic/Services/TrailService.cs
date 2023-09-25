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

    public async Task<IEnumerable<TrailModel>> SearchTrailByTitle(string searchValue)
    {
        return await _trailRepository.SearchTrailByTitle(searchValue);
    }
}