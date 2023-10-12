using Hike.Logic.Models;

namespace Hike.Logic.Repositories.Interfaces;

public interface ITrailRepository
{
    Task<TrailModel?> GetTrailById(Guid id);

    Task<IEnumerable<TrailModel>> SearchTrailByTitle(string? searchValue, int page, int pageSize);
    
    Task<bool> AddTrail(TrailModel model);
}