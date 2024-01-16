using Hike.Domain.Entities;

namespace Hike.Domain.Repositories.Interfaces;

public interface ITrailRepository
{
    Task<TrailEntity?> GetTrailById(Guid id);

    Task<IEnumerable<TrailEntity>> SearchTrailByTitle(string? searchValue, int page, int pageSize);
    
    Task<IEnumerable<TrailEntity>> SearchTrailByUser(string userId, int page, int pageSize);
    
    Task<IEnumerable<TrailEntity>> SearchFavoriteTrailsByUser(string userId, int page, int pageSize);

    Task<bool> AddTrail(TrailEntity entity);
    
    Task<bool> AddTrailToFavorites(string userId, Guid id);
    Task<bool> RemoveTrailFromFavorites(string userId, Guid id);

    Task<bool> DeleteTrail(Guid id);
}