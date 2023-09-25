using Hike.Data.Entities;

namespace Hike.Logic.Services.Interfaces;

public interface ITrailService
{
    Task<TrailModel?> GetTrailById(Guid id);

    Task<IEnumerable<TrailModel>> SearchTrailByTitle(string searchValue);
}