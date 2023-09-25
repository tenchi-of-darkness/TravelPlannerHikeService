using Hike.Data.Entities;

namespace Hike.Logic.Repositories.Interfaces;

public interface ITrailRepository
{
    Task<TrailModel?> GetTrailById(Guid id);

    Task<IEnumerable<TrailModel>> SearchTrailByTitle(string searchValue);
}