
using Hike.Domain.Entities;

namespace Hike.Domain.Repositories.Interfaces;

public interface ITrailRepository
{
    Task<TrailEntity?> GetTrailById(Guid id);

    Task<IEnumerable<TrailEntity>> SearchTrailByTitle(string? searchValue, int page, int pageSize);
    
    Task<bool> AddTrail(TrailEntity entity);

    Task<bool> DeleteTrail(Guid id);
}